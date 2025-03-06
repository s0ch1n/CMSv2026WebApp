using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.Repositories
{
    public interface IUserRepository
    {
        //Credentials
        User AuthenticateUser(string userName, string password);

        //Register User ----> Create User by Admin
        void AddUser(User user);
        List<User> GetAllUsers();
        List<Role> GetAllRoles();
        void UpdateUserStatus(int userId, bool isActive);
    }
}
