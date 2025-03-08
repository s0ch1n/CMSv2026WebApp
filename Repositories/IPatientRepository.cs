using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.Repositories
{
    public interface IPatientRepository
    {
        public List<Patient> GetAllPatients();

        //Patient GetPatientByID(int patient)
        Patient GetPatientById(int patientId);
        void AddConsultation(Consultation consultation);
        void AddPrescription(MedicinePrescription prescription);
        void AddLabTest(LabTestPrescription labTest);

    }
}
