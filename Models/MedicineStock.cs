using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.Models
{
    public class MedicineStock
    {
        [Key]
        public int MedicineStockId { get; set; }

        [Required]
        public int StockInHand { get; set; }

        [Required]
        public int ReOrderLevel { get; set; }

        [Required]
        public int Purchase { get; set; }

        [Required]
        public int Issuance { get; set; }

        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
