using CMSv2026WebApp.Models;
using CMSv2026WebApp.Repositories;

namespace CMSv2026WebApp.Services
{
    public class UserService : IUserService
    {
        //field
        private readonly IUserRepository _userRepository;
        private readonly IDoctorRepository _doctorRepository;


        //DI
        public UserService(IUserRepository userRepository, IDoctorRepository doctorRepository)
        {
            _userRepository = userRepository;
            _doctorRepository = doctorRepository;
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

        public Doctor GetDoctorByStaffId(int staffId)
        {
            return _doctorRepository.GetDoctorByStaffId(staffId);
        }

        public List<Specialization> GetAllSpecializations()
        {
            return _userRepository.GetAllSpecializations();
        }

        public Staff GetStaffById(int staffId)
        {
            return _userRepository.GetStaffById(staffId);

        }

        public int GetStaffIdByRoleId(int roleId)
        {
            return _userRepository.GetStaffIdByRoleId(roleId);
        }

        public Staff GetStaffByRoleId(int roleId)
        {
            return _userRepository.GetStaffByRoleId(roleId);
        }

        public void InsertStaff(Staff staff)
        {
            _userRepository.AddStaff(staff);
        }


        public void RemoveStaff(int staffId)
        {
            _userRepository.DeleteStaff(staffId);
        }

        public Staff UpdateStaff(Staff staff)
        {
            return _userRepository.UpdateStaff(staff);

        }

        public void UpdateStaffStatus(int userId, bool isActive)
        {
            _userRepository.UpdateUserStatus(userId, isActive);
        }
    }
 }
