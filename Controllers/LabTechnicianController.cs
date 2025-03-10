using CMSv2026WebApp.Models;
using CMSv2026WebApp.Services;
using Microsoft.AspNetCore.Mvc;



namespace CMSv2026WebApp.Controllers
{
    public class LabTechnicianController : Controller
    {
        private readonly ILabTestPrescriptionService _service;

        public LabTechnicianController(ILabTestPrescriptionService service)
        {
            _service = service;
        }

        // Action to display all prescriptions (from doctor) (HTTP GET)
        [HttpGet]
        public IActionResult ListPrescriptions()
        {
            var prescriptions = _service.GetAllPrescriptions();
            return View(prescriptions); // Ensure you're passing a valid collection to the view
        }

        // Action to generate and send a report to the doctor (HTTP GET)
        [HttpGet]
        public IActionResult GenerateReport(int prescriptionId)
        {
            var prescription = _service.GetPrescriptionById(prescriptionId);

            if (prescription != null)
            {
                var report = new LabTestReport
                {
                    PrescriptionId = prescription.LabTestPrescriptionId,
                    TestName = prescription.LabTestValue,
                    TestResult = "Normal",  // Dummy test result
                    Remarks = prescription.Remarks,
                    ReferenceRange = "10-50",  // Dummy reference range
                    Amount = 100.0m,  // Dummy amount
                    SampleRequired = GetSampleType(prescription.LabTestValue)
                };

                return View("ViewReport", report); // Pass single report to the view
            }

            return NotFound();
        }
        // Helper method to get the sample type based on the test name
        private string GetSampleType(string testName)
        {
            // Sample type logic based on test name
            switch (testName.ToLower())
            {
                case "blood test":
                    return "Blood";
                case "urine test":
                    return "Urine";
                case "x-ray":
                    return "No Sample";
                default:
                    return "Unknown Sample";
            }
        }
    }
}
