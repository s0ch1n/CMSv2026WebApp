using CMSv2026WebApp.Models;
using CMSv2026WebApp.Repositories;

namespace CMSv2026WebApp.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<IEnumerable<Appointment>> GetTodaysAppointmentsAsync(int doctorId)
        {
            return await _doctorRepository.GetTodaysAppointmentsAsync(doctorId);
        }

        public async Task<IEnumerable<Patient>> SearchDoctorPatientsAsync(int doctorId, string searchQuery)
        {
            return await _doctorRepository.SearchDoctorPatientsAsync(doctorId, searchQuery);
        }

        public async Task<IEnumerable<Consultation>> GetPatientConsultationHistoryAsync(int patientId)
        {
            return await _doctorRepository.GetPatientConsultationHistoryAsync(patientId);
        }

        public async Task AddConsultationAsync(Consultation consultation)
        {
            await _doctorRepository.AddConsultationAsync(consultation);
        }

        public async Task AddPrescriptionAsync(MedicinePrescription prescription)
        {
            await _doctorRepository.AddPrescriptionAsync(prescription);
        }
    }
}
