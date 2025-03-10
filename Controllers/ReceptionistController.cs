using CMSv2026WebApp.Models;
using CMSv2026WebApp.Services;
using CMSv2026WebApp.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CMSv2026WebApp.Controllers
{
    public class ReceptionistController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;

        public ReceptionistController(IPatientService patientService, IAppointmentService appointmentService)
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
        }

        public IActionResult Index(string searchTerm = null)
        {
            IEnumerable<Patient> patients;

            if (string.IsNullOrEmpty(searchTerm))
            {
                // If no search term, return all patients or an empty list
                patients = _patientService.SearchPatients(string.Empty); // Or fetch all patients if you have such method
            }
            else
            {
                patients = _patientService.SearchPatients(searchTerm);
            }
            TempData["SuccessMessage"] = "Patient added successfully!";
            TempData["ShowToast"] = true;  // Ensures the toast shows only once

            return View(patients);
        }

        [HttpGet]
        public IActionResult AddPatient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPatient(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _patientService.AddPatient(patient);
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        [HttpGet]
        public IActionResult EditPatient(int id)
        {
            var patient = _patientService.GetPatientById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        [HttpPost]
        public IActionResult EditPatient(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _patientService.UpdatePatient(patient);
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        [HttpPost]
        public IActionResult DeactivatePatient(int id)
        {
            _patientService.DeactivatePatient(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult SearchPatients(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction("Index"); // Redirect to show all patients
            }

            var patients = _patientService.SearchPatients(searchTerm);
            return View("Index", patients);
        }

        [HttpGet]
        public IActionResult BookAppointment(int patientId)
        {
            // Fetch the patient details
            var patient = _patientService.GetPatientById(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            // Fetch the list of specializations (departments)
            var specializations = _appointmentService.GetSpecializations();

            if (specializations == null)
            {
                specializations = new List<Specialization>(); // Handle null case
            }

            // Pass the data to the view
            ViewBag.Patient = patient;
            ViewBag.Specializations = specializations;

            return View(patient); // Pass the patient model to the view
        }

        [HttpGet]
        public IActionResult GetAvailableDoctors(int specializationId)
        {
            Console.WriteLine($"GetAvailableDoctors called with specializationId: {specializationId}");

            var doctors = _appointmentService.GetAvailableDoctors(specializationId, DateTime.Now);
            Console.WriteLine($"Doctors returned: {doctors?.Count ?? 0}");

            // Log the data for debugging
            foreach (var doctor in doctors)
            {
                Console.WriteLine($"DoctorId: {doctor.DoctorId}, DoctorName: {doctor.Name}, ConsultationFee: {doctor.ConsultationFee}");
            }

            return Json(doctors);
        }

        [HttpGet]
        public IActionResult GetAvailableTimeSlots(int doctorId, DateTime appointmentDate)
        {
            var timeSlots = _appointmentService.GetAvailableTimeSlots(doctorId, appointmentDate);

            // Convert TimeSpan to a formatted string (e.g., "hh:mm tt")
            var formattedTimeSlots = timeSlots
                .Select(t => new { time = t.ToString(@"hh\:mm") }) // Use 24-hour format or @"hh\:mm tt" for 12-hour format
                .ToList();

            return Json(formattedTimeSlots);
        }

        [HttpGet]
        public IActionResult CheckExistingAppointment(int patientId, int doctorId, DateTime appointmentDate)
        {
            bool hasExistingAppointment = _appointmentService.HasExistingAppointment(patientId, doctorId, appointmentDate);
            return Json(new { hasExistingAppointment });
        }

        [HttpPost]
        public IActionResult BookAppointment(int patientId, int doctorId, DateTime appointmentDate, TimeSpan appointmentTime)
        {
            // Fetch the patient details
            var patient = _patientService.GetPatientById(patientId);
            if (patient == null)
            {
                TempData["ErrorMessage"] = "Patient not found.";
                return RedirectToAction("Index");
            }

            // Fetch the doctor details
            var doctor = _appointmentService.GetDoctorById(doctorId);
            if (doctor == null)
            {
                TempData["ErrorMessage"] = "Doctor not found.";
                return RedirectToAction("Index");
            }

            // Check if the patient already has an appointment with the same doctor on the same day
            bool hasExistingAppointment = _appointmentService.HasExistingAppointment(patientId, doctorId, appointmentDate);
            if (hasExistingAppointment)
            {
                TempData["ErrorMessage"] = "You already have an appointment with this doctor on the selected date please select other date.";
                return RedirectToAction("Index");
            }

            // Combine the date and time into a single DateTime object
            DateTime combinedDateTime = appointmentDate.Date + appointmentTime;

            // Log the combinedDateTime for debugging
            Console.WriteLine($"Combined DateTime: {combinedDateTime}");

            // Generate token based on time slot
            int tokenNumber = CalculateTokenNumber(appointmentTime);

            // Book the appointment
            var appointment = _appointmentService.BookAppointment(patientId, doctorId, combinedDateTime, appointmentTime);

            if (appointment == null)
            {
                TempData["ErrorMessage"] = "Error booking appointment.";
                return RedirectToAction("Index");
            }

            // Generate ConsultationBill for the new appointment
            var consultationBill = _appointmentService.GenerateConsultationBill(appointment.AppointmentId);
            if (consultationBill == null)
            {
                TempData["ErrorMessage"] = "Error generating consultation bill.";
                return RedirectToAction("Index");
            }

            // Prepare a payment preview model
            var paymentViewModel = new PaymentViewModel
            {
                AppointmentId = appointment.AppointmentId, // Assign the actual AppointmentId
                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime,
                PatientName = patient.PatientName,
                MobileNumber = patient.MobileNumber,
                RegistrationId = patient.RegistrationId,
                BloodGroup = patient.BloodGroup,
                DoctorName = doctor.Name,
                ConsultationFee = doctor.ConsultationFee
            };

            return View("PaymentPreview", paymentViewModel);
        }

        [HttpPost]
        public IActionResult ConfirmPayment(int appointmentId)
        {
            try
            {
                // Log the appointmentId for debugging
                Console.WriteLine($"ConfirmPayment: Received AppointmentId {appointmentId}");

                var paymentDetails = _appointmentService.ConfirmPayment(appointmentId);

                if (paymentDetails != null)
                {
                    return Json(new { tokenNumber = paymentDetails.TokenNumber });
                }

                // Log the appointmentId when the appointment is not found
                Console.WriteLine($"ConfirmPayment: Appointment not found for AppointmentId {appointmentId}");
                return Json(new { error = "Error confirming payment. Appointment not found." });
            }
            catch (Exception ex)
            {
                // Log the appointmentId and the exception details
                Console.WriteLine($"ConfirmPayment: Exception for AppointmentId {appointmentId}: {ex.Message}");
                return Json(new { error = "Error confirming payment. Please try again." });
            }
        }

        private int CalculateTokenNumber(TimeSpan appointmentTime)
        {
            // Define the start time for token calculation (9:00 AM)
            TimeSpan startTime = new TimeSpan(9, 0, 0);

            // Calculate the difference between the appointment time and the start time
            TimeSpan difference = appointmentTime - startTime;

            // Calculate the token number based on 30-minute intervals
            if (difference.TotalMinutes >= 0)
            {
                // Divide the difference by 30 minutes and add 1 to get the token number
                int tokenNumber = (int)(difference.TotalMinutes / 30) + 1;
                return tokenNumber;
            }

            // If the appointment time is before 9:00 AM, return token number 0 or handle as needed
            return 0; // You can throw an exception or handle this case differently
        }
    }
}
