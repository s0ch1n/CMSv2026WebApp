using Dapper;
using CMSv2026WebApp.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace CMSv2026WebApp.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly string _connectionString;

        public PatientRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStrMVC");
        }

        public List<Patient> GetAllPatients()
        {
            var query = "SELECT * FROM Patient";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<Patient>(query).ToList();
            }
        }


        public void AddConsultation(Consultation consultation)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                INSERT INTO Consultation (Symptoms, Diagnosis, Notes, CreatedDate, AppointmentId)
                VALUES (@Symptoms, @Diagnosis, @Notes, @CreatedDate, @AppointmentId)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentId", consultation.AppointmentId);
                    cmd.Parameters.AddWithValue("@Symptoms", consultation.Symptoms);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Diagnosis", consultation.Diagnosis);
                    cmd.Parameters.AddWithValue("@Notes", consultation.Notes ?? (object)DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddPrescription(MedicinePrescription prescription)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Fetch MedicineId from Medicine table using MedicineName
                string fetchMedicineQuery = "SELECT MedicineId FROM Medicine WHERE MedicineName = @MedicineName";
                int medicineId;

                using (SqlCommand fetchCmd = new SqlCommand(fetchMedicineQuery, conn))
                {
                    fetchCmd.Parameters.AddWithValue("@MedicineName", prescription.Medicine.MedicineName);
                    var result = fetchCmd.ExecuteScalar();
                    if (result == null)
                    {
                        throw new Exception($"Medicine '{prescription.Medicine.MedicineName}' not found in the database.");
                    }
                    medicineId = Convert.ToInt32(result);
                }

                // Insert into MedicinePrescription table
                string insertQuery = @"
                INSERT INTO MedicinePrescription (MedicineId, MedicineName, Dosage, Frequency, Duration, CreatedDate, AppointmentId)
                VALUES (@MedicineId, @MedicineName, @Dosage, @Frequency, @Duration, @CreatedDate, @AppointmentId)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentId", prescription.AppointmentId);
                    cmd.Parameters.AddWithValue("@MedicineName", prescription.Medicine.MedicineName);
                    cmd.Parameters.AddWithValue("@MedicineId", medicineId);
                    cmd.Parameters.AddWithValue("@Dosage", prescription.Dosage);
                    cmd.Parameters.AddWithValue("@Frequency", prescription.Frequency);
                    cmd.Parameters.AddWithValue("@Duration", prescription.Duration);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void AddLabTest(LabTestPrescription labTest)
        {
            var query = @"
        INSERT INTO LabTestPrescription (LabTestId, LabTestName, LabTestValue, CreatedDate, Remarks, AppointmentId)
        VALUES (@LabTestId, @LabTestName, @LabTestValue, @CreatedDate, @Remarks, @AppointmentId)";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(query, new
                {
                    labTest.LabTestId,
                    labTest.LabTestName,
                    labTest.LabTestValue,
                    labTest.CreatedDate,
                    labTest.Remarks,
                    labTest.AppointmentId
                });
            }
        }
        public string GenerateRegistrationId()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (var cmd = new SqlCommand("SELECT 'PAT' + RIGHT('0000' + CAST(ISNULL(MAX(CAST(SUBSTRING(RegistrationId, 4, LEN(RegistrationId)) AS INT)), 0) + 1 AS VARCHAR(3)), 3) FROM Patient", conn))
                    {
                        string registrationId = cmd.ExecuteScalar() as string;
                        return registrationId;
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log the exception
                throw;
            }
        }

        // New methods based on ReceptionistController
        public int AddPatient(Patient patient)
        {
            try
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

                        conn.Open();
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (SqlException ex)
            {
                // Check if the exception is due to a duplicate entry
                if (ex.Number == 50000) // Custom error number used in RAISERROR
                {
                    throw new ApplicationException("Mobile number, registration ID, or email already exists. Please enter unique details.");
                }
                throw; // Re-throw other SQL exceptions
            }
        }

        public void UpdatePatient(Patient patient)
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

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeactivatePatient(int patientId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("sp_DeactivatePatient", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PatientId", patientId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Patient> SearchPatients(string searchTerm)
        {
            var patients = new List<Patient>();
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("sp_SearchPatient", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            patients.Add(new Patient
                            {
                                PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                                PatientName = reader.IsDBNull(reader.GetOrdinal("PatientName")) ? null : reader.GetString(reader.GetOrdinal("PatientName")),
                                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender")),
                                MobileNumber = reader.IsDBNull(reader.GetOrdinal("MobileNumber")) ? null : reader.GetString(reader.GetOrdinal("MobileNumber")),
                                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                                RegistrationId = reader.IsDBNull(reader.GetOrdinal("RegistrationId")) ? null : reader.GetString(reader.GetOrdinal("RegistrationId")),
                                BloodGroup = reader.IsDBNull(reader.GetOrdinal("BloodGroup")) ? null : reader.GetString(reader.GetOrdinal("BloodGroup")),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            });
                        }
                    }
                }
            }
            return patients;
        }


        public Patient GetPatientById(int patientId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("sp_GetPatientById", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PatientId", patientId);

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
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

        public List<Patient> GetPatientsConsultedByDoctor(int doctorId)
        {
            var patients = new List<Patient>();

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT DISTINCT P.*
            FROM Patient P
            INNER JOIN Appointment A ON P.PatientId = A.PatientId
            WHERE A.DoctorId = @DoctorId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DoctorId", doctorId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
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

        public List<Patient> SearchConsultedPatientsByDoctor(int doctorId, string query)
        {
            var patients = new List<Patient>();

            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = @"
            SELECT DISTINCT P.*
            FROM Patient P
            INNER JOIN Appointment A ON P.PatientId = A.PatientId
            WHERE A.DoctorId = @DoctorId AND A.ConsultationStatus = 'Consulted'
            AND (P.PatientName LIKE @Query OR P.RegistrationId LIKE @Query)";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DoctorId", doctorId);
                    command.Parameters.AddWithValue("@Query", "%" + query + "%");

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
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
    }
}
