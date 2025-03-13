using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.ViewModel
{
    public class PatientViewModel
    {
        public Patient Patient { get; set; }
        public List<Medicine> Medicines { get; set; }
        public List<LabTest> LabTests { get; set; }
        public Appointment Appointment { get; set; }

    }
}
