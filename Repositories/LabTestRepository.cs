using CMSv2026WebApp.Models;
using CMSv2026WebApp.ViewModel;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CMSv2026WebApp.Repositories
{
    public class LabTestRepository : ILabTestRepository
    {

        private readonly string _connectionString;

        public LabTestRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStrMVC");
        }

        // Lab Test Methods
        public void AddLabTest(LabTest labTest)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddLabTest", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TestName", labTest.TestName);
                cmd.Parameters.AddWithValue("@Amount", labTest.Amount);
                cmd.Parameters.AddWithValue("@ReferenceMinRange", labTest.ReferenceMinRange);
                cmd.Parameters.AddWithValue("@ReferenceMaxRange", labTest.ReferenceMaxRange);
                cmd.Parameters.AddWithValue("@SampleRequired", labTest.SampleRequired);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<LabTest> GetAllLabTests()
        {
            List<LabTest> labTests = new List<LabTest>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM LabTest WHERE IsActive = 1", con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        labTests.Add(new LabTest
                        {
                            LabTestId = Convert.ToInt32(reader["LabTestId"]),
                            TestName = reader["TestName"].ToString(),
                            Amount = Convert.ToDecimal(reader["Amount"]),
                            ReferenceMinRange = reader["ReferenceMinRange"] != DBNull.Value ? Convert.ToDecimal(reader["ReferenceMinRange"]) : (decimal?)null,
                            ReferenceMaxRange = reader["ReferenceMaxRange"] != DBNull.Value ? Convert.ToDecimal(reader["ReferenceMaxRange"]) : (decimal?)null,
                            SampleRequired = Convert.ToBoolean(reader["SampleRequired"])
                        });
                    }
                }
            }

            return labTests;
        }

        public LabTest GetLabTestById(int labTestId)
        {
            LabTest labTest = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM LabTest WHERE LabTestId = @LabTestId AND IsActive = 1", con);
                cmd.Parameters.AddWithValue("@LabTestId", labTestId);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        labTest = new LabTest
                        {
                            LabTestId = Convert.ToInt32(reader["LabTestId"]),
                            TestName = reader["TestName"].ToString(),
                            Amount = Convert.ToDecimal(reader["Amount"]),
                            ReferenceMinRange = reader["ReferenceMinRange"] != DBNull.Value ? Convert.ToDecimal(reader["ReferenceMinRange"]) : (decimal?)null,
                            ReferenceMaxRange = reader["ReferenceMaxRange"] != DBNull.Value ? Convert.ToDecimal(reader["ReferenceMaxRange"]) : (decimal?)null,
                            SampleRequired = Convert.ToBoolean(reader["SampleRequired"])
                        };
                    }
                }
            }

            return labTest;
        }

        public void UpdateLabTest(LabTest labTest)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateLabTest", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@LabTestId", labTest.LabTestId);
                cmd.Parameters.AddWithValue("@TestName", labTest.TestName);
                cmd.Parameters.AddWithValue("@Amount", labTest.Amount);
                cmd.Parameters.AddWithValue("@ReferenceMinRange", labTest.ReferenceMinRange);
                cmd.Parameters.AddWithValue("@ReferenceMaxRange", labTest.ReferenceMaxRange);
                cmd.Parameters.AddWithValue("@SampleRequired", labTest.SampleRequired);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Lab Test Prescription Methods
        public void AddLabTestPrescription(LabTestPrescription labTestPrescription)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddLabTestPrescription", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@LabTestId", labTestPrescription.LabTestId);
                cmd.Parameters.AddWithValue("@AppointmentId", labTestPrescription.AppointmentId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<LabTestPrescription> GetAllLabTestPrescriptions()
        {
            List<LabTestPrescription> prescriptions = new List<LabTestPrescription>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT 
                ltp.LabTestPrescriptionId,
                ltp.LabTestId,
                lt.TestName,
                ltp.AppointmentId,
                a.AppointmentDate,
                p.PatientName,
                s.FullName AS DoctorName,
                ltp.LabTestValue,
                ltp.Remarks,
                ltp.CreatedDate
            FROM LabTestPrescription ltp
            INNER JOIN LabTest lt ON ltp.LabTestId = lt.LabTestId
            INNER JOIN Appointment a ON ltp.AppointmentId = a.AppointmentId
            INNER JOIN Patient p ON a.PatientId = p.PatientId
            INNER JOIN Doctors d ON a.DoctorId = d.DoctorId
            INNER JOIN Staffs s ON d.StaffId = s.StaffId";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        prescriptions.Add(new LabTestPrescription
                        {
                            LabTestPrescriptionId = Convert.ToInt32(reader["LabTestPrescriptionId"]),
                            LabTestId = Convert.ToInt32(reader["LabTestId"]),
                            LabTest = new LabTest
                            {
                                TestName = reader["TestName"].ToString()
                            },
                            AppointmentId = Convert.ToInt32(reader["AppointmentId"]),
                            Appointment = new Appointment
                            {
                                AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                                Patient = new Patient
                                {
                                    PatientName = reader["PatientName"].ToString()
                                },
                                Doctor = new Doctor
                                {
                                    Staff = new Staff
                                    {
                                        FullName = reader["DoctorName"].ToString()
                                    }
                                }
                            },
                            LabTestValue = reader["LabTestValue"] != DBNull.Value ? reader["LabTestValue"].ToString() : null,
                            Remarks = reader["Remarks"] != DBNull.Value ? reader["Remarks"].ToString() : null,
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            //IsCompleted = Convert.ToBoolean(reader["IsCompleted"])
                        });
                    }
                }
            }

            return prescriptions;
        }


        public List<LabTestPrescription> GetPendingLabTestPrescriptions()
        {
            List<LabTestPrescription> prescriptions = new List<LabTestPrescription>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM LabTestPrescription WHERE IsCompleted = 0", con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        prescriptions.Add(new LabTestPrescription
                        {
                            LabTestPrescriptionId = Convert.ToInt32(reader["LabTestPrescriptionId"]),
                            LabTestId = Convert.ToInt32(reader["LabTestId"]),
                            AppointmentId = Convert.ToInt32(reader["AppointmentId"]),
                            LabTestValue = reader["LabTestValue"] != DBNull.Value ? reader["LabTestValue"].ToString() : null,
                            Remarks = reader["Remarks"] != DBNull.Value ? reader["Remarks"].ToString() : null,
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            IsCompleted = Convert.ToBoolean(reader["IsCompleted"])
                        });
                    }
                }
            }

            return prescriptions;
        }

        public LabTestPrescription GetLabTestPrescriptionById(int labTestPrescriptionId)
        {
            LabTestPrescription prescription = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = @"
        SELECT 
            ltp.*, 
            lt.TestName AS LabTestName, 
            a.AppointmentDate, 
            p.PatientName, 
            d.DoctorId, 
            s.FullName AS DoctorName
        FROM LabTestPrescription ltp
        LEFT JOIN LabTest lt ON ltp.LabTestId = lt.LabTestId
        LEFT JOIN Appointment a ON ltp.AppointmentId = a.AppointmentId
        LEFT JOIN Patient p ON a.PatientId = p.PatientId
        LEFT JOIN Doctors d ON a.DoctorId = d.DoctorId
        LEFT JOIN Staffs s ON d.StaffId = s.StaffId
        WHERE ltp.LabTestPrescriptionId = @LabTestPrescriptionId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@LabTestPrescriptionId", labTestPrescriptionId);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        prescription = new LabTestPrescription
                        {
                            LabTestPrescriptionId = Convert.ToInt32(reader["LabTestPrescriptionId"]),
                            LabTestId = reader["LabTestId"] != DBNull.Value ? Convert.ToInt32(reader["LabTestId"]) : 0,
                            AppointmentId = reader["AppointmentId"] != DBNull.Value ? Convert.ToInt32(reader["AppointmentId"]) : 0,
                            LabTestValue = reader["LabTestValue"] != DBNull.Value ? reader["LabTestValue"].ToString() : string.Empty,
                            Remarks = reader["Remarks"] != DBNull.Value ? reader["Remarks"].ToString() : string.Empty,
                            CreatedDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : DateTime.MinValue,
                            LabTest = new LabTest
                            {
                                TestName = reader["LabTestName"] != DBNull.Value ? reader["LabTestName"].ToString() : "Unknown Test"
                            },
                            Appointment = new Appointment
                            {
                                AppointmentDate = reader["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(reader["AppointmentDate"]) : DateTime.MinValue,
                                Patient = new Patient
                                {
                                    PatientName = reader["PatientName"] != DBNull.Value ? reader["PatientName"].ToString() : "Unknown Patient"
                                },
                                Doctor = new Doctor
                                {
                                    Staff = new Staff
                                    {
                                        FullName = reader["DoctorName"] != DBNull.Value ? reader["DoctorName"].ToString() : "Unknown Doctor"
                                    }
                                }
                            }
                        };

                    }
                }
            }

            return prescription;
        }

        public void UpdateLabTestPrescription(LabTestPrescription labTestPrescription)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateLabTestPrescription", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add all required parameters
                        cmd.Parameters.AddWithValue("@LabTestPrescriptionId", labTestPrescription.LabTestPrescriptionId);
                        cmd.Parameters.AddWithValue("@LabTestValue", labTestPrescription.LabTestValue ?? (object)DBNull.Value); // Handle null
                        cmd.Parameters.AddWithValue("@Remarks", labTestPrescription.Remarks ?? (object)DBNull.Value); // Handle null
                        cmd.Parameters.AddWithValue("@IsCompleted", labTestPrescription.IsCompleted);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error executing stored procedure sp_UpdateLabTestPrescription: {ex.Message}", ex);
            }
        }
        public void AddLabTestReport(LabTestReport labTestReport)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_AddLabTestReport", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LabTestPrescriptionId", labTestReport.LabTestPrescriptionId);
                        cmd.Parameters.AddWithValue("@TestName", labTestReport.TestName);
                        cmd.Parameters.AddWithValue("@PatientName", labTestReport.PatientName);
                        cmd.Parameters.AddWithValue("@PrescribedByDoctor", labTestReport.PrescribedByDoctor);
                        cmd.Parameters.AddWithValue("@GeneratedByLabTechnician", labTestReport.GeneratedByLabTechnician);
                        cmd.Parameters.AddWithValue("@ReportDate", labTestReport.ReportDate);
                        cmd.Parameters.AddWithValue("@Remarks", labTestReport.Remarks);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error executing stored procedure sp_AddLabTestReport: {ex.Message}", ex);
            }
        }

        public List<LabTestReport> GetAllLabTestReports()
        {
            List<LabTestReport> reports = new List<LabTestReport>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM LabTestReport", con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reports.Add(new LabTestReport
                        {
                            LabTestReportId = Convert.ToInt32(reader["LabTestReportId"]),
                            LabTestPrescriptionId = Convert.ToInt32(reader["LabTestPrescriptionId"]),
                            TestName = reader["TestName"].ToString(),
                            PrescribedByDoctor = reader["PrescribedByDoctor"].ToString(),
                            GeneratedByLabTechnician = reader["GeneratedByLabTechnician"].ToString(),
                            ReportDate = Convert.ToDateTime(reader["ReportDate"]),
                            Remarks = reader["Remarks"] != DBNull.Value ? reader["Remarks"].ToString() : null
                        });
                    }
                }
            }

            return reports;
        }

    }
}
//        public void AddLabTest(LabTest labTest)
//        {
//            using (SqlConnection con = new SqlConnection(_connectionString))
//            {
//                SqlCommand cmd = new SqlCommand("sp_AddLabTest", con);
//                cmd.CommandType = CommandType.StoredProcedure;

//                cmd.Parameters.AddWithValue("@LabTestName", labTest.LabTestName);
//                cmd.Parameters.AddWithValue("@RefMinRange", labTest.RefMinRange);
//                cmd.Parameters.AddWithValue("@RefMaxRange", labTest.RefMaxRange);
//                cmd.Parameters.AddWithValue("@SampleRequired", labTest.SampleRequired);
//                cmd.Parameters.AddWithValue("@LabTestCategoryId", labTest.LabTestCategoryId);

//                con.Open();
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    if (reader.Read())
//                    {
//                        labTest.LabTestId = Convert.ToInt32(reader["LabTestId"]);
//                        labTest.LabTestCategory = new LabTestCategory
//                        {
//                            LabTestCategoryId = Convert.ToInt32(reader["LabTestCategoryId"]),
//                            LabTestCategoryName = reader["LabTestCategoryName"].ToString()
//                        };
//                    }
//                }
//                con.Close();
//            }
//        }

//        public List<LabTest> GetAllLabTests()
//        {
//            List<LabTest> labTests = new List<LabTest>();

//            using (SqlConnection con = new SqlConnection(_connectionString))
//            {
//                SqlCommand cmd = new SqlCommand("sp_GetAllLabTests", con);
//                cmd.CommandType = CommandType.StoredProcedure;

//                con.Open();
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        LabTest labTest = new LabTest
//                        {
//                            LabTestId = Convert.ToInt32(reader["LabTestId"]),
//                            LabTestName = reader["LabTestName"].ToString(),
//                            RefMinRange = reader["RefMinRange"].ToString(),
//                            RefMaxRange = reader["RefMaxRange"].ToString(),
//                            SampleRequired = reader["SampleRequired"].ToString(),
//                            LabTestCategoryId = reader["LabTestCategoryId"] == DBNull.Value ?
//                                                (int?)null : Convert.ToInt32(reader["LabTestCategoryId"]),
//                            LabTestCategory = new LabTestCategory
//                            {
//                                LabTestCategoryId = reader["LabTestCategoryId"] == DBNull.Value ?
//                                                    0 : Convert.ToInt32(reader["LabTestCategoryId"]),
//                                LabTestCategoryName = reader["LabTestCategoryName"].ToString()
//                            }
//                        };

//                        labTests.Add(labTest);
//                    }
//                }
//                con.Close();
//            }

//            return labTests;
//        }

//        public List<LabTestCategory> GetLabTestCategories()
//        {
//            List<LabTestCategory> labTestCategories = new List<LabTestCategory>();

//            using (SqlConnection con = new SqlConnection(_connectionString))
//            {
//                SqlCommand cmd = new SqlCommand("sp_GetLabTestCategories", con);
//                cmd.CommandType = CommandType.StoredProcedure;

//                con.Open();
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        LabTestCategory labTestCategory = new LabTestCategory
//                        {
//                            LabTestCategoryId = Convert.ToInt32(reader["LabTestCategoryId"]),
//                            LabTestCategoryName = reader["LabTestCategoryName"].ToString()
//                        };

//                        labTestCategories.Add(labTestCategory);
//                    }
//                }
//                con.Close();
//            }

//            return labTestCategories;
//        }

//        public List<LabTestPrescription> GetLabTestPrescriptions()
//        {
//            List<LabTestPrescription> labTestPrescriptions = new List<LabTestPrescription>();

//            using (SqlConnection con = new SqlConnection(_connectionString))
//            {
//                SqlCommand cmd = new SqlCommand("GetPrescribedLabTests", con);
//                cmd.CommandType = CommandType.StoredProcedure;

//                con.Open();
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        LabTestPrescription labTestPrescription = new LabTestPrescription
//                        {
//                            LabTestPrescriptionId = reader["LabTestPrescriptionId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["LabTestPrescriptionId"]),
//                            LabTestName = reader["TestName"] == DBNull.Value ? string.Empty : reader["TestName"].ToString(),
//                            Amount = reader["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Amount"]),
//                            RefMinRange = reader["ReferenceMinRange"] == DBNull.Value ? string.Empty : reader["ReferenceMinRange"].ToString(),
//                            RefMaxRange = reader["ReferenceMaxRange"] == DBNull.Value ? string.Empty : reader["ReferenceMaxRange"].ToString(),
//                            LabTestValue = reader["LabTestValue"] == DBNull.Value ? string.Empty : reader["LabTestValue"].ToString(),
//                            ResultStatus = reader["ResultStatus"] == DBNull.Value ? string.Empty : reader["ResultStatus"].ToString(),
//                            PrescribedByDoctor = reader["PrescribedByDoctor"] == DBNull.Value ? string.Empty : reader["PrescribedByDoctor"].ToString(),
//                            PatientName = reader["PatientName"] == DBNull.Value ? string.Empty : reader["PatientName"].ToString(),
//                            BloodGroup = reader["BloodGroup"] == DBNull.Value ? string.Empty : reader["BloodGroup"].ToString(),
//                            AppointmentDate = reader["AppointmentDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["AppointmentDate"]),
//                        };

//                        labTestPrescriptions.Add(labTestPrescription);
//                    }
//                }
//                con.Close();
//            }

//            Console.WriteLine($"Fetched {labTestPrescriptions.Count} lab test prescriptions.");
//            return labTestPrescriptions;
//        }
//        public void UpdateLabTestPrescription(LabTestPrescription labTestPrescription)
//        {
//            using (SqlConnection con = new SqlConnection(_connectionString))
//            {
//                SqlCommand cmd = new SqlCommand("sp_UpdateLabTestPrescription", con);
//                cmd.CommandType = CommandType.StoredProcedure;

//                cmd.Parameters.AddWithValue("@LabTestPrescriptionId", labTestPrescription.LabTestPrescriptionId);
//                cmd.Parameters.AddWithValue("@Remarks", labTestPrescription.Remarks);

//                con.Open();
//                cmd.ExecuteNonQuery();
//                con.Close();
//            }
//        }

//        public LabTestPrescription GetLabTestPrescriptionById(int labTestPrescriptionId)
//        {
//            LabTestPrescription labTestPrescription = null;
//            using (SqlConnection con = new SqlConnection(_connectionString))
//            {
//                SqlCommand cmd = new SqlCommand("sp_GetLabTestPrescriptionById", con);
//                cmd.CommandType = CommandType.StoredProcedure;
//                cmd.Parameters.AddWithValue("@LabTestPrescriptionId", labTestPrescriptionId);

//                con.Open();
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    if (reader.Read())
//                    {
//                        labTestPrescription = new LabTestPrescription
//                        {
//                            LabTestPrescriptionId = Convert.ToInt32(reader["LabTestPrescriptionId"]),
//                            LabTestId = Convert.ToInt32(reader["LabTestId"]),
//                            Remarks = reader["Remarks"].ToString(),
//                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
//                            ConsultationBillId = Convert.ToInt32(reader["ConsultationBillId"]),
//                            LabTest = new LabTest
//                            {
//                                LabTestId = Convert.ToInt32(reader["LabTestId"]),
//                                LabTestName = reader["LabTestName"].ToString(),
//                                RefMinRange = reader["RefMinRange"].ToString(),
//                                RefMaxRange = reader["RefMaxRange"].ToString(),
//                                SampleRequired = reader["SampleRequired"].ToString(),
//                                LabTestCategory = new LabTestCategory
//                                {
//                                    LabTestCategoryName = reader["LabTestCategoryName"].ToString()
//                                }
//                            },
//                            ConsultationBill = new ConsultationBill
//                            {
//                                ConsultationBillId = Convert.ToInt32(reader["ConsultationBillId"])
//                            },
//                            PatientName = reader["PatientName"].ToString()
//                        };
//                    }
//                }
//                con.Close();
//            }
//            return labTestPrescription;
//        }
//    }
//}
//public void AddTestValue(int labTestPrescriptionId, string testValue, string remarks)
//        {
//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                SqlCommand cmd = new SqlCommand("UpdateLabTestValue", conn)
//                {
//                    CommandType = CommandType.StoredProcedure
//                };

//                cmd.Parameters.AddWithValue("@LabTestPrescriptionId", labTestPrescriptionId);
//                cmd.Parameters.AddWithValue("@LabTestValue", testValue);
//                cmd.Parameters.AddWithValue("@Remarks", remarks);

//                conn.Open();
//                cmd.ExecuteNonQuery();
//            }
//        }

//        public List<LabTestPrescription> GetPrescribedLabTests()
//        {
//            var prescriptions = new List<LabTestPrescription>();

//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                SqlCommand cmd = new SqlCommand("GetPrescribedLabTests", conn)
//                {
//                    CommandType = CommandType.StoredProcedure
//                };

//                conn.Open();
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        prescriptions.Add(new LabTestPrescription
//                        {
//                            LabTestPrescriptionId = Convert.ToInt32(reader["LabTestPrescriptionId"]),
//                            LabTestValue = reader["LabTestValue"].ToString(),
//                            LabTest = new LabTest
//                            {
//                                TestName = reader["TestName"].ToString(),
//                                Amount = reader["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Amount"]),
//                                ReferenceMinRange = reader["ReferenceMinRange"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ReferenceMinRange"]),
//                                ReferenceMaxRange = reader["ReferenceMaxRange"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ReferenceMaxRange"])
//                            },
//                            Remarks = reader["ResultStatus"].ToString(),
//                            Appointment = new Appointment
//                            {
//                                AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"])
//                            },
//                            DoctorName = reader["PrescribedByDoctor"].ToString(),
//                            PatientName = reader["PatientName"].ToString()
//                        });
//                    }
//                }
//            }

//            return prescriptions;
//        }
//        public List<LabTestPrescriptionViewModel> GetPrescribedLabTestss()
//        {
//            var prescriptions = new List<LabTestPrescriptionViewModel>();

//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                SqlCommand cmd = new SqlCommand("GetPrescribedLabTests", conn)
//                {
//                    CommandType = CommandType.StoredProcedure
//                };

//                conn.Open();
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        prescriptions.Add(new LabTestPrescriptionViewModel
//                        {
//                            LabTestPrescriptionId = Convert.ToInt32(reader["LabTestPrescriptionId"]),
//                            TestName = reader["TestName"].ToString(),
//                            DoctorName = reader["DoctorName"].ToString(),
//                            PatientName = reader["PatientName"].ToString(),
//                            LabTestValue = reader["LabTestValue"].ToString()
//                        });
//                    }
//                }
//            }

//            return prescriptions;
//        }
//        public void GenerateLabTestReport(int labTestPrescriptionId)
//        {
//            using (SqlConnection conn = new SqlConnection(_connectionString))
//            {
//                SqlCommand cmd = new SqlCommand("GenerateLabTestReport", conn)
//                {
//                    CommandType = CommandType.StoredProcedure
//                };

//                cmd.Parameters.AddWithValue("@LabTestPrescriptionId", labTestPrescriptionId);
//                conn.Open();
//                cmd.ExecuteNonQuery();
//            }
//        }

//        public LabTestPrescription GetPrescriptionById(int id)
//        {
//            LabTestPrescription prescription = null;

//            using (SqlConnection con = new SqlConnection(_connectionString))
//            {
//                SqlCommand cmd = new SqlCommand("GetLabTestPrescriptionById", con)
//                {
//                    CommandType = CommandType.StoredProcedure
//                };
//                cmd.Parameters.AddWithValue("@LabTestPrescriptionId", id);

//                con.Open();
//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    if (reader.Read())
//                    {
//                        prescription = new LabTestPrescription
//                        {
//                            LabTestPrescriptionId = Convert.ToInt32(reader["LabTestPrescriptionId"]),
//                            LabTestValue = reader["LabTestValue"].ToString(),
//                            Remarks = reader["Remarks"].ToString(),
//                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
//                            DoctorName = reader["DoctorName"].ToString(),
//                            PatientName = reader["PatientName"].ToString(),
//                            LabTest = new LabTest
//                            {
//                                TestName = reader["TestName"].ToString(),
//                                Amount = reader["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Amount"]),
//                                ReferenceMinRange = reader["ReferenceMinRange"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ReferenceMinRange"]),
//                                ReferenceMaxRange = reader["ReferenceMaxRange"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["ReferenceMaxRange"])
//                            },
//                            Appointment = new Appointment
//                            {
//                                AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"])
//                            }
//                        };
//                    }
//                }
//            }

//            return prescription;
//        }
//    }
//}

//       public async Task<List<LabTestPrescription>> GetPrescribedLabTestsByDoctor(int doctorId)
//        {
//            var labTests = new List<LabTestPrescription>();
//            using (var connection = new SqlConnection(_connectionString))
//            {
//                await connection.OpenAsync();
//                using (var command = new SqlCommand("GetPrescribedLabTestsByDoctor", connection))
//                {
//                    command.CommandType = CommandType.StoredProcedure;
//                    command.Parameters.AddWithValue("@DoctorId", doctorId);

//                    using (var reader = await command.ExecuteReaderAsync())
//                    {
//                        while (await reader.ReadAsync())
//                        {
//                            labTests.Add(new LabTestPrescription
//                            {
//                                LabTestPrescriptionId = reader.GetInt32(0),
//                                TestName = reader.GetString(1),
//                                LabTestValue = reader.GetString(2),
//                                ReferenceMinRange = reader.GetDecimal(3),
//                                ReferenceMaxRange = reader.GetDecimal(4),
//                                ResultStatus = reader.GetString(5),
//                                PatientName = reader.GetString(6),
//                                AppointmentDate = reader.GetDateTime(7)
//                            });
//                        }
//                    }
//                }
//            }
//            return labTests;
//        }

//        // Generate Lab Test Report
//        public async Task<string> GenerateLabTestReport(int labTestPrescriptionId)
//        {
//            using (var connection = new SqlConnection(_connectionString))
//            {
//                await connection.OpenAsync();
//                using (var command = new SqlCommand("GenerateLabTestReport", connection))
//                {
//                    command.CommandType = CommandType.StoredProcedure;
//                    command.Parameters.AddWithValue("@LabTestPrescriptionId", labTestPrescriptionId);

//                    var reportText = await command.ExecuteScalarAsync() as string;
//                    return reportText;
//                }
//            }
//        }
//    }
//}