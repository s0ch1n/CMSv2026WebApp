using CMSv2026WebApp.Models;
using CMSv2026WebApp.Repositories;

namespace CMSv2026WebApp.Services
{
    public interface ILabTestService
    {
        //List<LabTestPrescription> GetPrescribedLabTests();
        //void GenerateLabTestReport(int labTestPrescriptionId);
        //LabTestPrescription GetPrescriptionById(int id);
        //List<LabTestPrescriptionViewModel> GetPrescribedLabTestss();
        //void AddTestValue(int labTestPrescriptionId, string testValue, string remarks);
        void AddLabTest(LabTest labTest);
        LabTestPrescription GetLabTestPrescriptionById(int labTestPrescriptionId);
        void UpdateLabTestPrescription(LabTestPrescription labTestPrescription);
        List<LabTestPrescription> GetLabTestPrescriptions();
        // List<LabTestPrescription> GetLabTestPrescriptions(int staffId);
        List<LabTestCategory> GetLabTestCategories();
        List<LabTest> GetAllLabTests();

    }
}

