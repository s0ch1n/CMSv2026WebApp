using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.ViewModel
{
    public class ReceptionistViewModel
    {
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public List<Patient> Patients { get; set; } = new List<Patient>();
        public List<Medicine> Medicines { get; set; } = new List<Medicine>();
    }
}
