using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.Repositories
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Appointment>> GetTodaysAppointmentsAsync(int doctorId);
        Task<IEnumerable<Patient>> SearchDoctorPatientsAsync(int doctorId, string searchQuery);
        Task<IEnumerable<Consultation>> GetPatientConsultationHistoryAsync(int patientId);
        Task AddConsultationAsync(Consultation consultation);
        Task AddPrescriptionAsync(MedicinePrescription prescription);
        Task RequestLabTestAsync(LabTest labTest);
        Task ReferPatientAsync(Referral referral);
        
    }
}
