namespace CMSv2026WebApp.DTO
{
    public class ConsultationRequest
    {
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string Diagnosis { get; set; }
        public string Notes { get; set; }
    }
}
