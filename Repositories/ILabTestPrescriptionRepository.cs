using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.Repositories
{
    public interface ILabTestPrescriptionRepository
    {
        //fetching prescription
        List<LabTestPrescription> GetAllPrescriptions(); 
        LabTestPrescription GetPrescriptionById(int prescriptionId);  
    }
}
