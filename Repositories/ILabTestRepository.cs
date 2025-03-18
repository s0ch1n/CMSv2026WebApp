using CMSv2026WebApp.Models;

public interface ILabTestRepository
{
    // Lab Test Methods
    void AddLabTest(LabTest labTest);
    List<LabTest> GetAllLabTests();
    LabTest GetLabTestById(int labTestId);
    void UpdateLabTest(LabTest labTest);

    // Lab Test Prescription Methods
    void AddLabTestPrescription(LabTestPrescription labTestPrescription);
    List<LabTestPrescription> GetAllLabTestPrescriptions();
    List<LabTestPrescription> GetPendingLabTestPrescriptions(); // Get prescriptions not yet completed
    LabTestPrescription GetLabTestPrescriptionById(int labTestPrescriptionId);
    void UpdateLabTestPrescription(LabTestPrescription labTestPrescription);

    // Lab Test Report Methods
    void AddLabTestReport(LabTestReport labTestReport);
    List<LabTestReport> GetAllLabTestReports();
}
