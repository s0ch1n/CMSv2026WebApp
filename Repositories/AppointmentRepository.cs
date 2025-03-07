//using CMSv2026WebApp.Models;
//using Microsoft.Data.SqlClient;


//namespace CMSv2026WebApp.Repositories
//{
//    public class AppointmentRepository : IAppointmentRepository
//    {
//        private readonly string _connectionString;

//        public AppointmentRepository(IConfiguration configuration)
//        {
//            _connectionString = configuration.GetConnectionString("DefaultConnection");
//        }

//        // Get doctor departments
//        public async Task<Dictionary<int, string>> GetDoctorDepartmentsAsync()
//        {
//            var departments = new Dictionary<int, string>();

//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                await conn.OpenAsync();
//                string query = "SELECT DeptId, DeptName FROM Departments";

//                using (SqlCommand cmd = new SqlCommand(query, conn))
//                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
//                {
//                    while (await reader.ReadAsync())
//                    {
//                        departments.Add(reader.GetInt32(0), reader.GetString(1));
//                    }
//                }
//            }
//            return departments;
//        }

//        // Get available doctors in a department
//        public async Task<List<Doctor>> GetAvailableDoctorsAsync(int departmentID, DateTime appointmentDate)
//        {
//            var doctors = new List<Doctor>();

//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                await conn.OpenAsync();
//                string query = @"
//                SELECT d.DoctorID, d.DName, d.DeptId, 
//                       (SELECT COUNT(*) FROM Appointments a 
//                        WHERE a.DocId = d.DoctorID 
//                        AND a.AppoDate = @AppointmentDate) AS PatientCount
//                FROM Doctors d 
//                WHERE d.DeptId = @DepartmentID AND d.IsAvailabile = 1";

//                using (SqlCommand cmd = new SqlCommand(query, conn))
//                {
//                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
//                    cmd.Parameters.AddWithValue("@AppointmentDate", appointmentDate.Date);

//                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
//                    {
//                        while (await reader.ReadAsync())
//                        {
//                            int patientCount = reader.GetInt32(3);
//                            if (patientCount < 30) // Enforce 30 appointments limit
//                            {
//                                doctors.Add(new Doctor
//                                {
//                                    DoctorID = reader.GetInt32(0),
//                                    Name = reader.GetString(1),
//                                    DepartmentID = reader.GetInt32(2)
//                                });
//                            }
//                        }
//                    }
//                }
//            }

//            return doctors;
//        }

//        // Get available time slots
//        public async Task<Dictionary<int, TimeSpan>> GetAvailableTimeSlotsAsync(int doctorId, DateTime appointmentDate, bool isMorning)
//        {
//            Console.WriteLine("Checking slots for Doctor ID: " + doctorId);

//            // Define Morning & Evening Slots
//            TimeSpan[] morningSlots = {
//        new TimeSpan(9, 0, 0), new TimeSpan(9, 15, 0), new TimeSpan(9, 30, 0),
//        new TimeSpan(9, 45, 0), new TimeSpan(10, 0, 0), new TimeSpan(10, 15, 0),
//        new TimeSpan(10, 30, 0), new TimeSpan(10, 45, 0), new TimeSpan(11, 00, 0),
//        new TimeSpan(11, 15, 0), new TimeSpan(11, 30, 0), new TimeSpan(11, 45, 0),
//        new TimeSpan(12, 0, 0), new TimeSpan(12, 15, 0), new TimeSpan(12, 30, 0),
//        new TimeSpan(12, 45, 0), new TimeSpan(13, 0, 0)
//                                       };

//            TimeSpan[] eveningSlots = {
//        new TimeSpan(14, 0, 0), new TimeSpan(14, 15, 0), new TimeSpan(14, 30, 0),
//        new TimeSpan(14, 45, 0), new TimeSpan(15, 0, 0), new TimeSpan(15, 15, 0),
//        new TimeSpan(15, 30, 0), new TimeSpan(15, 45, 0), new TimeSpan(16, 00, 0),
//        new TimeSpan(16, 15, 0), new TimeSpan(16, 30, 0), new TimeSpan(16, 45, 0),
//        new TimeSpan(17, 0, 0)
//                                     };

//            TimeSpan[] selectedSlots = isMorning ? morningSlots : eveningSlots;
//            HashSet<TimeSpan> bookedSlots = new HashSet<TimeSpan>();
//            var availableSlots = new Dictionary<int, TimeSpan>();

//            // Database Query to Fetch Already Booked Slots
//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                await conn.OpenAsync();
//                string query = "SELECT TimeSlot FROM Appointments WHERE DocId = @DoctorID AND AppoDate = @Date";

//                using (SqlCommand cmd = new SqlCommand(query, conn))
//                {
//                    cmd.Parameters.AddWithValue("@DoctorID", doctorId);
//                    cmd.Parameters.AddWithValue("@Date", appointmentDate.Date);

//                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
//                    {
//                        while (await reader.ReadAsync())
//                        {
//                            string timeString = reader.GetString(0);
//                            if (TimeSpan.TryParse(timeString, out TimeSpan bookedTime))
//                            {
//                                bookedSlots.Add(bookedTime);
//                            }
//                            else
//                            {
//                                Console.WriteLine($"Invalid time format in database: {timeString}");
//                            }
//                        }
//                    }
//                }
//            }

//            // Filter available slots based on the current time if it's today
//            TimeSpan currentTime = DateTime.Now.TimeOfDay;
//            int index = 1;

//            foreach (var slot in selectedSlots)
//            {
//                if (!bookedSlots.Contains(slot) && (appointmentDate.Date > DateTime.Now.Date || slot > currentTime))
//                {
//                    availableSlots.Add(index++, slot);
//                }
//            }

//            return availableSlots;
//        }


//        public async Task<Doctor> GetDoctorByIDAsync(int doctorID)
//        {
//            string query = @"
//        SELECT DoctorId, DName, DeptId, ConsultationFee 
//        FROM Doctors 
//        WHERE DoctorId = @DoctorID";

//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                await conn.OpenAsync();
//                using (SqlCommand cmd = new SqlCommand(query, conn))
//                {
//                    cmd.Parameters.AddWithValue("@DoctorID", doctorID);

//                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
//                    {
//                        if (await reader.ReadAsync())
//                        {

//                            return new Doctor
//                            {
//                                DoctorId = reader.GetInt32(0),
//                                Name = reader.GetString(1),
//                                DepartmentId = reader.GetInt32(2),
//                                ConsultationFee = Convert.ToDecimal(reader[3])
//                            };
//                        }
//                    }
//                }
//            }
//            return null; // Return null if no doctor is found
//        }

//        public async Task<int> BookAppointmentAsync(Appointment appointment, bool isMorningShift)
//        {
//            try
//            {
//                using (SqlConnection conn = new SqlConnection(_connectionString))
//                {
//                    await conn.OpenAsync();
//                    using (SqlTransaction transaction = conn.BeginTransaction())
//                    {
//                        try
//                        {
//                            var availableSlots = await GetAvailableTimeSlotsAsync(appointment.DoctorID, appointment.AppoDate, isMorningShift);

//                            if (availableSlots == null || !availableSlots.Any())
//                            {
//                                Console.WriteLine("No available slots.");
//                                return -1;
//                            }

//                            //int slotIndex = ConsoleHelper.SelectOption("Available Time Slots:", availableSlots.Values.Select(ts => ts.ToString(@"hh\:mm")).ToList());
//                            var slotList = availableSlots.Values.Select(ts => ts.ToString(@"hh\:mm")).ToList();

//                            Console.ReadKey();
//                            Console.Clear();
//                            int slotIndex = ConsoleHelper.SelectOption("Available Time Slots", slotList);
//                            if (slotIndex == -1) return -1; // User didn't select a valid option

//                            TimeSpan selectedTimeSlot = availableSlots.ElementAt(slotIndex).Value;

//                            int tokenNumber = await GetNextTokenAsync(appointment.DoctorID, appointment.AppoDate, conn, transaction);

//                            string query = @"
//                            INSERT INTO Appointments (PatId, DocId, AppoDate, TokenId, TimeSlot) 
//                            VALUES (@PatientID, @DoctorID, @AppointmentDate, @TokenNumber, @TimeSlot)";

//                            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
//                            {
//                                cmd.Parameters.AddWithValue("@PatientID", appointment.PatientID);
//                                cmd.Parameters.AddWithValue("@DoctorID", appointment.DoctorID);
//                                cmd.Parameters.AddWithValue("@AppointmentDate", appointment.AppoDate.Date);
//                                cmd.Parameters.AddWithValue("@TokenNumber", tokenNumber);
//                                cmd.Parameters.AddWithValue("@TimeSlot", selectedTimeSlot.ToString(@"hh\:mm"));

//                                await cmd.ExecuteNonQueryAsync();
//                            }

//                            await transaction.CommitAsync();
//                            return tokenNumber;
//                        }
//                        catch (Exception ex)
//                        {
//                            await transaction.RollbackAsync();
//                            Console.WriteLine($"Error booking appointment: {ex.Message}");
//                            return -1;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Database connection error: {ex.Message}");
//                return -1;
//            }
//        }




//        private async Task<int> GetDoctorIdFromUserAsync(int userId, SqlConnection conn, SqlTransaction transaction)
//        {
//            string query = "SELECT DoctorId FROM Doctors WHERE UsId = @UserID";

//            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
//            {
//                cmd.Parameters.AddWithValue("@UserID", userId);
//                object result = await cmd.ExecuteScalarAsync();

//                return (result != null && int.TryParse(result.ToString(), out int doctorId)) ? doctorId : -1;
//            }
//        }



//        public async Task<bool> ProcessPaymentAsync(int patientID, int doctorID, decimal consultationFee, DateTime appointmentDate)
//        {
//            string query = @"
//        INSERT INTO Bills (PatientId, DoctorId, AppointmentDate, Amount, PaymentStatus, PaymentDate)
//        VALUES (@PatientID, @DoctorID, @AppointmentDate, @Amount, @PaymentStatus, @PaymentDate)";

//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                await conn.OpenAsync();
//                using (SqlCommand cmd = new SqlCommand(query, conn))
//                {
//                    cmd.Parameters.AddWithValue("@PatientID", patientID);
//                    cmd.Parameters.AddWithValue("@DoctorID", doctorID);
//                    cmd.Parameters.AddWithValue("@AppointmentDate", appointmentDate);
//                    cmd.Parameters.AddWithValue("@Amount", consultationFee);
//                    cmd.Parameters.AddWithValue("@PaymentStatus", "Paid"); // Assuming immediate payment
//                    cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);

//                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
//                    return rowsAffected > 0;
//                }
//            }
//        }


//        // Get the next available token
//        private async Task<int> GetNextTokenAsync(int doctorID, DateTime date, SqlConnection conn, SqlTransaction transaction)
//        {
//            string query = "SELECT ISNULL(MAX(TokenId), 0) + 1 FROM Appointments WHERE DocId = @DoctorID AND AppoDate = @Date";

//            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
//            {
//                cmd.Parameters.AddWithValue("@DoctorID", doctorID);
//                cmd.Parameters.AddWithValue("@Date", date.Date);

//                object result = await cmd.ExecuteScalarAsync();
//                return result == DBNull.Value ? 1 : Convert.ToInt32(result);
//            }
//        }
//    }
//}
