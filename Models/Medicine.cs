using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.Models
{
    public class Medicine
    {
        [Key]
        public int MedicineId { get; set; }

        [Required, MaxLength(100)]
        public string MedicineName { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required, MaxLength(50)]
        public string Unit { get; set; }

        [ForeignKey("MedicineType")]
        public int MedicineTypeId { get; set; }
        public MedicineType MedicineType { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
