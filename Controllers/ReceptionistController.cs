////using CMSv2026WebApp.Services;
//using Microsoft.AspNetCore.Mvc;

//namespace CMSv2026WebApp.Controllers
//{
//    public class ReceptionistController : Controller
//    {
//        private readonly IAppointmentService _appointmentService;
//        private readonly IPatientService _patientService;

//        public ReceptionistController(IAppointmentService appointmentService, IPatientService patientService)
//        {
//            _appointmentService = appointmentService;
//            _patientService = patientService;
//        }

//        // Search for an existing patient by Name / Phone / Registration ID
//        [HttpGet("search-patient")]
//        public async Task<IActionResult> SearchPatient(string searchQuery)
//        {
//            var patients = await _patientService.SearchPatientAsync(searchQuery);
//            if (!patients.Any()) return NotFound("No matching patient found.");

//            return Ok(patients);
//        }

//        // Get available doctors in a selected department
//        [HttpGet("available-doctors/{departmentId}")]
//        public async Task<IActionResult> GetAvailableDoctors(int departmentId)
//        {
//            var doctors = await _appointmentService.GetAvailableDoctorsByDepartmentAsync(departmentId);
//            return Ok(doctors);
//        }

//        // Check doctor availability
//        [HttpGet("doctor-availability/{doctorId}/{date}")]
//        public async Task<IActionResult> CheckDoctorAvailability(int doctorId, DateTime date)
//        {
//            bool isAvailable = await _appointmentService.CheckDoctorAvailabilityAsync(doctorId, date);
//            return Ok(new { isAvailable });
//        }

//        // Book an appointment for a patient
//        [HttpPost("book-appointment")]
//        public async Task<IActionResult> BookAppointment([FromBody] AppointmentRequest request)
//        {
//            try
//            {
//                int appointmentId = await _appointmentService.BookAppointmentAsync(request.PatientId, request.DoctorId, request.AppointmentDate, request.TimeSlot);
//                return Ok(new { appointmentId, message = "Appointment booked successfully." });
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { error = ex.Message });
//            }
//        }

//        // Generate consultation bill after booking an appointment
//        [HttpPost("generate-bill")]
//        public async Task<IActionResult> GenerateConsultationBill([FromBody] ConsultationBillRequest request)
//        {
//            try
//            {
//                int billId = await _appointmentService.GenerateConsultationBillAsync(request.PatientId, request.DoctorId, request.AppointmentDate, request.Amount);
//                return Ok(new { billId, message = "Consultation bill generated successfully." });
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { error = ex.Message });
//            }
//        }
//    }
//}
