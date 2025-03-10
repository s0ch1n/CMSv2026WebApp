using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.Models
{
    public class LabTest
    {
        public int LabTestId { get; set; }
        [Required]
        public string TestName { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public decimal? ReferenceMinRange { get; set; }
        public decimal? ReferenceMaxRange { get; set; }
        public bool SampleRequired { get; set; } = true;
        public bool IsActive { get; set; } = true;
    }
}
