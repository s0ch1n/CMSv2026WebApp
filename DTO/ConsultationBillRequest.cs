namespace CMSv2026WebApp.DTO
{
    public class ConsultationBillRequest
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal Amount { get; set; }
    }
}
