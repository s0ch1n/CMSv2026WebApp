using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.Models
{
    public class Patient
    {
       
        [Key]
        public int PatientId { get; set; }

        [Required, MaxLength(100)]
        public string PatientName { get; set; } = string.Empty;

        [Required, MaxLength(10)]
        public string Gender { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required, MaxLength(15)]
        public string MobileNumber { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string Address { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string RegistrationId { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Email { get; set; }  // Nullable as per DB schema

        [Required, MaxLength(5)]
        public string BloodGroup { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
