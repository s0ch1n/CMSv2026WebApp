using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.Models
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [MaxLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [MaxLength(10, ErrorMessage = "Gender cannot exceed 10 characters.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Date of Joining is required.")]
        public DateTime DateOfJoining { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Mobile Number is required.")]
        [MaxLength(15, ErrorMessage = "Mobile Number cannot exceed 15 characters.")]
        [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Invalid Mobile Number.")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(255, ErrorMessage = "Password cannot exceed 255 characters.")]
        public string Password { get; set; }

        [MaxLength(255, ErrorMessage = "Qualification cannot exceed 255 characters.")]
        public string Qualification { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address format.")]
        [MaxLength(100, ErrorMessage = "Email Address cannot exceed 100 characters.")]
        public string EmailAddress { get; set; }

        [ForeignKey("Role")]
        public int? RoleId { get; set; }
        public Role? Role { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property for Doctor details (only applicable if staff is a doctor)
        public Doctor? Doctor { get; set; }
    }
}
