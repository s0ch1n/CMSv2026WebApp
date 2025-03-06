using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Doctor>> GetAvailableDoctorsByDepartmentAsync(int departmentId);
        Task<bool> CheckDoctorAvailabilityAsync(int doctorId, DateTime selectedDate);
        Task<int> BookAppointmentAsync(int patientId, int doctorId, DateTime selectedDate, string timeSlot);
        Task<int> GenerateConsultationBillAsync(int patientId, int doctorId, DateTime appointmentDate, decimal fee);
    }
}
