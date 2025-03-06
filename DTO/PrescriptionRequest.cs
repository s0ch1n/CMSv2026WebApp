namespace CMSv2026WebApp.DTO
{
    public class PrescriptionRequest
    {
        public int ConsultationId { get; set; }
        public int MedicineId { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }
        public string Instructions { get; set; }
    }
}
