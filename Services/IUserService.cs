using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.Services
{
    public interface IUserService
    {
        //Credentials
        Staff AuthenticateTheUser(string userName, string password);
        //Register User ----> Create User by Admin
        void InsertStaff(Staff staff);
        void EditStaff(Staff staff);
        void RemoveStaff(int staffId);
        List<Staff> GetAllStaffs();
        List<Role> GetAllStaffRoles();
        void UpdateStaffStatus(int userId, bool isActive);
        Staff GetStaffById(int staffId);
        Staff UpdateStaff(Staff staff);
        Doctor GetDoctorByStaffId(int staffId);
        int GetStaffIdByRoleId(int roleId);
        Staff GetStaffByRoleId(int roleId);
        List<Specialization> GetAllSpecializations();
    }
}
