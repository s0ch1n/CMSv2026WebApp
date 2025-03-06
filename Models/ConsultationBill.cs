namespace CMSv2026WebApp.Models
{
    public class ConsultationBill
    {
        public int PaymentID { get; set; }  // Primary Key
        public int PatientID { get; set; }  // Foreign Key referencing Patients
        public int DoctorID { get; set; }   // Foreign Key referencing Doctors
        public DateTime AppointmentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; } = "Pending"; // Default status
        public DateTime PaymentDate { get; set; }

        // Navigation Properties (if using EF Core)
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
