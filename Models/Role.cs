using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required, MaxLength(50)]
        public string RoleName { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
