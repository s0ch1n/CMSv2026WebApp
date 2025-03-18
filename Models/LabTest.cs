namespace CMSv2026WebApp.Models
{
    public class LabTest

    {
        public int LabTestId { get; set; }  // Primary Key
        public int PatientId { get; set; }  // Foreign Key referencing Patients
        public int? DoctorId { get; set; }  // Nullable Foreign Key for walk-in tests
        public string TestName { get; set; } = string.Empty;
        public decimal TestCost { get; set; }
        public DateTime TestDate { get; set; } = DateTime.Now;
        public string TestStatus { get; set; } = "Pending"; // Default status
        public string? TestResult { get; set; } // Nullable, as the result might not be available initially
        public decimal Amount { get; set; }
        public decimal? ReferenceMinRange { get; set; }
        public decimal? ReferenceMaxRange { get; set; }
        public bool SampleRequired { get; set; }


        // Navigation Properties (for Entity Framework)
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
