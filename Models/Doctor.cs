using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required]
        public decimal ConsultationFee { get; set; }

        [ForeignKey("Specialization")]
        public int SpecializationId { get; set; }
        public Specialization Specialization { get; set; }

        [ForeignKey("Staff")]
        public int StaffId { get; set; }
        public Staff Staff { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
