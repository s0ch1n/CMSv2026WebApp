namespace CMSv2026WebApp.Models
{
    public class DoctorAvailability
    {
        
        public int DoctorId { get; set; }
        public string Name { get; set; } // Doctor's name
        public string SpecializationName { get; set; }
        public decimal ConsultationFee { get; set; }
        public int CurrentPatientCount { get; set; }
        

    }
}
