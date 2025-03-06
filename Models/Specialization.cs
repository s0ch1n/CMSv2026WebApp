using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.Models
{
    public class Specialization
    {
        [Key]
        public int SpecializationId { get; set; }

        [Required, MaxLength(100)]
        public string SpecializationName { get; set; }
    }
}

