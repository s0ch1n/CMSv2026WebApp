using CMSv2026WebApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CMSv2026WebApp.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly string _connectionString;

        public DoctorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStrMVC");
        }

        public List<Appointment> GetTodaysAppointments(int doctorId)
        {
            var appointments = new List<Appointment>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                SELECT * FROM Appointment 
                WHERE DoctorId = @DoctorId AND AppointmentDate = CAST(GETDATE() AS DATE)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            appointments.Add(new Appointment
                            {
                                AppointmentId = reader.GetInt32("AppointmentId"),
                                PatientId = reader.GetInt32("PatientId"),
                                DoctorId = reader.GetInt32("DoctorId"),
                                AppointmentDate = reader.GetDateTime("AppointmentDate"),
                                ConsultationStatus = reader.GetString("ConsultationStatus")
                            });
                        }
                    }
                }
            }
            return appointments;
        }

        public List<Patient> SearchDoctorPatients(int doctorId, string searchQuery)
        {
            var patients = new List<Patient>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                SELECT DISTINCT P.* FROM Patient P
                JOIN Appointment A ON P.PatientId = A.PatientId
                WHERE A.DoctorId = @DoctorId 
                AND (P.PatientName LIKE @SearchQuery OR P.MobileNumber LIKE @SearchQuery)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                    cmd.Parameters.AddWithValue("@SearchQuery", $"%{searchQuery}%");

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            patients.Add(new Patient
                            {
                                PatientId = reader.GetInt32("PatientId"),
                                PatientName = reader.GetString("PatientName"),
                                MobileNumber = reader.GetString("MobileNumber"),
                                Address = reader.GetString("Address")
                            });
                        }
                    }
                }
            }
            return patients;
        }

        public void AddConsultation(Consultation consultation)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                INSERT INTO Consultation (AppointmentId, Diagnosis, Notes, CreatedDate) 
                VALUES (@AppointmentId, @Diagnosis, @Notes, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentId", consultation.AppointmentId);
                    cmd.Parameters.AddWithValue("@Diagnosis", consultation.Diagnosis);
                    cmd.Parameters.AddWithValue("@Notes", consultation.Notes ?? (object)DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            // Update the consultation status to "Consulted"
            UpdateConsultationStatus(consultation.AppointmentId, "Consulted");
        }

        public void UpdateConsultationStatus(int appointmentId, string status)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                UPDATE Appointment 
                SET ConsultationStatus = @Status 
                WHERE AppointmentId = @AppointmentId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                    cmd.Parameters.AddWithValue("@Status", status);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Medicine> GetAllMedicines()
        {
            var medicines = new List<Medicine>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Medicine";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            medicines.Add(new Medicine
                            {
                                MedicineId = reader.GetInt32("MedicineId"),
                                MedicineName = reader.GetString("MedicineName"),
                                // Add other properties as needed
                            });
                        }
                    }
                }
            }

            return medicines;
        }

        public List<LabTest> GetAllLabTests()
        {
            var labTests = new List<LabTest>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM LabTest";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            labTests.Add(new LabTest
                            {
                                LabTestId = reader.GetInt32("LabTestId"),
                                TestName = reader.GetString("TestName"),
                                // Add other properties as needed
                            });
                        }
                    }
                }
            }

            return labTests;
        }

        public void AddPrescription(MedicinePrescription prescription)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                INSERT INTO Prescription (AppointmentId, MedicineId, Dosage, Frequency, Duration, CreatedDate) 
                VALUES (@AppointmentId, @MedicineId, @Dosage, @Frequency, @Duration, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentId", prescription.AppointmentId);
                    cmd.Parameters.AddWithValue("@MedicineId", prescription.MedicineId);
                    cmd.Parameters.AddWithValue("@Dosage", prescription.Dosage);
                    cmd.Parameters.AddWithValue("@Frequency", prescription.Frequency);
                    cmd.Parameters.AddWithValue("@Duration", prescription.Duration);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Consultation> GetPatientConsultationHistory(int patientId)
        {
            var consultations = new List<Consultation>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
        SELECT C.* FROM Consultation C
        JOIN Appointment A ON C.AppointmentId = A.AppointmentId
        WHERE A.PatientId = @PatientId
        ORDER BY C.CreatedDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PatientId", patientId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            consultations.Add(new Consultation
                            {
                                ConsultationId = reader.GetInt32("ConsultationId"),
                                AppointmentId = reader.GetInt32("AppointmentId"),
                                Diagnosis = reader.GetString("Diagnosis"),
                                Notes = reader["Notes"] as string,
                                CreatedDate = reader.GetDateTime("CreatedDate")
                            });
                        }
                    }
                }
            }
            return consultations;
        }

        public void RequestLabTest(LabTestPrescription labTest)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                INSERT INTO LabTestPrescription (LabTestId, LabTestValue, CreatedDate, Remarks, AppointmentId) 
                VALUES (@LabTestId, @LabTestValue, GETDATE(), @Remarks, @AppointmentId)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@LabTestId", labTest.LabTestId);
                    cmd.Parameters.AddWithValue("@LabTestValue", labTest.LabTestValue);
                    cmd.Parameters.AddWithValue("@Remarks", labTest.Remarks ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AppointmentId", labTest.AppointmentId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<LabTestResult> GetPatientLabResults(int appointmentId)
        {
            var labResults = new List<LabTestResult>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
        SELECT LTR.* FROM LabTestResult LTR
        JOIN LabTestPrescription LTP ON LTR.LabTestPrescriptionId = LTP.LabTestPrescriptionId
        WHERE LTP.AppointmentId = @AppointmentId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            labResults.Add(new LabTestResult
                            {
                                ReportID = reader.GetInt32("ReportID"),
                                LabTestID = reader.GetInt32("LabTestID"),
                                ReportFilePath = reader.GetString("ReportFilePath"),
                                UploadDate = reader.GetDateTime("UploadDate")
                            });
                        }
                    }
                }
            }
            return labResults;
        }



        public void ReferPatient(Referral referral)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                INSERT INTO Referrals (AppointmentID, ReferringDoctorID, ReferredDoctorID, ReferralDate) 
                VALUES (@AppointmentID, @ReferringDoctorID, @ReferredDoctorID, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentID", referral.AppointmentID);
                    cmd.Parameters.AddWithValue("@ReferringDoctorID", referral.ReferringDoctorID);
                    cmd.Parameters.AddWithValue("@ReferredDoctorID", referral.ReferredDoctorID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
