using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.Models
{
    public class MedicinePrescription
    {
        [Key]
        public int MedicinePrescriptionId { get; set; }

        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        [Required, MaxLength(50)]
        public string Dosage { get; set; }

        [Required, MaxLength(50)]
        public string Frequency { get; set; }

        [Required, MaxLength(50)]
        public string Duration { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
    }
}
