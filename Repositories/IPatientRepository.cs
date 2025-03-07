namespace CMSv2026WebApp.Repositories
{
    public interface IPatientRepository
    {
        Task<int> AddPatient(Patient patient);
        Task UpdatePatient(Patient patient);
        Task DeactivatePatient(int patientId);
        Task<IEnumerable<Patient>> SearchPatients(string searchTerm);
        Task<Patient> GetPatientById(int patientId);
    }
}
