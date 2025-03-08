using CMSv2026WebApp.Models;
using CMSv2026WebApp.Repositories;

namespace CMSv2026WebApp.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public void AddConsultations(Consultation consultation)
        {
            _doctorRepository.AddConsultation(consultation);
        }
        public List<Medicine> GetAllMedicines()
        {
            return _doctorRepository.GetAllMedicines();
        }

        public List<LabTest> GetAllLabTests()
        {
            return _doctorRepository.GetAllLabTests();
        }
        public void AddPrescriptions(MedicinePrescription prescription)
        {
            _doctorRepository.AddPrescription(prescription);
        }
        public void UpdateConsultationStatus(int appointmentId, string status)
        {
            _doctorRepository.UpdateConsultationStatus(appointmentId, status);
        }

        public List<Consultation> GetPatientConsultationHistory(int patientId)
        {
            return _doctorRepository.GetPatientConsultationHistory(patientId);
        }

        public List<LabTestResult> GetThePatientLabResults(int appointmentId)
        {
            return _doctorRepository.GetPatientLabResults(appointmentId);
        }

        public List<Appointment> GetTodaysAppointments(int doctorId)
        {
            return _doctorRepository.GetTodaysAppointments(doctorId);
        }

        public void ReferPatients(Referral referral)
        {
            _doctorRepository.ReferPatient(referral);
        }

        public void RequestLabTests(LabTestPrescription labTest)
        {
            _doctorRepository.RequestLabTest(labTest);
        }

        public List<Patient> SearchDoctorPatients(int doctorId, string searchQuery)
        {
            return _doctorRepository.SearchDoctorPatients(doctorId, searchQuery);
        }
    }
}
