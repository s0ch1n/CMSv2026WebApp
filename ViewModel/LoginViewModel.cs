using System.ComponentModel.DataAnnotations;

namespace CMSv2026WebApp.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
