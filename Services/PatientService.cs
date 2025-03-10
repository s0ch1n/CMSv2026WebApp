using CMSv2026WebApp.Models;
using CMSv2026WebApp.Repositories;

namespace CMSv2026WebApp.Services
{
    public class PatientService : IPatientService
    {
        //field
        
        private readonly IPatientRepository _patientRepository;

        //DI
        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public void AddConsultations(Consultation consultation)
        {
            _patientRepository.AddConsultation(consultation);
        }

        public void AddLabTests(LabTestPrescription labTest)
        {
            _patientRepository.AddLabTest(labTest);
        }

        public void AddPrescriptions(MedicinePrescription prescription)
        {
            _patientRepository.AddPrescription(prescription);
        }

        public List<Patient> GetAllthePatients()
        {
            return _patientRepository.GetAllPatients();
        }

        public void UpdatePatient(Patient patient)
        {
            _patientRepository.UpdatePatient(patient);
        }

        public void DeactivatePatient(int patientId)
        {
            _patientRepository.DeactivatePatient(patientId);
        }

        public IEnumerable<Patient> SearchPatients(string searchTerm)
        {
            return _patientRepository.SearchPatients(searchTerm);
        }

        public Patient GetPatientById(int patientId)
        {
            return _patientRepository.GetPatientById(patientId);
        }

        void IPatientService.AddPatient(Patient patient)
        {
           _patientRepository.AddPatient(patient);
        }

        
    }
}
