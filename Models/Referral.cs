namespace CMSv2026WebApp.Models
{
    public class Referral
    {
        public int ReferralID { get; set; }  // Primary Key
        public int AppointmentID { get; set; }  // Foreign Key referencing Appointments
        public int ReferringDoctorID { get; set; }  // Doctor who made the referral
        public int ReferredDoctorID { get; set; }  // Doctor to whom the patient is referred
        public DateTime ReferralDate { get; set; } = DateTime.Now;  // Default to current date

        // Navigation Properties (for Entity Framework)
        public Appointment? Appointment { get; set; }
        public Doctor? ReferringDoctor { get; set; }
        public Doctor? ReferredDoctor { get; set; }
    }
}
