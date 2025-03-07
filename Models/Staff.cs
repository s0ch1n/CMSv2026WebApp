using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.Models
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required, MaxLength(10)]
        public string Gender { get; set; }

        [Required]
        public DateTime DateOfJoining { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required, MaxLength(15)]
        public string MobileNumber { get; set; }

        [Required, MaxLength(50)]
        public string UserName { get; set; }

        [Required, MaxLength(255)]
        public string Password { get; set; }

        [MaxLength(255)]
        public string Qualification { get; set; } 

        [Required, MaxLength(100), EmailAddress]
        public string EmailAddress { get; set; } 

        [ForeignKey("Role")]
        public int? RoleId { get; set; }
        public Role Role { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
