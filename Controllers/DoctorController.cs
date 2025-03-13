using System.Security.Claims;
using CMSv2026WebApp.Models;
using CMSv2026WebApp.Services;
using CMSv2026WebApp.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CMSv2026WebApp.Controllers
{
    public class DoctorController : Controller
    {
        //Fields
        private readonly IUserService _userService;
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;

        //DI
        public DoctorController(IUserService userService, IAppointmentService appointmentService, IPatientService patientService, IDoctorService doctorService)
        {
            _userService = userService;
            _patientService = patientService;
            _appointmentService = appointmentService;
            _doctorService = doctorService;
        }

        //GET
        public IActionResult Index()
        {
            if (!IsUserInRole(2))
            {
                return RedirectToAction("Login", "Accounts");
            }

            var viewModel = new DoctorViewModel
            {
                Staffs = _userService.GetAllStaffs(),
                Roles = _userService.GetAllStaffRoles(),
                Patients = _patientService.GetAllthePatients(),
                Appointments = _appointmentService.GetTodaysAppointments()

            };

            ViewData["PageTitle"] = "Doctor";
            ViewBag.Role = "Doctor";
            ViewBag.Roles = viewModel.Roles;
            return View(viewModel);
        }
        public IActionResult PatientHistory(int patientId)
        {
            var patientHistory = _doctorService.GetPatientConsultationHistory(patientId);
            if (patientHistory == null)
            {
                return NotFound();
            }

            return View(patientHistory);
        }

        // GET: Doctor/LabResults/
        public IActionResult LabResults(int appointmentId)
        {
            var labResults = _doctorService.GetThePatientLabResults(appointmentId);
            if (labResults == null)
            {
                return NotFound();
            }

            return View(labResults);
        }
        //GET: Doctor/PatientDetails/
        public IActionResult PatientDetails(int patientId)
        {
            var patient = _patientService.GetPatientById(patientId);
            if (patient == null)
            {
                return NotFound();
            }
            var appointment = _appointmentService.GetAppointmentByPatientAndDoctor(patientId, GetLoggedInDoctorId());
            if (appointment == null)
            {
                return NotFound();
            }
            var viewModel = new PatientViewModel
            {
                Patient = patient,
                Medicines = _doctorService.GetAllMedicines(),
                LabTests = _doctorService.GetAllLabTests(),
                Appointment = appointment
            };

            return View(viewModel);
        }

        private int GetLoggedInDoctorId()
        {
            var staff = _userService.GetStaffByRoleId(2);
            if (staff == null)
            {
                throw new Exception("Doctor with RoleId 2 not found.");
            }

            var doctor = _userService.GetDoctorByStaffId(staff.StaffId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found for the given StaffId.");
            }

            return doctor.DoctorId;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitConsultation([FromBody] ConsultationViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new { success = false, message = "Invalid consultation data." });
                }

                // Retrieve the appointment for the specific patient and doctor
                var appointment = _appointmentService.GetAppointmentByPatientAndDoctor(model.PatientId, GetLoggedInDoctorId());
                if (appointment == null)
                {
                    return NotFound(new { success = false, message = "No appointment found for the specified patient and doctor." });
                }

                var consultation = new Consultation
                {
                    AppointmentId = appointment.AppointmentId,
                    Symptoms = model.Symptoms,
                    Diagnosis = model.Diagnosis,
                    Notes = model.Notes,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };

                _patientService.AddConsultations(consultation);

                return Json(new { success = true, message = "Consultation submitted successfully!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SubmitConsultation: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while submitting consultation." });
            }
        }


        [HttpGet]
        public JsonResult GetMedicineSuggestions(string term)
        {
            var medicines = _doctorService.GetMedicineNamesByTerm(term);
            return Json(medicines);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitPrescriptions([FromBody] PrescriptionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid prescription data.");
            }

            var appointment = _appointmentService.GetAppointmentByPatientAndDoctor(model.PatientId, GetLoggedInDoctorId());
            if (appointment == null)
            {
                return NotFound("No appointment found for the specified patient and doctor.");
            }

            foreach (var prescription in model.Prescriptions)
            {
                if (prescription.MedicineName == null)
                {
                    return BadRequest($"Medicine details are missing for prescription with dosage {prescription.Dosage}.");
                }

                var newPrescription = new MedicinePrescription
                {
                    AppointmentId = appointment.AppointmentId,
                    MedicineName = prescription.MedicineName,
                    Dosage = prescription.Dosage,
                    Frequency = prescription.Frequency,
                    Duration = prescription.Duration,
                    Medicine = new Medicine { MedicineName = prescription.MedicineName } // Ensure Medicine is initialized
                };
                _patientService.AddPrescriptions(newPrescription);
            }

            return Json(new { success = true, message = "Prescription submitted successfully!" });
        }


        [HttpGet]
        public JsonResult GetLabTestSuggestions(string term)
        {
            var labTests = _doctorService.GetAllLabTests()
                .Where(t => t.TestName.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Select(t => t.TestName)
                .ToList();
            return Json(labTests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitLabTests([FromBody] LabPrescriptionViewModel model)
        {
            // Retrieve the appointment for the specific patient and doctor
            var appointment = _appointmentService.GetAppointmentByPatientAndDoctor(model.PatientId, GetLoggedInDoctorId());
            if (appointment == null)
            {
                return NotFound("No appointment found for the specified patient and doctor.");
            }

            foreach (var labTest in model.LabTests)
            {
                // Fetch LabTestId from the LabTests table using TestName
                var labTestId = _doctorService.GetLabTestIdByName(labTest.LabTestName);
                if (labTestId == null)
                {
                    return BadRequest($"Lab test '{labTest.LabTestName}' not found.");
                }

                var labTestPrescription = new LabTestPrescription
                {
                    LabTestId = labTestId.Value,
                    LabTestName = labTest.LabTestName,
                    LabTestValue = labTest.LabTestValue,
                    Remarks = labTest.Remarks,
                    AppointmentId = appointment.AppointmentId,
                    CreatedDate = DateTime.Now
                };

                _patientService.AddLabTests(labTestPrescription);
            }

            return RedirectToAction("PatientDetails", new { patientId = model.PatientId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateConsultationStatus([FromBody] ConsultationStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid consultation status data.");
            }

            var appointment = _appointmentService.GetAppointmentById(model.AppointmentId);
            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            var validStatuses = new List<string> { "Scheduled", "Consulted", "Prescribed for Lab Test", "Completed", "Cancelled" };
            if (!validStatuses.Contains(model.Status))
            {
                return BadRequest("Invalid status value.");
            }

            try
            {
                _appointmentService.UpdateConsultationStatus(model.AppointmentId, model.Status);
                return Json(new { success = true, message = "Consultation status updated successfully!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating consultation status: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while updating consultation status." });
            }
        }




        //To Check RoleId
        private bool IsUserInRole(int requiredRoleId)
        {
            //Get the roleId from cookies
            var roleId = Request.Cookies["RoleId"];

            //If not match --> Redirect to Login

            return roleId != null
                && int.TryParse(roleId, out int userRoleId)
                && userRoleId == requiredRoleId;
        }
    }
}


