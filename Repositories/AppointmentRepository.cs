using Dapper;
using CMSv2026WebApp.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using CMSv2026WebApp.ViewModel;
using System.Data;

namespace CMSv2026WebApp.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly string _connectionString;

        public AppointmentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStrMVC");
        }


        public AppointmentViewModel BookAppointment(int patientId, int doctorId, DateTime appointmentDate, TimeSpan appointmentTime)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("sp_BookAppointment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Combine date and time into a single DateTime
                    DateTime combinedDateTime = appointmentDate.Date + appointmentTime;

                    // Add input parameters
                    cmd.Parameters.AddWithValue("@PatientId", patientId);
                    cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                    cmd.Parameters.AddWithValue("@AppointmentDate", combinedDateTime);
                    cmd.Parameters.AddWithValue("@AppointmentTime", appointmentTime);

                    // Add output parameter for TokenNumber
                    var tokenNumberParam = new SqlParameter("@TokenNumber", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(tokenNumberParam);

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        // Read first result set (TokenNumber)
                        if (reader.Read())
                        {
                            int tokenNumber = reader.GetInt32(reader.GetOrdinal("TokenNumber"));
                        }

                        // Move to the second result set
                        if (reader.NextResult() && reader.Read())
                        {
                            return new AppointmentViewModel
                            {
                                AppointmentId = reader.GetInt32(reader.GetOrdinal("AppointmentId")),
                                AppointmentDate = reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                                AppointmentTime = reader.GetTimeSpan(reader.GetOrdinal("AppointmentTime")),  // Ensure this is correctly retrieved
                                TokenNumber = reader.GetInt32(reader.GetOrdinal("TokenNumber")),
                                PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                                DoctorId = reader.GetInt32(reader.GetOrdinal("DoctorId")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                IsActive = true
                            };
                        }
                    }
                }
            }
            return null;
        }

        public PaymentViewModel ConfirmPayment(int appointmentId)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand("sp_ConfirmPayment", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new PaymentViewModel
                                {
                                    AppointmentId = reader.GetInt32(reader.GetOrdinal("AppointmentId")),
                                    TokenNumber = reader.GetInt32(reader.GetOrdinal("TokenNumber")),
                                    AppointmentDate = reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                                    PatientName = reader.GetString(reader.GetOrdinal("PatientName")),
                                    MobileNumber = reader.GetString(reader.GetOrdinal("MobileNumber")),
                                    RegistrationId = reader.GetString(reader.GetOrdinal("RegistrationId")),
                                    BloodGroup = reader.GetString(reader.GetOrdinal("BloodGroup")),
                                    DoctorId = reader.GetInt32(reader.GetOrdinal("DoctorId")),
                                    SpecializationName = reader.GetString(reader.GetOrdinal("SpecializationName")),
                                    ConsultationFee = reader.GetDecimal(reader.GetOrdinal("ConsultationFee")),
                                    BillDate = reader.GetDateTime(reader.GetOrdinal("BillDate")),
                                    PaymentStatus = reader.GetString(reader.GetOrdinal("PaymentStatus"))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in ConfirmPayment: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }

            return null;
        }

        public List<DoctorAvailability> GetAvailableDoctors(int specializationId, DateTime appointmentDate)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("sp_GetAvailableDoctors", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SpecializationId", specializationId);
                    cmd.Parameters.AddWithValue("@AppointmentDate", appointmentDate);

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        var doctors = new List<DoctorAvailability>();
                        while (reader.Read())
                        {
                            doctors.Add(new DoctorAvailability
                            {
                                DoctorId = reader.GetInt32(reader.GetOrdinal("DoctorId")),
                                Name = reader.GetString(reader.GetOrdinal("DoctorName")), // Ensure this matches the column name
                                ConsultationFee = reader.GetDecimal(reader.GetOrdinal("ConsultationFee"))
                            });
                        }
                        return doctors;
                    }
                }
            }
        }

        public List<Specialization> GetSpecializations()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("sp_GetSpecializations", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        var specializations = new List<Specialization>();
                        while (reader.Read())
                        {
                            specializations.Add(new Specialization
                            {
                                SpecializationId = reader.GetInt32(reader.GetOrdinal("SpecializationId")),
                                SpecializationName = reader.GetString(reader.GetOrdinal("SpecializationName"))
                            });
                        }
                        return specializations;
                    }
                }
            }
        }

        public ConsultationBill GenerateConsultationBill(int appointmentId)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand("sp_GenerateConsultationBill", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);

                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new ConsultationBill
                                {
                                    BillId = reader.GetInt32(reader.GetOrdinal("BillId")),
                                    AppointmentId = reader.GetInt32(reader.GetOrdinal("AppointmentId")),
                                    ConsultationFee = reader.GetDecimal(reader.GetOrdinal("ConsultationFee")),
                                    BillDate = reader.GetDateTime(reader.GetOrdinal("BillDate")),
                                    PaymentStatus = reader.GetString(reader.GetOrdinal("PaymentStatus"))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in GenerateConsultationBill: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }

            return null;
        }

        public Doctor GetDoctorById(int doctorId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("sp_GetDoctorById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DoctorId", doctorId);

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Doctor
                            {
                                DoctorId = reader.GetInt32(reader.GetOrdinal("DoctorId")),
                                Name = reader.GetString(reader.GetOrdinal("DoctorName")),
                                SpecializationId = reader.GetInt32(reader.GetOrdinal("SpecializationId")),
                                ConsultationFee = reader.GetDecimal(reader.GetOrdinal("ConsultationFee")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool HasExistingAppointment(int patientId, int doctorId, DateTime appointmentDate)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("sp_HasExistingAppointment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PatientId", patientId);
                    cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                    cmd.Parameters.AddWithValue("@AppointmentDate", appointmentDate.Date); // Use only the date part

                    conn.Open();
                    var result = cmd.ExecuteScalar();

                    // If the result is 1, an existing appointment exists
                    return result != null && (int)result == 1;
                }
            }
        }

        public List<Staff> GetAvailableDoctors()
        {
            throw new NotImplementedException();
        }

        public List<TimeSpan> GetAvailableTimeSlots(int doctorId, DateTime appointmentDate)
        {
            List<TimeSpan> availableSlots = new();

            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("sp_GetAvailableTimeSlots", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                    cmd.Parameters.AddWithValue("@AppointmentDate", appointmentDate.Date);

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Read the TimeSpan directly from the database
                            var timeSlot = reader.GetTimeSpan(0);
                            availableSlots.Add(timeSlot);
                        }
                    }
                }
            }

            return availableSlots;
        }

        public List<Patient> SearchPatients(string searchTerm, string searchBy)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("sp_SearchPatient1", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
                    cmd.Parameters.AddWithValue("@SearchBy", searchBy);

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        var patients = new List<Patient>();
                        while (reader.Read())
                        {
                            patients.Add(new Patient
                            {
                                PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                                PatientName = reader.IsDBNull(reader.GetOrdinal("PatientName")) ? null : reader.GetString(reader.GetOrdinal("PatientName")),
                                MobileNumber = reader.IsDBNull(reader.GetOrdinal("MobileNumber")) ? null : reader.GetString(reader.GetOrdinal("MobileNumber")),
                                RegistrationId = reader.IsDBNull(reader.GetOrdinal("RegistrationId")) ? null : reader.GetString(reader.GetOrdinal("RegistrationId")),
                                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender")),
                                BloodGroup = reader.IsDBNull(reader.GetOrdinal("BloodGroup")) ? null : reader.GetString(reader.GetOrdinal("BloodGroup")),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email"))
                            });
                        }
                        return patients;
                    }
                }
            }
        }


        public List<Appointment> GetTodaysAppointments()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("sp_GetTodaysAppointments", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Today", DateTime.Today);

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        var appointments = new List<Appointment>();
                        while (reader.Read())
                        {
                            var appointment = new Appointment
                            {
                                AppointmentId = reader.GetInt32(reader.GetOrdinal("AppointmentId")),
                                AppointmentDate = reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                                Patient = new Patient
                                {
                                    PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                                    PatientName = reader.IsDBNull(reader.GetOrdinal("PatientName")) ? null : reader.GetString(reader.GetOrdinal("PatientName")),
                                    MobileNumber = reader.IsDBNull(reader.GetOrdinal("MobileNumber")) ? null : reader.GetString(reader.GetOrdinal("MobileNumber")),
                                    RegistrationId = reader.IsDBNull(reader.GetOrdinal("RegistrationId")) ? null : reader.GetString(reader.GetOrdinal("RegistrationId")),
                                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                    Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender")),
                                    BloodGroup = reader.IsDBNull(reader.GetOrdinal("BloodGroup")) ? null : reader.GetString(reader.GetOrdinal("BloodGroup")),
                                    Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email"))
                                },
                                Doctor = new Doctor
                                {
                                    DoctorId = reader.GetInt32(reader.GetOrdinal("DoctorId")),
                                    Name = reader.IsDBNull(reader.GetOrdinal("DoctorName")) ? null : reader.GetString(reader.GetOrdinal("DoctorName")),
                                    SpecializationId = reader.GetInt32(reader.GetOrdinal("SpecializationId")),
                                    ConsultationFee = reader.GetDecimal(reader.GetOrdinal("ConsultationFee")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                },
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };

                            // Check for null value in AppointmentTime
                            if (!reader.IsDBNull(reader.GetOrdinal("AppointmentTime")))
                            {
                                appointment.AppointmentTime = reader.GetTimeSpan(reader.GetOrdinal("AppointmentTime"));
                            }

                            appointments.Add(appointment);
                        }
                        return appointments;
                    }
                }
            }
        }


    }
}
