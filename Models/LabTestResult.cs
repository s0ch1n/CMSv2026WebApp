namespace CMSv2026WebApp.Models
{
    public class LabTestResult
    {
        public int ReportID { get; set; }  // Primary Key
        public int LabTestID { get; set; }  // Foreign Key referencing LabTests
        public string ReportFilePath { get; set; } = string.Empty; // Path to the report file
        public DateTime UploadDate { get; set; } = DateTime.Now; // Default to current date

        // Navigation Property (for Entity Framework)
        public LabTest? LabTest { get; set; }
    }
}
