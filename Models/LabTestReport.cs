namespace CMSv2026WebApp.Models
{
    public class LabTestReport
    {
        public int LabTestReportId { get; set; }
        public int LabTestPrescriptionId { get; set; }
        public string PatientName { get; set; }
        public string TestName { get; set; } // Maps to VARCHAR(100)
        public string PrescribedByDoctor { get; set; } // Maps to VARCHAR(100)
        public string GeneratedByLabTechnician { get; set; } // Maps to VARCHAR(100)
        public DateTime ReportDate { get; set; } = DateTime.Now;
        public string Remarks { get; set; } // Ensure this is not nullable (string, not string?)

    }
}
