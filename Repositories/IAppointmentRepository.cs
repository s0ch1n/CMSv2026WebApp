using CMSv2026WebApp.Models;
using Microsoft.Data.SqlClient;

namespace CMSv2026WebApp.Repositories
{
    public interface IAppointmentRepository
    {
        Task<Dictionary<int, string>> GetDoctorDepartmentsAsync();
        Task<List<Doctor>> GetAvailableDoctorsAsync(int departmentID, DateTime appointmentDate);
        Task<Dictionary<int, TimeSpan>> GetAvailableTimeSlotsAsync(int doctorId, DateTime appointmentDate, bool isMorning);
        Task<Doctor> GetDoctorByIDAsync(int doctorID);
        Task<int> GetDoctorIdFromUserAsync(int userId, SqlConnection conn, SqlTransaction transaction);
        Task<bool> ProcessPaymentAsync(int patientID, int doctorID, decimal consultationFee, DateTime appointmentDate);
        Task<int> GetNextTokenAsync(int doctorID, DateTime date, SqlConnection conn, SqlTransaction transaction);

    }
}
