////using CMSv2026WebApp.Services;
//using Microsoft.AspNetCore.Mvc;

//namespace CMSv2026WebApp.Controllers
//{
//    public class DoctorController : Controller
//    {
//        private readonly IAppointmentService _appointmentService;
//        private readonly IPatientService _patientService;
//        //private readonly IConsultationService _consultationService;
//        //private readonly IPrescriptionService _prescriptionService;
//        //private readonly ILabTestService _labTestService;
//        private readonly IDoctorService _doctorService;

//        public DoctorController(
//            IAppointmentService appointmentService,
//            IPatientService patientService,
//            //IConsultationService consultationService,
//            //IPrescriptionService prescriptionService,
//            //ILabTestService labTestService,
//            IDoctorService doctorService)
//        {
//            _appointmentService = appointmentService;
//            _patientService = patientService;
//            //_consultationService = consultationService;
//            //_prescriptionService = prescriptionService;
//            //_labTestService = labTestService;
//            _doctorService = doctorService;
//        }

//        // Get today's appointments with status (upcoming & consulted)
//        [HttpGet("appointments/{doctorId}")]
//        public async Task<IActionResult> GetTodaysAppointments(int doctorId)
//        {
//            var appointments = await _appointmentService.GetTodaysAppointmentsAsync(doctorId);
//            return Ok(appointments);
//        }

//        // Search past patients of the doctor
//        [HttpGet("search-patients")]
//        public async Task<IActionResult> SearchPatients(int doctorId, string searchQuery)
//        {
//            var patients = await _patientService.SearchDoctorPatientsAsync(doctorId, searchQuery);
//            return Ok(patients);
//        }

//        // Get patient history
//        [HttpGet("patient-history/{patientId}")]
//        public async Task<IActionResult> GetPatientHistory(int patientId)
//        {
//            var history = await _consultationService.GetPatientConsultationHistoryAsync(patientId);
//            return Ok(history);
//        }

//        // Add notes/diagnosis details
//        [HttpPost("add-diagnosis")]
//        public async Task<IActionResult> AddDiagnosis([FromBody] ConsultationRequest request)
//        {
//            try
//            {
//                await _consultationService.AddConsultationAsync(request);
//                return Ok(new { message = "Diagnosis details saved successfully." });
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { error = ex.Message });
//            }
//        }

//        // Prescribe Medicines
//        [HttpPost("prescribe-medicine")]
//        public async Task<IActionResult> PrescribeMedicine([FromBody] PrescriptionRequest request)
//        {
//            try
//            {
//                await _prescriptionService.AddPrescriptionAsync(request);
//                return Ok(new { message = "Medicine prescribed successfully." });
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { error = ex.Message });
//            }
//        }

//        // Prescribe Lab Test
//        [HttpPost("prescribe-lab-test")]
//        public async Task<IActionResult> PrescribeLabTest([FromBody] LabTestRequest request)
//        {
//            try
//            {
//                await _labTestService.RequestLabTestAsync(request);
//                return Ok(new { message = "Lab test prescribed successfully." });
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { error = ex.Message });
//            }
//        }

//        // View Lab Test Results
//        [HttpGet("lab-results/{patientId}")]
//        public async Task<IActionResult> GetLabResults(int patientId)
//        {
//            var results = await _labTestService.GetPatientLabResultsAsync(patientId);
//            return Ok(results);
//        }

//        // Refer to Another Doctor
//        [HttpPost("refer-doctor")]
//        public async Task<IActionResult> ReferDoctor([FromBody] ReferralRequest request)
//        {
//            try
//            {
//                await _doctorService.ReferPatientAsync(request);
//                return Ok(new { message = "Patient referred successfully." });
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { error = ex.Message });
//            }
//        }
//    }
//}
