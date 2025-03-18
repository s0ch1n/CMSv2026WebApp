using CMSv2026WebApp.Models;
using CMSv2026WebApp.Service;
using CMSv2026WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMSv2026WebApp.Controllers
{
    public class LabTestController : Controller
    {

        private readonly ILabTestService _labTestService;
        private readonly PdfService _pdfService;

        public LabTestController(ILabTestService labTestService, PdfService pdfService)
        {
            _labTestService = labTestService;
            _pdfService = pdfService;
        }

        public IActionResult Index()
        {
            var pendingPrescriptions = _labTestService.GetAllLabTestPrescriptions();
            return View(pendingPrescriptions);
        }

        // Display pending prescriptions
        public IActionResult PendingPrescriptions()
        {
            var pendingPrescriptions = _labTestService.GetAllLabTestPrescriptions();
            return View(pendingPrescriptions);
        }

        // GET: Generate Report
        public IActionResult GenerateReport(int id)
        {
            var prescription = _labTestService.GetLabTestPrescriptionById(id);
            if (prescription == null)
            {
                return NotFound();
            }

            var labTechnicianName = User.Identity?.Name ?? "Unknown Lab Technician";

            var report = new LabTestReport
            {
                LabTestPrescriptionId = prescription.LabTestPrescriptionId,
                TestName = prescription.LabTest?.TestName ?? "Unknown Test", // Handle null LabTest
                PatientName = prescription.Appointment?.Patient?.PatientName ?? "Unknown Patient", // Handle null nested objects
                PrescribedByDoctor = prescription.Appointment?.Doctor?.Staff?.FullName ?? "Unknown Doctor", // Handle null nested objects
                GeneratedByLabTechnician = labTechnicianName, // Use logged-in lab technician's name
                ReportDate = DateTime.Now // Set the report date to the current date and time
            };

            return View(report);
        }
        // POST: Generate Report
        [HttpPost]
        public IActionResult GenerateReport(LabTestReport labTestReport)
        {
            if (ModelState.IsValid)
            {
                // Add the lab test report
                _labTestService.AddLabTestReport(labTestReport);

                // Mark the prescription as completed
                var prescription = _labTestService.GetLabTestPrescriptionById(labTestReport.LabTestPrescriptionId);
                //if (prescription != null)
                //{
                //    prescription.IsCompleted = true;
                //    _labTestService.UpdateLabTestPrescription(prescription);
                //}
                //else
                //{
                //    // Handle the case where the prescription is not found
                //    ModelState.AddModelError("", "Prescription not found.");
                //    return View(labTestReport);
                //}

                // Generate the PDF
                var pdfBytes = _pdfService.GenerateLabTestReportPdf(labTestReport);

                // Return the PDF as a file download
                return File(pdfBytes, "application/pdf", $"LabTestReport_{labTestReport.LabTestPrescriptionId}.pdf");
            }

            // If ModelState is invalid, return to the form with validation errors
            return View(labTestReport);
        }
    }
}
