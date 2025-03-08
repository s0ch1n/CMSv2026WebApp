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
            //return Content("Doctor Dashboard)
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
            var patient = _patientService.GetPatientsById(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            var viewModel = new PatientViewModel
            {
                Patient = patient,
                Medicines = _doctorService.GetAllMedicines(),
                LabTests = _doctorService.GetAllLabTests()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SubmitConsultation(int patientId, string symptoms, string diagnosis, string notes)
        {
            var consultation = new Consultation
            {
                AppointmentId = patientId, // Assuming AppointmentId is the same as patientId for simplicity
                Symptoms = symptoms,
                Diagnosis = diagnosis,
                Notes = notes
            };
            _patientService.AddConsultations(consultation);
            return RedirectToAction("PatientDetails", new { patientId });
        }

        [HttpPost]
        public IActionResult SubmitPrescriptions(int patientId, List<MedicinePrescription> prescriptions)
        {
            foreach (var prescription in prescriptions)
            {
                prescription.AppointmentId = patientId; // Assuming AppointmentId is the same as patientId for simplicity
                _patientService.AddPrescriptions(prescription);
            }
            return RedirectToAction("PatientDetails", new { patientId });
        }

        [HttpPost]
        public IActionResult SubmitLabTests(int patientId, List<LabTestPrescription> labTests)
        {
            foreach (var labTest in labTests)
            {
                labTest.AppointmentId = patientId; // Assuming AppointmentId is the same as patientId for simplicity
                _patientService.AddLabTests(labTest);
            }
            return RedirectToAction("PatientDetails", new { patientId });
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

