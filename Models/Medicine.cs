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

        [Required]
        public int Unit { get; set; }

        public int StockInHand { get; set; }

        [ForeignKey("MedicineTypeId")]
        public int MedicineTypeId { get; set; }
        public string MedicineTypeName { get; set; } // For display
        public MedicineType MedicineType { get; set; }
        public MedicineStock MedicineStock { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
