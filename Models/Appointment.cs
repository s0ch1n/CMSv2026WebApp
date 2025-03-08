using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSv2026WebApp.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public int TokenNumber { get; set; }

        [Required, MaxLength(50)]
        public string ConsultationStatus { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        // Additional properties for the view
        public ICollection<LabTestPrescription> LabTestPrescriptions { get; set; }
        [NotMapped]
        public string PatientName => Patient?.PatientName;
        [NotMapped]
        public string Gender => Patient?.Gender;
        [NotMapped]
        public int Age => Patient != null ? DateTime.Now.Year - Patient.DateOfBirth.Year : 0;
    }
}
