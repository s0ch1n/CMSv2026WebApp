using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.Services
{
    public interface IDoctorService
    {
        List<Appointment> GetTodaysAppointments(int doctorId);
        List<Patient> SearchDoctorPatients(int doctorId, string searchQuery);
        List<Consultation> GetPatientConsultationHistory(int patientId);
        void AddConsultations(Consultation consultation);
        void UpdateConsultationStatus(int appointmentId, string status);
        void AddPrescriptions(MedicinePrescription prescription);
        void RequestLabTests(LabTestPrescription labTest);
        List<LabTestResult> GetThePatientLabResults(int appointmentId);
        void ReferPatients(Referral referral);
        List<Medicine> GetAllMedicines();
        List<LabTest> GetAllLabTests();
        List<string> GetMedicineNamesByTerm(string term);
        int? GetLabTestIdByName(string testName);
    }
}
