using CMSv2026WebApp.Models;
using CMSv2026WebApp.ViewModel;
using Microsoft.Data.SqlClient;

namespace CMSv2026WebApp.Repositories
{
    public class PharmacistRepository : IPharmacistRepository
    {
        private readonly string _connectionString;
        public PharmacistRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStrMVC");
        }

        public int AddMedicine(Medicine medicine)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Medicine (MedicineName, MedicineTypeId, Unit, ExpiryDate) 
                         OUTPUT INSERTED.MedicineId 
                         VALUES (@MedicineName, @MedicineTypeId, @Unit, @ExpiryDate)";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MedicineName", medicine.MedicineName);
                command.Parameters.AddWithValue("@MedicineTypeId", medicine.MedicineTypeId);
                command.Parameters.AddWithValue("@Unit", medicine.Unit);
                command.Parameters.AddWithValue("@ExpiryDate", medicine.ExpiryDate);

                connection.Open();
                return (int)command.ExecuteScalar(); // Returns the inserted MedicineId
            }
        }



        public void AddStock(int medicineId, int quantity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
            IF EXISTS (SELECT 1 FROM MedicineStock WHERE MedicineId = @MedicineId)
                UPDATE MedicineStock SET StockInHand = StockInHand + @Quantity WHERE MedicineId = @MedicineId
            ELSE
                INSERT INTO MedicineStock (MedicineId, StockInHand) VALUES (@MedicineId, @Quantity)";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MedicineId", medicineId);
                command.Parameters.AddWithValue("@Quantity", quantity);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }


        public void DeleteMedicine(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Medicine WHERE MedicineId = @MedicineId";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MedicineId", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }


        public List<Medicine> GetAllMedicines()
        {
            var medicines = new List<Medicine>();

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
                                SELECT 
                            m.MedicineId, 
                            m.MedicineName, 
                            m.MedicineTypeId,   
                            mt.MedicineTypeName,  
                            m.Unit,  
                            m.ExpiryDate, 
                            ISNULL(ms.StockInHand, 0) AS StockInHand
                        FROM Medicine m
                        LEFT JOIN MedicineStock ms ON m.MedicineId = ms.MedicineId 
                        LEFT JOIN MedicineType mt ON m.MedicineTypeId = mt.MedicineTypeId";

                var command = new SqlCommand(query, connection);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        medicines.Add(new Medicine
                        {
                            MedicineId = Convert.ToInt32(reader["MedicineId"]),
                            MedicineName = reader["MedicineName"].ToString(),
                            MedicineTypeId = Convert.ToInt32(reader["MedicineTypeId"]),
                            MedicineTypeName = reader["MedicineTypeName"].ToString(),
                            ExpiryDate = Convert.ToDateTime(reader["ExpiryDate"]),
                            Unit = Convert.ToInt32(reader["Unit"]) // Fetching stock value
                        });
                    }
                }
            }
            return medicines;
        }


        public Medicine GetMedicineById(int id)
        {
            Medicine medicine = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT m.MedicineId, m.MedicineName, m.MedicineTypeId, m.Unit, m.ExpiryDate, 
                   ISNULL(ms.StockInHand, 0) AS StockInHand
            FROM Medicine m
            LEFT JOIN MedicineStock ms ON m.MedicineId = ms.MedicineId
            WHERE m.MedicineId = @MedicineId";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MedicineId", id);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        medicine = new Medicine
                        {
                            MedicineId = Convert.ToInt32(reader["MedicineId"]),
                            MedicineName = reader["MedicineName"].ToString(),
                            MedicineTypeId = Convert.ToInt32(reader["MedicineTypeId"]),
                            ExpiryDate = Convert.ToDateTime(reader["ExpiryDate"]),
                            Unit = Convert.ToInt32(reader["Unit"]) // Fetching stock value// Fetching stock value
                        };
                    }
                }
            }
            return medicine;
        }


        public List<MedicineType> GetMedicineTypes()
        {
            var medicineTypes = new List<MedicineType>();

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM MedicineType";
                var command = new SqlCommand(query, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        medicineTypes.Add(new MedicineType
                        {
                            MedicineTypeId = Convert.ToInt32(reader["MedicineTypeId"]),
                            MedicineTypeName = reader["MedicineTypeName"].ToString()
                        });
                    }
                }
            }
            return medicineTypes;
        }

        public MedicineStock GetStockByMedicineId(int medicineId)
        {
            MedicineStock stock = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM MedicineStock WHERE MedicineId = @MedicineId";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MedicineId", medicineId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        stock = new MedicineStock
                        {
                            MedicineId = Convert.ToInt32(reader["MedicineId"]),
                            StockInHand = Convert.ToInt32(reader["StockInHand"])
                        };
                    }
                }
            }
            return stock;
        }

        public void ReduceStock(int medicineId, int quantity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE MedicineStock SET StockInHand = StockInHand - @Issuance WHERE MedicineId = @MedicineId";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MedicineId", medicineId);
                command.Parameters.AddWithValue("@Issuance", quantity);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void RemoveStock(int medicineId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM MedicineStock WHERE MedicineId = @MedicineId";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MedicineId", medicineId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateMedicine(Medicine medicine)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
            UPDATE Medicine 
            SET MedicineName = @MedicineName, 
                MedicineTypeId = @MedicineTypeId,  
                ExpiryDate = @ExpiryDate 
            WHERE MedicineId = @MedicineId;

            UPDATE MedicineStock 
            SET StockInHand = @StockInHand 
            WHERE MedicineId = @MedicineId;";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MedicineId", medicine.MedicineId);
                command.Parameters.AddWithValue("@MedicineName", medicine.MedicineName);
                command.Parameters.AddWithValue("@MedicineTypeId", medicine.MedicineTypeId);
                //command.Parameters.AddWithValue("@Unit", medicine.Unit);
                command.Parameters.AddWithValue("@ExpiryDate", medicine.ExpiryDate);
                command.Parameters.AddWithValue("@StockInHand", medicine.StockInHand); // Include stock update

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public MedicinePrescription GetTodaysPrescriptionById(int medicinePrescriptionId)
        {
            MedicinePrescription prescription = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT mp.MedicinePrescriptionId, mp.MedicineId, m.MedicineName, mp.Dosage, mp.Frequency, mp.Duration, 
                   mp.CreatedDate, mp.AppointmentId,
                   a.PatientId, p.PatientName,
                   a.DoctorId, s.StaffName AS DoctorName
            FROM MedicinePrescription mp
            INNER JOIN Medicine m ON mp.MedicineId = m.MedicineId
            INNER JOIN Appointment a ON mp.AppointmentId = a.AppointmentId
            INNER JOIN Patient p ON a.PatientId = p.PatientId
            INNER JOIN Doctor d ON a.DoctorId = d.DoctorId
            INNER JOIN Staff s ON d.StaffId = s.StaffId
            WHERE mp.MedicinePrescriptionId = @MedicinePrescriptionId 
            AND CAST(mp.CreatedDate AS DATE) = CAST(GETDATE() AS DATE);"; //Fetch only today's prescriptions
        
        var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MedicinePrescriptionId", medicinePrescriptionId);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        prescription = new MedicinePrescription
                        {
                            MedicinePrescriptionId = Convert.ToInt32(reader["MedicinePrescriptionId"]),
                            MedicineId = Convert.ToInt32(reader["MedicineId"]),
                            MedicineName = reader["MedicineName"].ToString(),
                            Dosage = reader["Dosage"].ToString(),
                            Frequency = reader["Frequency"].ToString(),
                            Duration = reader["Duration"].ToString(),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            AppointmentId = Convert.ToInt32(reader["AppointmentId"]),

                            // New fields
                            Appointment = new Appointment
                            {
                                PatientId = Convert.ToInt32(reader["PatientId"]),
                                DoctorId = Convert.ToInt32(reader["DoctorId"]),
                                Patient = new Patient
                                {
                                    PatientId = Convert.ToInt32(reader["PatientId"]),
                                    PatientName = reader["PatientName"].ToString()
                                },
                                Doctor = new Doctor
                                {
                                    DoctorId = Convert.ToInt32(reader["DoctorId"]),
                                    Staff = new Staff
                                    {
                                        FullName = reader["DoctorName"].ToString()
                                    }
                                }
                            }
                        };
                    }
                }
            }
            return prescription;
        }

        public void DispatchMedicine(int medicineId, int quantity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Check stock availability
                        string checkStockQuery = "SELECT StockInHand FROM MedicineStock WHERE MedicineId = @MedicineId";
                        var checkCommand = new SqlCommand(checkStockQuery, connection, transaction);
                        checkCommand.Parameters.AddWithValue("@MedicineId", medicineId);

                        int currentStock = Convert.ToInt32(checkCommand.ExecuteScalar() ?? 0);

                        if (currentStock < quantity)
                        {
                            throw new ApplicationException("Insufficient stock available.");
                        }

                        // Reduce stock
                        string updateStockQuery = "UPDATE MedicineStock SET StockInHand = StockInHand - @Quantity WHERE MedicineId = @MedicineId";
                        var updateCommand = new SqlCommand(updateStockQuery, connection, transaction);
                        updateCommand.Parameters.AddWithValue("@MedicineId", medicineId);
                        updateCommand.Parameters.AddWithValue("@Quantity", quantity);
                        updateCommand.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<DispatchMedicineViewModel> GetTodaysPrescriptions()
        {
            var prescriptions = new List<DispatchMedicineViewModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
                SELECT 
                    mp.MedicinePrescriptionId AS PrescriptionId,
                    p.PatientName,
                    s.FullName AS DoctorName,
                    m.MedicineId,
                    m.MedicineName,
                    mp.Dosage,
                    mp.Frequency,
                    mp.Duration,
                    ISNULL(ms.StockInHand, 0) AS AvailableStock
                FROM MedicinePrescription mp
                INNER JOIN Appointment a ON mp.AppointmentId = a.AppointmentId
                INNER JOIN Patient p ON a.PatientId = p.PatientId
                INNER JOIN Doctors d ON a.DoctorId = d.DoctorId
                INNER JOIN Staffs s ON d.StaffId = s.StaffId
                INNER JOIN Medicine m ON mp.MedicineId = m.MedicineId
                LEFT JOIN MedicineStock ms ON m.MedicineId = ms.MedicineId
                WHERE CAST(mp.CreatedDate AS DATE) = CAST(GETDATE() AS DATE);
            ";

                var command = new SqlCommand(query, connection);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        prescriptions.Add(new DispatchMedicineViewModel
                        {
                            PrescriptionId = Convert.ToInt32(reader["PrescriptionId"]),
                            PatientName = reader["PatientName"].ToString(),
                            DoctorName = reader["DoctorName"].ToString(),
                            MedicineId = Convert.ToInt32(reader["MedicineId"]),
                            MedicineName = reader["MedicineName"].ToString(),
                            Dosage = reader["Dosage"].ToString(),
                            Frequency = reader["Frequency"].ToString(),
                            Duration = reader["Duration"].ToString(),
                            AvailableStock = Convert.ToInt32(reader["AvailableStock"])
                        });
                    }
                }
            }

            return prescriptions;
        }
    }
}
