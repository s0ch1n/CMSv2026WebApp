using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.Repositories
{
    public class LabTestPrescriptionRepository:ILabTestPrescriptionRepository
    {
        // Simulated dummy data for prescriptions
        private static List<LabTestPrescription> _dummyPrescriptions = new List<LabTestPrescription>
        {
            new LabTestPrescription
            {
                LabTestPrescriptionId = 1,
                LabTestId = 1,
                LabTestValue = "Normal",
                Remarks = "Test results are within normal limits.",
                CreatedDate = System.DateTime.Now.AddDays(-1),
                AppointmentId = 101
            },
            new LabTestPrescription
            {
                LabTestPrescriptionId = 2,
                LabTestId = 2,
                LabTestValue = "High",
                Remarks = "Test results indicate high levels. Further consultation needed.",
                CreatedDate = System.DateTime.Now.AddDays(-2),
                AppointmentId = 102
            }
        };

        // Fetch all prescriptions
        public List<LabTestPrescription> GetAllPrescriptions()
        {
            return _dummyPrescriptions;
        }

        // Fetch a specific prescription by its ID
        public LabTestPrescription GetPrescriptionById(int prescriptionId)
        {
            return _dummyPrescriptions.FirstOrDefault(p => p.LabTestPrescriptionId == prescriptionId);
        }
    }
}
    

