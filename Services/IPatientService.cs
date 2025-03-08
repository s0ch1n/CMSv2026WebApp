using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.Services
{
    public interface IPatientService
    {
        List<Patient> GetAllthePatients();
        Patient GetPatientsById(int patientId);
        void AddConsultations(Consultation consultation);
        void AddPrescriptions(MedicinePrescription prescription);
        void AddLabTests(LabTestPrescription labTest);

    }
}
