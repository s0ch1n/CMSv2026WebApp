//using CMSv2026WebApp.Models;
//using Microsoft.Data.SqlClient;
//using System.Data;

//namespace CMSv2026WebApp.Repositories
//{
//    public class DoctorRepository : IDoctorRepository
//    {
//        private readonly string _connectionString;

//        public DoctorRepository(IConfiguration configuration)
//        {
//            _connectionString = configuration.GetConnectionString("DefaultConnection");
//        }

//        public async Task<IEnumerable<Appointment>> GetTodaysAppointmentsAsync(int doctorId)
//        {
//            var appointments = new List<Appointment>();

//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                string query = @"
//                SELECT * FROM Appointments 
//                WHERE DoctorId = @DoctorId AND AppointmentDate = CAST(GETDATE() AS DATE)";

//                using (SqlCommand cmd = new SqlCommand(query, conn))
//                {
//                    cmd.Parameters.AddWithValue("@DoctorId", doctorId);
//                    await conn.OpenAsync();

//                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
//                    {
//                        while (await reader.ReadAsync()) // ✅ Read Async
//                        {
//                            appointments.Add(new Appointment
//                            {
//                                AppointmentId = reader.GetInt32("AppointmentId"),
//                                PatientId = reader.GetInt32("PatientId"),
//                                DoctorId = reader.GetInt32("DoctorId"),
//                                AppointmentDate = reader.GetDateTime("AppointmentDate"),
//                                Status = reader.GetString("Status")
//                            });
//                        }
//                    }
//                }
//            }
//            return appointments;
//        }

//        public async Task<IEnumerable<Patient>> SearchDoctorPatientsAsync(int doctorId, string searchQuery)
//        {
//            var patients = new List<Patient>();

//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                string query = @"
//                SELECT DISTINCT P.* FROM Patients P
//                JOIN Appointments A ON P.PatientId = A.PatientId
//                WHERE A.DoctorId = @DoctorId 
//                AND (P.Name LIKE @SearchQuery OR P.Phone LIKE @SearchQuery)";

//                using (SqlCommand cmd = new SqlCommand(query, conn))
//                {
//                    cmd.Parameters.AddWithValue("@DoctorId", doctorId);
//                    cmd.Parameters.AddWithValue("@SearchQuery", $"%{searchQuery}%");

//                    await conn.OpenAsync();
//                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
//                    {
//                        while (await reader.ReadAsync())
//                        {
//                            patients.Add(new Patient
//                            {
//                                PatientId = reader.GetInt32("PatientId"),
//                                Name = reader.GetString("Name"),
//                                Phone = reader.GetString("Phone"),
//                                Address = reader.GetString("Address")
//                            });
//                        }
//                    }
//                }
//            }
//            return patients;
//        }

//        public async Task AddConsultationAsync(Consultation consultation)
//        {
//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                string query = @"
//                INSERT INTO Consultations (AppointmentId, DoctorId, PatientId, Diagnosis, Notes, ConsultationDate) 
//                VALUES (@AppointmentId, @DoctorId, @PatientId, @Diagnosis, @Notes, GETDATE())"; // ✅ Fixed column name

//                using (SqlCommand cmd = new SqlCommand(query, conn))
//                {
//                    cmd.Parameters.AddWithValue("@AppointmentId", consultation.AppointmentId);
//                    cmd.Parameters.AddWithValue("@DoctorId", consultation.DoctorId);
//                    cmd.Parameters.AddWithValue("@PatientId", consultation.PatientId);
//                    cmd.Parameters.AddWithValue("@Diagnosis", consultation.Diagnosis);
//                    cmd.Parameters.AddWithValue("@Notes", consultation.Notes ?? (object)DBNull.Value);

//                    await conn.OpenAsync();
//                    await cmd.ExecuteNonQueryAsync();
//                }
//            }
//        }

//        public async Task AddPrescriptionAsync(MedicinePrescription prescription)
//        {
//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                string query = @"
//                INSERT INTO Prescriptions (ConsultationId, MedicineId, Dosage, Frequency, Duration, Instructions) 
//                VALUES (@ConsultationId, @MedicineId, @Dosage, @Frequency, @Duration, @Instructions)";

//                using (SqlCommand cmd = new SqlCommand(query, conn))
//                {
//                    cmd.Parameters.AddWithValue("@ConsultationId", prescription.ConsultationId);
//                    cmd.Parameters.AddWithValue("@MedicineId", prescription.MedicineId);
//                    cmd.Parameters.AddWithValue("@Dosage", prescription.Dosage);
//                    cmd.Parameters.AddWithValue("@Frequency", prescription.Frequency);
//                    cmd.Parameters.AddWithValue("@Duration", prescription.Duration);
//                    cmd.Parameters.AddWithValue("@Instructions", prescription.Instructions ?? (object)DBNull.Value);

//                    await conn.OpenAsync();
//                    await cmd.ExecuteNonQueryAsync();
//                }
//            }
//        }

//        public async Task<IEnumerable<Consultation>> GetPatientConsultationHistoryAsync(int patientId)
//        {
//            var consultations = new List<Consultation>();

//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                string query = @"
//                SELECT * FROM Consultations WHERE PatientId = @PatientId ORDER BY ConsultationDate DESC";

//                using (SqlCommand cmd = new SqlCommand(query, conn))
//                {
//                    cmd.Parameters.AddWithValue("@PatientId", patientId);

//                    await conn.OpenAsync();
//                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
//                    {
//                        while (await reader.ReadAsync())
//                        {
//                            consultations.Add(new Consultation
//                            {
//                                ConsultationId = reader.GetInt32("ConsultationID"),
//                                AppointmentId = reader.GetInt32("AppointmentID"),
//                                DoctorId = reader.GetInt32("DoctorID"),
//                                PatientId = reader.GetInt32("PatientID"),
//                                Diagnosis = reader.GetString("Diagnosis"),
//                                Notes = reader["Notes"] as string, // Handle null values
//                                ConsultationDate = reader.GetDateTime("ConsultationDate")
//                            });
//                        }
//                    }
//                }
//            }
//            return consultations;
//        }

//        public async Task RequestLabTestAsync(LabTest labTest)
//        {
//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                string query = @"
//                INSERT INTO LabTests (PatientID, DoctorID, TestName, TestCost, TestDate, TestStatus) 
//                VALUES (@PatientID, @DoctorID, @TestName, @TestCost, GETDATE(), 'Pending')";

//                using (SqlCommand cmd = new SqlCommand(query, conn))
//                {
//                    cmd.Parameters.AddWithValue("@PatientID", labTest.PatientId);
//                    cmd.Parameters.AddWithValue("@DoctorID", labTest.DoctorId ?? (object)DBNull.Value);
//                    cmd.Parameters.AddWithValue("@TestName", labTest.TestName);
//                    cmd.Parameters.AddWithValue("@TestCost", labTest.TestCost);

//                    await conn.OpenAsync();
//                    await cmd.ExecuteNonQueryAsync();
//                }
//            }
//        }

//        public async Task<IEnumerable<LabTestResult>> GetPatientLabResultsAsync(int patientId)
//        {
//            var labTests = new List<LabTest>();

//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                string query = @"
//                SELECT * FROM LabTests WHERE PatientID = @PatientID AND TestStatus = 'Completed'";

//                using (SqlCommand cmd = new SqlCommand(query, conn))
//                {
//                    cmd.Parameters.AddWithValue("@PatientID", patientId);

//                    await conn.OpenAsync();
//                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
//                    {
//                        while (await reader.ReadAsync())
//                        {
//                            labTests.Add(new LabTest
//                            {
//                                LabTestId = reader.GetInt32("LabTestID"),
//                                PatientId = reader.GetInt32("PatientID"),
//                                DoctorId = reader["DoctorID"] as int?,
//                                TestName = reader.GetString("TestName"),
//                                TestCost = reader.GetDecimal("TestCost"),
//                                TestStatus = reader.GetString("TestStatus"),
//                                TestDate = reader.GetDateTime("TestDate"),
//                                TestResult = reader["TestResult"] as string
//                            });
//                        }
//                    }
//                }
//            }
//            return labTests;
//        }

//        public async Task ReferPatientAsync(Referral referral)
//        {
//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                string query = @"
//        INSERT INTO Referrals (AppointmentID, ReferringDoctorID, ReferredDoctorID, ReferralDate) 
//        VALUES (@AppointmentID, @ReferringDoctorID, @ReferredDoctorID, GETDATE())";

//                using (SqlCommand cmd = new SqlCommand(query, conn))
//                {
//                    cmd.Parameters.AddWithValue("@AppointmentID", referral.AppointmentId);
//                    cmd.Parameters.AddWithValue("@ReferringDoctorID", referral.ReferringDoctorId);
//                    cmd.Parameters.AddWithValue("@ReferredDoctorID", referral.ReferredDoctorId);

//                    await conn.OpenAsync();
//                    await cmd.ExecuteNonQueryAsync();
//                }
//            }
//        }

//    }
//}
