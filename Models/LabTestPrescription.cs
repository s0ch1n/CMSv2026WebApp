using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.Models
{
    public class LabTestPrescription
    {
        [Key]
        public int LabTestPrescriptionId { get; set; }

        [Required]
        public int LabTestId { get; set; }  // Foreign Key for LabTest

        [Required, MaxLength(100)]
        public string LabTestName { get; set; }

        [Required, MaxLength(100)]
        public string LabTestValue { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? Remarks { get; set; } // Nullable

        [Required]
        public int AppointmentId { get; set; }  // Foreign Key for Appointment

        public ICollection<LabTestResult> LabTestResults { get; set; }

        // Navigation Properties
        [ForeignKey("LabTestId")]
        public virtual LabTest? LabTest { get; set; }

        [ForeignKey("AppointmentId")]
        public virtual Appointment? Appointment { get; set; }
    }
}
