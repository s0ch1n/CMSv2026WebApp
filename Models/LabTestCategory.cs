using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.Models
{
    public class LabTestCategory
    {
        [Key]
        public int LabTestCategoryId { get; set; }
        [Required]
        [StringLength(100)]
        public string LabTestCategoryName { get; set; }

    }
}
