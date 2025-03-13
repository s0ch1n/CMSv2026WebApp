using CMSv2026WebApp.Models;
using CMSv2026WebApp.ViewModel;
using System.Collections.Generic;

namespace CMSv2026WebApp.Repositories
{
    public interface IAppointmentRepository
    {
        //List<Staff> GetAvailableDoctors();
        AppointmentViewModel BookAppointment(int patientId, int doctorId, DateTime appointmentDate, TimeSpan appointmentTime);
        ConsultationBill GenerateConsultationBill(int appointmentId);
        PaymentViewModel ConfirmPayment(int appointmentId);
        List<Patient> SearchPatients(string searchTerm, string searchBy);
        List<DoctorAvailability> GetAvailableDoctors(int specializationId, DateTime appointmentDate);
        List<Specialization> GetSpecializations();
        Doctor GetDoctorById(int doctorId);
        Doctor GetDoctorsById(int doctorId);
        bool HasExistingAppointment(int patientId, int doctorId, DateTime appointmentDate);
        List<TimeSpan> GetAvailableTimeSlots(int doctorId, DateTime appointmentDate);
        List<Appointment> GetTodaysAppointments();
        Appointment GetAppointmentByPatientAndDoctor(int patientId, int doctorId);
        Appointment GetAppointmentById(int appointmentId);
        void UpdateConsultationStatus(int appointmentId, string status);
    }

}
