namespace CMSv2026WebApp.ViewModel
{
    public class PaymentViewModel
    {
        public int AppointmentId { get; set; }
        public int TokenNumber { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string PatientName { get; set; }
        public string MobileNumber { get; set; }
        public string RegistrationId { get; set; }
        public string BloodGroup { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string SpecializationName { get; set; }
        public decimal ConsultationFee { get; set; }
        public DateTime BillDate { get; set; }
        public string PaymentStatus { get; set; }
    }
}
