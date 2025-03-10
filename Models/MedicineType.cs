using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSv2026WebApp.Models
{
    public class MedicineType
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MedicineTypeId { get; set; }

        [Required, MaxLength(100)]
        public string MedicineTypeName { get; set; }
    }

 }

