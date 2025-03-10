using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMSv2026WebApp.ViewModel
{
    public class AppointmentViewModel
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int TokenNumber { get; set; }
        public TimeSpan AppointmentTime { get; set; }

        public string ConsultationStatus { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public List<SelectListItem> PatientsList { get; set; }  // Dropdown
        public List<SelectListItem> Specializations { get; set; }  // Dropdown
        public List<SelectListItem> AvailableDoctors { get; set; }  // Dropdown

    }
}
