using CMSv2026WebApp.Models;

public interface ILabTestRepository
{
    //List<LabTestPrescription> GetPrescribedLabTests();
    //List<LabTestPrescriptionViewModel> GetPrescribedLabTestss();
    //void GenerateLabTestReport(int labTestPrescriptionId);
    //LabTestPrescription GetPrescriptionById(int id);
    //void AddTestValue(int labTestPrescriptionId, string testValue, string remarks);
    void AddLabTest(LabTest labTest);
    LabTestPrescription GetLabTestPrescriptionById(int labTestPrescriptionId);
    void UpdateLabTestPrescription(LabTestPrescription labTestPrescription);
    //List<LabTestPrescription> GetLabTestPrescriptions(int StaffId);
    List<LabTestCategory> GetLabTestCategories();
    List<LabTest> GetAllLabTests();
    List<LabTestPrescription> GetLabTestPrescriptions();
}
