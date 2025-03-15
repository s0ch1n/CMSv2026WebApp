using CMSv2026WebApp.Models;
using CMSv2026WebApp.ViewModel;
using System.Collections.Generic;

namespace CMSv2026WebApp.Services
{
    public interface IAppointmentService
    {
        AppointmentViewModel BookAppointment(int patientId, int doctorId, DateTime appointmentDate, int tokenNumber);
        ConsultationBill GenerateConsultationBill(int appointmentId);
        List<Patient> SearchPatients(string searchTerm, string searchBy);
        List<DoctorAvailability> GetAvailableDoctors(int specializationId, DateTime appointmentDate);
        List<Specialization> GetSpecializations();
        AppointmentViewModel BookAppointment(int patientId, int doctorId, DateTime appointmentDate, TimeSpan appointmentTime);
        List<TimeSpan> GetAvailableTimeSlots(int doctorId, DateTime appointmentDate);
        PaymentViewModel ConfirmPayment(int appointmentId);
        Doctor GetDoctorById(int doctorId);
        Doctor GetDoctorsById(int doctorId);
        bool HasExistingAppointment(int patientId, int doctorId, DateTime appointmentDate);
        //List<Appointment> GetTodaysAppointments();
        Appointment GetAppointmentByPatientAndDoctor(int patientId, int doctorId);
        Appointment GetAppointmentById(int appointmentId);
        void UpdateConsultationStatus(int appointmentId, string status);
        List<Appointment> GetTodaysAppointmentsForDoctor(int doctorId);

    }
}
