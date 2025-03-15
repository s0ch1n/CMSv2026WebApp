using CMSv2026WebApp.Models;
using CMSv2026WebApp.Repositories;
using CMSv2026WebApp.ViewModel;

namespace CMSv2026WebApp.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public AppointmentViewModel BookAppointment(int patientId, int doctorId, DateTime appointmentDate, int tokenNumber)
        {
            if (_appointmentRepository.HasExistingAppointment(patientId, doctorId, appointmentDate))
            {
                throw new InvalidOperationException("The patient already has an appointment with this doctor on the given date.");
            }

            var appointment = new Appointment
            {
                PatientId = patientId,
                DoctorId = doctorId,
                AppointmentDate = appointmentDate,
                TokenNumber = tokenNumber,
                ConsultationStatus = "Scheduled"
            };

            return _appointmentRepository.BookAppointment(patientId, doctorId, appointmentDate, appointment.AppointmentTime);
        }

        public ConsultationBill GenerateConsultationBill(int appointmentId)
        {
            return _appointmentRepository.GenerateConsultationBill(appointmentId);
        }

        public List<Patient> SearchPatients(string searchTerm, string searchBy)
        {
            return _appointmentRepository.SearchPatients(searchTerm, searchBy);
        }

        public List<DoctorAvailability> GetAvailableDoctors(int specializationId, DateTime appointmentDate)
        {
            return _appointmentRepository.GetAvailableDoctors(specializationId, appointmentDate);
        }

        public List<Specialization> GetSpecializations()
        {
            return _appointmentRepository.GetSpecializations();
        }

        public AppointmentViewModel BookAppointment(int patientId, int doctorId, DateTime appointmentDate, TimeSpan appointmentTime)
        {
            if (_appointmentRepository.HasExistingAppointment(patientId, doctorId, appointmentDate))
            {
                throw new InvalidOperationException("The patient already has an appointment with this doctor on the given date.");
            }

            var appointment = new Appointment
            {
                PatientId = patientId,
                DoctorId = doctorId,
                AppointmentDate = appointmentDate,
                AppointmentTime = appointmentTime,
                ConsultationStatus = "Scheduled"
            };

            return _appointmentRepository.BookAppointment(patientId, doctorId, appointmentDate, appointmentTime);
        }

        public List<TimeSpan> GetAvailableTimeSlots(int doctorId, DateTime appointmentDate)
        {
            return _appointmentRepository.GetAvailableTimeSlots(doctorId, appointmentDate);
        }

        public PaymentViewModel ConfirmPayment(int appointmentId)
        {
            return _appointmentRepository.ConfirmPayment(appointmentId);
        }

        public Doctor GetDoctorById(int doctorId)
        {
            return _appointmentRepository.GetDoctorById(doctorId);
        }
        public Doctor GetDoctorsById(int doctorId)
        {
            return _appointmentRepository.GetDoctorsById(doctorId);
        }

        public Appointment GetAppointmentByPatientAndDoctor(int patientId, int doctorId)
        {
            return _appointmentRepository.GetAppointmentByPatientAndDoctor(patientId, doctorId);
        }

        public Appointment GetAppointmentById(int appointmentId)
        {
            return _appointmentRepository.GetAppointmentById(appointmentId);
        }

        public void UpdateConsultationStatus(int appointmentId, string status)
        {
            _appointmentRepository.UpdateConsultationStatus(appointmentId, status);
        }

        public bool HasExistingAppointment(int patientId, int doctorId, DateTime appointmentDate)
        {
            return _appointmentRepository.HasExistingAppointment(patientId, doctorId, appointmentDate);
        }

        //public List<Appointment> GetTodaysAppointments()
        //{
        //    return _appointmentRepository.GetTodaysAppointments();
        //}

        public List<Appointment> GetTodaysAppointmentsForDoctor(int doctorId)
        {
            return _appointmentRepository.GetTodaysAppointmentsForDoctor(doctorId);
          
        }

    }
}
