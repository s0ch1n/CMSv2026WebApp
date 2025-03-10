using CMSv2026WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMSv2026WebApp.Services
{
    public class LabTestPrescriptionService : ILabTestPrescriptionService
    {
        private List<LabTestPrescription> _prescriptions = new List<LabTestPrescription>();

        public LabTestPrescriptionService()
        {
            // Dummy data to simulate prescriptions from the doctor
            _prescriptions = new List<LabTestPrescription>
        {
            new LabTestPrescription
            {
                LabTestPrescriptionId = 1,
                LabTestValue = "Blood Test",
                Remarks = "Patient showing signs of infection.",
                CreatedDate = DateTime.Now.AddDays(-1),
                LabTestId = 101
            },
            new LabTestPrescription
            {
                LabTestPrescriptionId = 2,
                LabTestValue = "Urine Test",
                Remarks = "Routine checkup.",
                CreatedDate = DateTime.Now.AddDays(-2),
                LabTestId = 102
            }
        };
        }

        public IEnumerable<LabTestPrescription> GetAllPrescriptions()
        {
            return _prescriptions;
        }

        public LabTestPrescription GetPrescriptionById(int prescriptionId)
        {
            return _prescriptions.FirstOrDefault(p => p.LabTestPrescriptionId == prescriptionId);
        }
    }
}