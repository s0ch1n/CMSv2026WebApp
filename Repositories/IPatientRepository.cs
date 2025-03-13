using CMSv2026WebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMSv2026WebApp.Repositories
{
    public interface IPatientRepository
    {
        List<Patient> GetAllPatients();
        Patient GetPatientById(int patientId);
        void AddConsultation(Consultation consultation);
        void AddPrescription(MedicinePrescription prescription);
        void AddLabTest(LabTestPrescription labTest);

        // New methods based on ReceptionistController

        int AddPatient(Patient patient);
        void UpdatePatient(Patient patient);
        void DeactivatePatient(int patientId);
        IEnumerable<Patient> SearchPatients(string searchTerm);
        string GenerateRegistrationId();

    }
}
