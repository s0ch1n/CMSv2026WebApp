using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.Models
{
    public class Patient
    {
        //public int PatientId { get; set; }

        //[Required, MaxLength(100)]
        //public string PatientName { get; set; }

        //[Required]
        //public DateTime DateOfBirth { get; set; }

        //[Required, MaxLength(10)]
        //public string Gender { get; set; }

        //[Required, MaxLength(15)]
        //public string MobileNumber { get; set; }

        //[Required, MaxLength(255)]
        //public string Address { get; set; }

        //[Required, MaxLength(20)]
        //public string RegistrationId { get; set; }

        //public bool IsActive { get; set; } = true;
        public int PatientId { get; set; }

        [Required, MaxLength(100)]
        public string PatientName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required, MaxLength(10)]
        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other.")]
        public string Gender { get; set; }

        [Required, MaxLength(15)]
        [Phone]
        public string MobileNumber { get; set; }

        [Required, MaxLength(255)]
        public string Address { get; set; }

        [Required, MaxLength(20)]
        public string RegistrationId { get; set; }

        [Required, MaxLength(10)]
        [RegularExpression("^(A\\+|A-|B\\+|B-|O\\+|O-|AB\\+|AB-)$", ErrorMessage = "Invalid blood group.")]
        public string BloodGroup { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        public bool IsActive { get; set; } = true;s
    }
}
