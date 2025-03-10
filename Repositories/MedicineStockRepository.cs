using CMSv2026WebApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CMSv2026WebApp.Repositories
{
    public class MedicineStockRepository:IMedicineStockRepository
    {
        private readonly string _connectionString;

        public MedicineStockRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStrMVC");
        }

        public IEnumerable<MedicineStock> GetAll()
        {
            List<MedicineStock> stocks = new List<MedicineStock>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllMedicineStock", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stocks.Add(new MedicineStock
                        {
                            MedicineStockId = Convert.ToInt32(reader["MedicineStockId"]),
                            StockInHand = Convert.ToInt32(reader["StockInHand"]),
                            ReOrderLevel = Convert.ToInt32(reader["ReOrderLevel"]),
                            Purchase = Convert.ToInt32(reader["Purchase"]),
                            Issuance = Convert.ToInt32(reader["Issuance"]),
                            MedicineId = Convert.ToInt32(reader["MedicineId"]),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            IsActive = Convert.ToBoolean(reader["IsActive"])
                        });
                    }
                }
            }
            return stocks;
        }

        public void Add(MedicineStock stock)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddMedicineStock", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@StockInHand", stock.StockInHand);
                cmd.Parameters.AddWithValue("@ReOrderLevel", stock.ReOrderLevel);
                cmd.Parameters.AddWithValue("@Purchase", stock.Purchase);
                cmd.Parameters.AddWithValue("@Issuance", stock.Issuance);
                cmd.Parameters.AddWithValue("@MedicineId", stock.MedicineId);
                cmd.Parameters.AddWithValue("@CreatedDate", stock.CreatedDate);
                cmd.Parameters.AddWithValue("@IsActive", stock.IsActive);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(MedicineStock stock)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateMedicineStock", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@MedicineStockId", stock.MedicineStockId);
                cmd.Parameters.AddWithValue("@StockInHand", stock.StockInHand);
                cmd.Parameters.AddWithValue("@ReOrderLevel", stock.ReOrderLevel);
                cmd.Parameters.AddWithValue("@Purchase", stock.Purchase);
                cmd.Parameters.AddWithValue("@Issuance", stock.Issuance);
                cmd.Parameters.AddWithValue("@MedicineId", stock.MedicineId);
                cmd.Parameters.AddWithValue("@IsActive", stock.IsActive);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteMedicineStock", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@MedicineStockId", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

    

