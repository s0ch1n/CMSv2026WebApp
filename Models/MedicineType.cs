using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.Models
{
    public class MedicineType
    {
        [Key]
        public int MedicineTypeId { get; set; }

        [Required, MaxLength(100)]
        public string MedicineTypeName { get; set; }
    }
}
