namespace CMSv2026WebApp.DTO
{
    public class ReferralRequest
    {
        public int AppointmentId { get; set; }
        public int ReferringDoctorId { get; set; }
        public int ReferredDoctorId { get; set; }
    }
}
