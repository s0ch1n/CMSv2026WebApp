using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.ViewModel
{
    public class UserRegistrationViewModel
    {
        public Staff Staff { get; set; } = new Staff();
        public List<Staff> Staffs { get; set; } = new List<Staff>();
        public List<Role> Roles { get; set; } = new List<Role>();


    }
}
