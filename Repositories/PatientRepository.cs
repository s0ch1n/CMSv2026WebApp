using CMSv2026WebApp.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CMSv2026WebApp.Repositories
{
    public class PatientRepository : IPatientRepository
    {

        //Fields
        private readonly string _connectionString;

        public PatientRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnStrMVC");
        }

        public void AddConsultation(Consultation consultation)
        {
            var query = "INSERT INTO Consultation (AppointmentId, Symptoms, Diagnosis, Notes, CreatedDate, IsActive) VALUES (@AppointmentId, @Symptoms, @Diagnosis, @Notes, @CreatedDate, @IsActive)";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(query, consultation);
            }
        }

        public void AddLabTest(LabTestPrescription labTest)
        {
            var query = "INSERT INTO LabTestPrescription (AppointmentId, LabTestValue, Remarks, CreatedDate) VALUES (@AppointmentId, @LabTestValue, @Remarks, @CreatedDate)";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(query, labTest);
            }
        }

        public void AddPrescription(MedicinePrescription prescription)
        {
            var query = "INSERT INTO MedicinePrescription (AppointmentId, MedicineId, Dosage, Frequency, Duration, CreatedDate) VALUES (@AppointmentId, @MedicineId, @Dosage, @Frequency, @Duration, @CreatedDate)";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(query, prescription);
            }
        }

        public List<Patient> GetAllPatients()
        {
            var query = "SELECT * FROM Patient";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var patients = connection.Query<Patient>(query).ToList();
                return patients;
            }
        }

        public Patient GetPatientById(int patientId)
        {
            var query = "SELECT * FROM Patient WHERE PatientId = @PatientId";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var patient = connection.QuerySingleOrDefault<Patient>(query, new { PatientId = patientId });
                return patient;
            }
        }
    }
}
