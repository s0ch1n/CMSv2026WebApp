//using CMSv2026WebApp.Models;
//using CMSv2026WebApp.Repositories;

//namespace CMSv2026WebApp.Services
//{
//    public class AppointmentService
//    {
//        private readonly IAppointmentRepository _appointmentRepository;

//        public AppointmentService(IAppointmentRepository appointmentRepository)
//        {
//            _appointmentRepository = appointmentRepository;
//        }

//        // Get list of available doctors in a department
//        public async Task<IEnumerable<Doctor>> GetAvailableDoctorsByDepartmentAsync(int departmentId)
//        {
//            return await _appointmentRepository.GetAvailableDoctorsByDepartmentAsync(departmentId);
//        }

//        // Check if a doctor has available slots
//        public async Task<bool> CheckDoctorAvailabilityAsync(int doctorId, DateTime selectedDate)
//        {
//            int bookedSlots = await _appointmentRepository.GetBookedSlotsForDoctorAsync(doctorId, selectedDate);
//            return bookedSlots < 30; // Return true if slots are available
//        }

//        // Book an appointment
//        public async Task<int> BookAppointmentAsync(int patientId, int doctorId, DateTime selectedDate, string timeSlot)
//        {
//            bool isAvailable = await CheckDoctorAvailabilityAsync(doctorId, selectedDate);
//            if (!isAvailable) throw new Exception("No available slots for this doctor on the selected date.");

//            var appointment = new Appointment
//            {
//                PatId = patientId,
//                DocId = doctorId,
//                AppoDate = selectedDate,
//                TimeSlot = timeSlot
//            };

//            return await _appointmentRepository.BookAppointmentAsync(appointment);
//        }

//        // Generate Consultation Bill after booking an appointment
//        public async Task<int> GenerateConsultationBillAsync(int patientId, int doctorId, DateTime appointmentDate, decimal fee)
//        {
//            var bill = new ConsultationBill
//            {
//                PatientID = patientId,
//                DoctorID = doctorId,
//                AppointmentDate = appointmentDate,
//                Amount = fee,
//                PaymentStatus = "Pending"
//            };

//            return await _appointmentRepository.GenerateConsultationBillAsync(bill);
//        }
//    }
//}
