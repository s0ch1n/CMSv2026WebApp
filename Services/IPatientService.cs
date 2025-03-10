using CMSv2026WebApp.Models;
using System.Collections.Generic;

namespace CMSv2026WebApp.Services
{
    public interface IPatientService
    {
        void AddPatient(Patient patient);
        void DeactivatePatient(int patientId);
        Patient GetPatientById(int patientId);
        IEnumerable<Patient> SearchPatients(string searchTerm);
        void UpdatePatient(Patient patient);
        List<Patient> GetAllthePatients();
        void AddConsultations(Consultation consultation);
        void AddPrescriptions(MedicinePrescription prescription);
        void AddLabTests(LabTestPrescription labTest);

       

    }
}
