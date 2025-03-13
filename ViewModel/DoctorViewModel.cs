using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.ViewModel
{
    public class DoctorViewModel
    {
        public Staff Staff { get; set; } = new Staff();
    public List<Appointment> Appointments { get; set; } = new List<Appointment>();
    public List<Patient> Patients { get; set; } = new List<Patient>();
    public List<Staff> Staffs { get; set; } = new List<Staff>(); 
    public List<Role> Roles { get; set; } = new List<Role>();

    // Additional Properties
    public int TotalAppointments => Appointments.Count;
    public int TotalPatients => Patients.Count;
    public List<Appointment> UpcomingAppointments => Appointments.Where(a => a.AppointmentDate > DateTime.Now).ToList();
    public List<Patient> RecentPatients => Patients.OrderByDescending(p => p.PatientId).Take(5).ToList();
    public Dictionary<string, int> AppointmentStatistics => Appointments
        .GroupBy(a => a.AppointmentDate.Date)
        .ToDictionary(g => g.Key.ToString("MM/dd/yyyy"), g => g.Count());

        // New property to filter today's appointments excluding those already consulted
        public List<Appointment> TodaysAppointments => Appointments
            .Where(a => a.AppointmentDate.Date == DateTime.Today && a.ConsultationStatus != "Consulted")
            .ToList();
    }
}
