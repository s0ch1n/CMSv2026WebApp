using Dapper;
using CMSv2026WebApp.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

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

        //public Patient GetPatientById(int patientId)
        //{
        //    var query = "SELECT * FROM Patients WHERE PatientId = @PatientId";

        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();
        //        return connection.QuerySingleOrDefault<Patient>(query, new { PatientId = patientId });
        //    }
        //}

        public void AddConsultation(Consultation consultation)
        {
            var query = @"
                INSERT INTO Consultations (Symptoms, Diagnosis, Notes, CreatedDate, AppointmentId, IsActive)
                VALUES (@Symptoms, @Diagnosis, @Notes, @CreatedDate, @AppointmentId, @IsActive)";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(query, consultation);
            }
        }

        public void AddPrescription(MedicinePrescription prescription)
        {
            var query = @"
                INSERT INTO MedicinePrescriptions (MedicineId, Dosage, Frequency, Duration, CreatedDate, AppointmentId)
                VALUES (@MedicineId, @Dosage, @Frequency, @Duration, @CreatedDate, @AppointmentId)";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(query, prescription);
            }
        }

        public void AddLabTest(LabTestPrescription labTest)
        {
            var query = @"
                INSERT INTO LabTestPrescriptions (LabTestId, LabTestValue, CreatedDate, Remarks, AppointmentId)
                VALUES (@LabTestId, @LabTestValue, @CreatedDate, @Remarks, @AppointmentId)";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(query, labTest);
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

    }
}
