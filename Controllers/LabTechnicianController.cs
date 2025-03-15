using CMSv2026WebApp.Models;
using CMSv2026WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMSv2026WebApp.Controllers
{
    public class LabTechnicianController : Controller
    {

        private readonly ILabTestService _labTestService;
        private readonly IPatientService _patientService;

        public LabTechnicianController(ILabTestService labTestService, IPatientService patientService)
        {
            _labTestService = labTestService;
            _patientService = patientService;
        }

        // GET: LabTechnician/Index
        //public IActionResult Index()
        //{
        //    var labTests = _labTestService.GetAllPrescribedLabTests();
        //    return View(labTests);
        //}

        // GET: LabTechnician/PrescriptionDetails/{prescriptionId}
        public IActionResult PrescriptionDetails(int prescriptionId)
        {
            var prescription = _labTestService.GetLabTestPrescriptionById(prescriptionId);
            if (prescription == null)
            {
                return NotFound();
            }

            return View(prescription);
        }

        // GET: LabTechnician/GenerateReport/{prescriptionId}
        public IActionResult GenerateReport(int prescriptionId)
        {
            var prescription = _labTestService.GetLabTestPrescriptionById(prescriptionId);
            if (prescription == null)
            {
                return NotFound();
            }

            return View(prescription);
        }

        // POST: LabTechnician/GenerateReport
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult GenerateReport(LabTestResult labTestResult)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _labTestService.AddLabTestResult(labTestResult);
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(labTestResult);

        //}
    }
}