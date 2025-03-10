using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.Models
{
    public class Patient
    {

        public int PatientId { get; set; }

        [Required(ErrorMessage = "Patient name is required.")]
        [MaxLength(20, ErrorMessage = "Patient name cannot exceed 20 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Patient name must contain only alphabets and spaces.")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [MaxLength(10, ErrorMessage = "Gender cannot exceed 10 characters.")]
        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female, or Other.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [MaxLength(10, ErrorMessage = "Mobile number cannot exceed 10 characters.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be 10 digits.")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(50, ErrorMessage = "Address cannot exceed 50 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Registration ID is required.")]
        [MaxLength(10, ErrorMessage = "Registration ID cannot exceed 10 characters.")]
        // [RegularExpression(@"^(PL|PAT)\d{3}$", ErrorMessage = "Registration ID must be PL or PAT followed by three digits (e.g., PL001).")]

        public string RegistrationId { get; set; }

        [Required(ErrorMessage = "Blood group is required.")]
        [MaxLength(10, ErrorMessage = "Blood group cannot exceed 10 characters.")]
        [RegularExpression("^(A\\+|A-|B\\+|B-|O\\+|O-|AB\\+|AB-)$", ErrorMessage = "Invalid blood group.")]
        public string BloodGroup { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [MaxLength(40, ErrorMessage = "Email cannot exceed 40 characters.")]
        public string Email { get; set; }

        public bool IsActive { get; set; } = true;

    }
}
