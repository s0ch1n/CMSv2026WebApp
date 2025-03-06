namespace CMSv2026WebApp.DTO
{
    public class LabTestRequest
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string TestName { get; set; }
        public decimal TestCost { get; set; }
    }
}
