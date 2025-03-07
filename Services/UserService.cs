using CMSv2026WebApp.Models;
using CMSv2026WebApp.Repositories;

namespace CMSv2026WebApp.Services
{
    public class UserService : IUserService
    {
        //field
        private readonly IUserRepository _userRepository;


        //DI
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Staff AuthenticateTheUser(string userName, string password)
        {
           return _userRepository.AuthenticateUser(userName, password);
        }

        public void EditStaff(Staff staff)
        {
           _userRepository.UpdateStaff(staff);
        }

        public List<Role> GetAllStaffRoles()
        {
            return _userRepository.GetAllRoles();
        }

        public List<Staff> GetAllStaffs()
        {
            return _userRepository.GetAllUsers();
        }

        public void InsertStaff(Staff staff)
        {
            _userRepository.AddStaff(staff);
        }

        public void RemoveStaff(int staffId)
        {
            _userRepository.DeleteStaff(staffId);
        }

        public void UpdateStaffStatus(int userId, bool isActive)
        {
            _userRepository.UpdateUserStatus(userId, isActive);
        }
    }
 }
