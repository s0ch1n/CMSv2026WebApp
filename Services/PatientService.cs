namespace CMSv2026WebApp.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<int> AddPatient(Patient patient)
        {
            return await _patientRepository.AddPatient(patient);
        }

        public async Task UpdatePatient(Patient patient)
        {
            await _patientRepository.UpdatePatient(patient);
        }

        public async Task DeactivatePatient(int patientId)
        {
            await _patientRepository.DeactivatePatient(patientId);
        }

        public async Task<IEnumerable<Patient>> SearchPatients(string searchTerm)
        {
            return await _patientRepository.SearchPatients(searchTerm);
        }

        public async Task<Patient> GetPatientById(int patientId)
        {
            return await _patientRepository.GetPatientById(patientId);
        }
    }
}
