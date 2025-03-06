using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [Required, MaxLength(100)]
        public string PatientName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required, MaxLength(10)]
        public string Gender { get; set; }

        [Required, MaxLength(15)]
        public string MobileNumber { get; set; }

        [Required, MaxLength(255)]
        public string Address { get; set; }

        [Required, MaxLength(20)]
        public string RegistrationId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
