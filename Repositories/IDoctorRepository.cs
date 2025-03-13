using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.Repositories
{
    public interface IDoctorRepository
    {
        List<Appointment> GetTodaysAppointments(int doctorId);
        List<Patient> SearchDoctorPatients(int doctorId, string searchQuery);
        List<Consultation> GetPatientConsultationHistory(int patientId);
        void AddConsultation(Consultation consultation);
        void UpdateConsultationStatus(int appointmentId, string status);
        void AddPrescription(MedicinePrescription prescription);
        void RequestLabTest(LabTestPrescription labTest);
        List<LabTestResult> GetPatientLabResults(int appointmentId);
        void ReferPatient(Referral referral);
        List<Medicine> GetAllMedicines();
        List<LabTest> GetAllLabTests();
        Doctor GetDoctorByStaffId(int staffId);
        List<string> GetMedicineNamesByTerm(string term);
        int? GetLabTestIdByName(string testName);

    }
}
