using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.Repositories
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly string _connectionString;
        public MedicineRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStrMVC");
        }

        // ✅ Get all medicines
        public IEnumerable<Medicine> GetAllMedicines()
        {
            List<Medicine> medicines = new List<Medicine>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllMedicines", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        medicines.Add(new Medicine
                        {
                            MedicineId = Convert.ToInt32(reader["MedicineId"]),
                            MedicineName = reader["MedicineName"].ToString(),
                            ExpiryDate = Convert.ToDateTime(reader["ExpiryDate"]),
                            Unit = reader["Unit"].ToString(),
                            MedicineTypeId = Convert.ToInt32(reader["MedicineTypeId"]),
                            IsActive = Convert.ToBoolean(reader["IsActive"]),
                            MedicineType = new MedicineType
                            {
                                MedicineTypeId = Convert.ToInt32(reader["MedicineTypeId"]),
                                MedicineTypeName = reader["MedicineTypeName"].ToString()
                            }
                        });
                    }
                }
            }
            return medicines;
        }

        // ✅ Get medicine by name
        public Medicine GetMedicineByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Medicine name cannot be empty.", nameof(name));
            }

            Medicine medicine = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetMedicineByName", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MedicineName", SqlDbType.NVarChar, 100)
                    {
                        Value = name.Trim()
                    });

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            medicine = new Medicine
                            {
                                MedicineId = reader.GetInt32(reader.GetOrdinal("MedicineId")),
                                MedicineName = reader.GetString(reader.GetOrdinal("MedicineName")),
                                ExpiryDate = reader.GetDateTime(reader.GetOrdinal("ExpiryDate")),
                                Unit = reader.GetString(reader.GetOrdinal("Unit")),
                                MedicineTypeId = reader.GetInt32(reader.GetOrdinal("MedicineTypeId")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                MedicineType = new MedicineType
                                {
                                    MedicineTypeId = reader.GetInt32(reader.GetOrdinal("MedicineTypeId")),
                                    MedicineTypeName = reader.GetString(reader.GetOrdinal("MedicineTypeName"))
                                }
                            };
                        }
                    }
                }
            }
            return medicine;
        }



        // ✅ Add new medicine
        public void AddMedicine(Medicine medicine)
        {
            Console.WriteLine("Inside AddMedicine() Repository Method!");

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddMedicine", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@MedicineName", medicine.MedicineName);
                cmd.Parameters.AddWithValue("@ExpiryDate", medicine.ExpiryDate);
                cmd.Parameters.AddWithValue("@MedicineTypeId", medicine.MedicineTypeId);
                cmd.Parameters.AddWithValue("@Unit", medicine.Unit);
                cmd.Parameters.AddWithValue("@IsActive", medicine.IsActive);

                con.Open();
                cmd.ExecuteNonQuery();  // ✅ Runs the SQL stored procedure
            }
        }

        // ✅ Update medicine
        public void UpdateMedicine(Medicine medicine)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateMedicine", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@MedicineId", medicine.MedicineId);
                cmd.Parameters.AddWithValue("@MedicineName", medicine.MedicineName);
                cmd.Parameters.AddWithValue("@ExpiryDate", medicine.ExpiryDate);
                cmd.Parameters.AddWithValue("@Unit", medicine.Unit);
                cmd.Parameters.AddWithValue("@MedicineTypeId", medicine.MedicineTypeId);
                cmd.Parameters.AddWithValue("@IsActive", medicine.IsActive);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        // ✅ Delete medicine by name
        public void DeleteMedicine(string name)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteMedicine", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@MedicineName", name);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        // ✅ Get all Medicine Types (For Dropdown)
        public List<MedicineType> GetMedicineTypes()
        {
            List<MedicineType> medicineTypes = new List<MedicineType>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllMedicineTypes", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
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

    }
}