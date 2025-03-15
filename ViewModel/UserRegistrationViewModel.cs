using CMSv2026WebApp.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.ViewModel
{
    public class UserRegistrationViewModel
    {
        // Staff Details
        public Staff Staff { get; set; }

        // Doctor Details (if applicable)
        //public Doctor Doctor { get; set; }

        // List of all staff members
        public List<Staff> Staffs { get; set; } = new List<Staff>();

        // Available roles for selection
        public List<Role> Roles { get; set; } = new List<Role>();

        // Available specializations (if applicable)
        public List<Specialization> Specializations { get; set; } = new List<Specialization>();

        // UI Messages for feedback
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
