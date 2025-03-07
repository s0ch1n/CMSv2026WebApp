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
                var sql = @"INSERT INTO Staffs (FullName, Gender, DateOfJoining, DateOfBirth, MobileNumber, UserName, 
                        Password, Qualification, EmailAddress, RoleId, IsActive) 
                        VALUES (@FullName, @Gender, @DateOfJoining, @DateOfBirth, @MobileNumber, @UserName, 
                        @Password, @Qualification, @EmailAddress, @RoleId, @IsActive)";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FullName", staff.FullName);
                    command.Parameters.AddWithValue("@Gender", staff.Gender);
                    command.Parameters.AddWithValue("@DateOfJoining", staff.DateOfJoining);
                    command.Parameters.AddWithValue("@DateOfBirth", (object?)staff.DateOfBirth ?? DBNull.Value);
                    command.Parameters.AddWithValue("@MobileNumber", staff.MobileNumber);
                    command.Parameters.AddWithValue("@UserName", staff.UserName);
                    command.Parameters.AddWithValue("@Password", staff.Password);
                    command.Parameters.AddWithValue("@Qualification", (object?)staff.Qualification ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EmailAddress", staff.EmailAddress);
                    command.Parameters.AddWithValue("@RoleId", staff.RoleId);
                    command.Parameters.AddWithValue("@IsActive", staff.IsActive);

                    command.ExecuteNonQuery();
                }
            }
        }


        public Staff AuthenticateUser(string userName, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = @"SELECT s.StaffId, s.UserName, s.Password, s.RoleId, s.IsActive, 
                    r.RoleName, r.IsActive 
                   FROM Staffs s
                   JOIN Roles r ON s.RoleId = r.RoleId
                   WHERE s.UserName = @UserName 
                   AND s.Password = @Password 
                   AND s.IsActive = 1";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Parameters.AddWithValue("@Password", password);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Staff
                            {
                                StaffId = reader.GetInt32(0),
                                UserName = reader.GetString(1),
                                Password = reader.GetString(2),
                                RoleId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
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

        public void DeleteStaff(int staffId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "DELETE FROM Staffs WHERE StaffId=@StaffId";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@StaffId", staffId);
                    command.ExecuteNonQuery();
                }
            }
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

        public List<Staff> GetAllUsers()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = @"SELECT s.StaffId, s.FullName, s.Gender, s.DateOfJoining, s.DateOfBirth,
                   s.MobileNumber, s.UserName, s.Password, s.Qualification, s.EmailAddress,
                   s.RoleId, s.IsActive, r.RoleName FROM Staffs s
                   JOIN Roles r ON s.RoleId = r.RoleId";
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var staffs = new List<Staff>();
                        while (reader.Read())
                        {
                            staffs.Add(new Staff
                            {
                                StaffId = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Gender = reader.GetString(2),
                                DateOfJoining = reader.GetDateTime(3),
                                DateOfBirth = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                                MobileNumber = reader.GetString(5),
                                UserName = reader.GetString(6),
                                Password = reader.GetString(7), 
                                Qualification = reader.IsDBNull(8) ? null : reader.GetString(8),
                                EmailAddress = reader.GetString(9),
                                RoleId = reader.GetInt32(10),
                                IsActive = reader.GetBoolean(11),
                                Role = new Role
                                {
                                    RoleId = reader.GetInt32(10),
                                    RoleName = reader.GetString(12)
                                }
                            });
                        }
                        return staffs;
                    }
                }
            }
        }

        public void UpdateStaff(Staff staff)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = @"UPDATE Staffs SET FullName=@FullName, Gender=@Gender, DateOfJoining=@DateOfJoining, 
                        DateOfBirth=@DateOfBirth, MobileNumber=@MobileNumber, UserName=@UserName, 
                        Password=@Password, Qualification=@Qualification, EmailAddress=@EmailAddress, 
                        RoleId=@RoleId, IsActive=@IsActive WHERE StaffId=@StaffId";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@StaffId", staff.StaffId);
                    command.Parameters.AddWithValue("@FullName", staff.FullName);
                    command.Parameters.AddWithValue("@Gender", staff.Gender);
                    command.Parameters.AddWithValue("@DateOfJoining", staff.DateOfJoining);
                    command.Parameters.AddWithValue("@DateOfBirth", (object?)staff.DateOfBirth ?? DBNull.Value);
                    command.Parameters.AddWithValue("@MobileNumber", staff.MobileNumber);
                    command.Parameters.AddWithValue("@UserName", staff.UserName);
                    command.Parameters.AddWithValue("@Password", staff.Password);
                    command.Parameters.AddWithValue("@Qualification", (object?)staff.Qualification ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EmailAddress", staff.EmailAddress);
                    command.Parameters.AddWithValue("@RoleId", staff.RoleId);
                    command.Parameters.AddWithValue("@IsActive", staff.IsActive);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateUserStatus(int staffId, bool isActive)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = @"update Staffs set IsActive = @IsActive where StaffId = @StaffId";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IsActive", isActive);
                    command.Parameters.AddWithValue("@StaffId", staffId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
