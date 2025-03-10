using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CMSv2026WebApp.Models
{
    public class Staff
    {
       
        [Key]
        public int StaffId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }


        [Required]
        public DateTime DateOfJoining { get; set; }

        [Required]
        [Phone]
        public string MobileNumber { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [ForeignKey("Role")]
        public int? RoleId { get; set; }
        
        public Role? Role { get; set; }


        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        public string Qualification { get; set; }

        public bool IsActive { get; set; }
    }

    }

