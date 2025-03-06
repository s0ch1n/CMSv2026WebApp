using CMSv2026WebApp.Models;
using Microsoft.Data.SqlClient;

namespace CMSv2026WebApp.Repositories
{
    public class UserRepository : IUserRepository
    {

        //Fields
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStrMVC");
        }

        public void AddStaff(Staff staff)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = @"INSERT INTO Staffs (FullName, Gender, DateofJoining, DateOfBirth, 
                    MobileNumber, UserName, Password, RoleId, IsActive)
                    VALUES (@FullName, @Gender, @DateofJoining, @DateOfBirth, 
                    @MobileNumber, @UserName, @Password, @RoleId, @IsActive)";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FullName", staff.FullName);
                    command.Parameters.AddWithValue("@Gender", staff.Gender);
                    command.Parameters.AddWithValue("@DateofJoining", staff.DateOfJoining);
                    command.Parameters.AddWithValue("@DateOfBirth", (object)staff.DateOfBirth ?? DBNull.Value);
                    command.Parameters.AddWithValue("@MobileNumber", staff.MobileNumber);
                    command.Parameters.AddWithValue("@UserName", staff.UserName);
                    command.Parameters.AddWithValue("@Password", staff.Password);  // Store hashed password in real-world applications
                    command.Parameters.AddWithValue("@RoleId", staff.RoleId);
                    command.Parameters.AddWithValue("@IsActive", staff.IsActive);

                    command.ExecuteNonQuery();
                }
            }
        }


        public User AuthenticateUser(string userName, string Password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = @"select u.UserId, u.Username, u.Password, u.RoleId,
                    u.IsActive, r.RoleName, r.IsActive from Users u
                    join Roles r on u.RoleId = r.RoleId
                    where u.Username = @Username and 
                    u.Password = @Password
                    and u.IsActive = 1";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", userName);
                    command.Parameters.AddWithValue("@Password", Password);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Password = reader.GetString(2),
                                RoleId = reader.GetInt32(3),
                                IsActive = reader.GetBoolean(4),
                                Role = new Role
                                {
                                    RoleId = reader.GetInt32(3),
                                    RoleName = reader.GetString(5),
                                    IsActive = reader.GetBoolean(6)
                                }
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<Role> GetAllRoles()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = @"select RoleId, RoleName, IsActive from Roles";
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var roles = new List<Role>();
                        while (reader.Read())
                        {
                            roles.Add(new Role
                            {
                                RoleId = reader.GetInt32(0),
                                RoleName = reader.GetString(1),
                                IsActive = reader.GetBoolean(2)
                            });
                        }
                        return roles;
                    }
                }
            }
        }

        public List<User> GetAllUsers()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = @"select u.UserId, u.Username, u.Password, u.RoleId,
                    u.IsActive, r.RoleName from Users u
                    join Roles r on u.RoleId = r.RoleId";
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var users = new List<User>();
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                UserId = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Password = reader.GetString(2),
                                RoleId = reader.GetInt32(3),
                                IsActive = reader.GetBoolean(4),
                                Role = new Role
                                {
                                    RoleId = reader.GetInt32(3),
                                    RoleName = reader.GetString(5)
                                }
                            });
                        }
                        return users;
                    }
                }
            }
        }

        public void UpdateUserStatus(int userId, bool isActive)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = @"update Users set IsActive = @IsActive where UserId = @UserId";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IsActive", isActive);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
