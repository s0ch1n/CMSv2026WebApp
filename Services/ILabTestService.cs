using CMSv2026WebApp.Models;
using CMSv2026WebApp.Repositories;

namespace CMSv2026WebApp.Services
{
    public interface ILabTestService
    {
        // Lab Test Methods
        void AddLabTest(LabTest labTest);
        List<LabTest> GetAllLabTests();
        LabTest GetLabTestById(int labTestId);
        void UpdateLabTest(LabTest labTest);

        // Lab Test Prescription Methods
        void AddLabTestPrescription(LabTestPrescription labTestPrescription);
        List<LabTestPrescription> GetAllLabTestPrescriptions();
        //List<LabTestPrescription> GetPendingLabTestPrescriptions();
        LabTestPrescription GetLabTestPrescriptionById(int labTestPrescriptionId);
        void UpdateLabTestPrescription(LabTestPrescription labTestPrescription);

        // Lab Test Report Methods
        void AddLabTestReport(LabTestReport labTestReport);
        List<LabTestReport> GetAllLabTestReports();
    }
}

