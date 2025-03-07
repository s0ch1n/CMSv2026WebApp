namespace CMSv2026WebApp.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly string _connectionString;

        public PatientRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStrMVC");
        }
        public async Task<int> AddPatient(Patient patient)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("sp_AddPatient", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PatientName", patient.PatientName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", patient.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gender", patient.Gender);
                    cmd.Parameters.AddWithValue("@MobileNumber", patient.MobileNumber);
                    cmd.Parameters.AddWithValue("@Address", patient.Address);
                    cmd.Parameters.AddWithValue("@RegistrationId", patient.RegistrationId);
                    cmd.Parameters.AddWithValue("@BloodGroup", patient.BloodGroup);
                    cmd.Parameters.AddWithValue("@Email", patient.Email);

                    await conn.OpenAsync();
                    return Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }
            }
        }

        public async Task UpdatePatient(Patient patient)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("sp_EditPatient", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PatientId", patient.PatientId);
                    cmd.Parameters.AddWithValue("@PatientName", patient.PatientName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", patient.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gender", patient.Gender);
                    cmd.Parameters.AddWithValue("@MobileNumber", patient.MobileNumber);
                    cmd.Parameters.AddWithValue("@Address", patient.Address);
                    cmd.Parameters.AddWithValue("@RegistrationId", patient.RegistrationId);
                    cmd.Parameters.AddWithValue("@BloodGroup", patient.BloodGroup);
                    cmd.Parameters.AddWithValue("@Email", patient.Email);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeactivatePatient(int patientId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("sp_DeactivatePatient", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PatientId", patientId);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<IEnumerable<Patient>> SearchPatients(string searchTerm)
        {
            var patients = new List<Patient>();
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("sp_SearchPatient", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            patients.Add(new Patient
                            {
                                PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                                PatientName = reader.GetString(reader.GetOrdinal("PatientName")),
                                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                Gender = reader.GetString(reader.GetOrdinal("Gender")),
                                MobileNumber = reader.GetString(reader.GetOrdinal("MobileNumber")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                RegistrationId = reader.GetString(reader.GetOrdinal("RegistrationId")),
                                BloodGroup = reader.GetString(reader.GetOrdinal("BloodGroup")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            });
                        }
                    }
                }
            }
            return patients;
        }

        public async Task<Patient> GetPatientById(int patientId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("sp_GetPatientById", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PatientId", patientId);

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Patient
                            {
                                PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                                PatientName = reader.GetString(reader.GetOrdinal("PatientName")),
                                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                Gender = reader.GetString(reader.GetOrdinal("Gender")),
                                MobileNumber = reader.GetString(reader.GetOrdinal("MobileNumber")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                RegistrationId = reader.GetString(reader.GetOrdinal("RegistrationId")),
                                BloodGroup = reader.GetString(reader.GetOrdinal("BloodGroup")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}

    }
