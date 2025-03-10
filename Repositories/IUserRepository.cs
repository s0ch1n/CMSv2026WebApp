using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.Repositories
{
    public interface IUserRepository
    {
        //Credentials
        Staff AuthenticateUser(string userName, string password);

        //Register User ----> Create User by Admin
        void AddStaff(Staff staff);
        //void UpdateStaff(Staff staff);
        void DeleteStaff(int staffId);
        List<Staff> GetAllUsers();
        List<Role> GetAllRoles();
        //void UpdateUserStatus(int userId, bool isActive);
        Staff UpdateStaff(Staff staff);
        Staff GetStaffById(int staffId);
    }
}
