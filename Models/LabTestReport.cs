namespace CMSv2026WebApp.Models
{

    public class LabTestReport
    {
        public int PrescriptionId { get; set; }
        public string TestName { get; set; }
        public string TestResult { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ReferenceRange { get; set; }
        public string SampleRequired { get; set; }
        public decimal Amount { get; set; }
    }
}
