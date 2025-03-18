using CMSv2026WebApp.Models;
using CMSv2026WebApp.Repositories;
using CMSv2026WebApp.Services;

namespace CMSv2026WebApp.Services
{
    public class LabTestService : ILabTestService
    {
        private readonly ILabTestRepository _labTestRepository;
        private readonly ILogger<LabTestService> _logger;

        public LabTestService(ILabTestRepository labTestRepository, ILogger<LabTestService> logger)
        {
            _labTestRepository = labTestRepository;
            _logger = logger;
        }

        // Lab Test Methods
        public void AddLabTest(LabTest labTest)
        {
            _labTestRepository.AddLabTest(labTest);
        }

        public List<LabTest> GetAllLabTests()
        {
            return _labTestRepository.GetAllLabTests();
        }

        public LabTest GetLabTestById(int labTestId)
        {
            return _labTestRepository.GetLabTestById(labTestId);
        }

        public void UpdateLabTest(LabTest labTest)
        {
            _labTestRepository.UpdateLabTest(labTest);
        }

        // Lab Test Prescription Methods
        public void AddLabTestPrescription(LabTestPrescription labTestPrescription)
        {
            _labTestRepository.AddLabTestPrescription(labTestPrescription);
        }

        public List<LabTestPrescription> GetAllLabTestPrescriptions()
        {
            return _labTestRepository.GetAllLabTestPrescriptions();
        }

        //public List<LabTestPrescription> GetPendingLabTestPrescriptions()
        //{
        //    // Simulate dummy data for pending prescriptions
        //    var pendingPrescriptions = new List<LabTestPrescription>
        //    {
        //        new LabTestPrescription
        //        {
        //            LabTestPrescriptionId = 1,
        //            LabTestId = 1,
        //            LabTest = new LabTest { TestName = "Blood Test" },
        //            AppointmentId = 1,
        //            Appointment = new Appointment
        //            {
        //                AppointmentDate = DateTime.Now.AddDays(1),
        //                Patient = new Patient { PatientName = "John Doe" },
        //                Doctor = new Doctor { Staff = new Staff { FullName = "Dr. Smith" } }
        //            },
        //            LabTestValue = null, // Pending
        //            Remarks = null, // Pending
        //            CreatedDate = DateTime.Now,
        //            IsCompleted = false // Pending
        //        },
        //        new LabTestPrescription
        //        {
        //            LabTestPrescriptionId = 2,
        //            LabTestId = 2,
        //            LabTest = new LabTest { TestName = "Urine Test" },
        //            AppointmentId = 2,
        //            Appointment = new Appointment
        //            {
        //                AppointmentDate = DateTime.Now.AddDays(2),
        //                Patient = new Patient { PatientName = "Jane Doe" },
        //                Doctor = new Doctor { Staff = new Staff { FullName = "Dr. Johnson" } }
        //            },
        //            LabTestValue = null, // Pending
        //            Remarks = null, // Pending
        //            CreatedDate = DateTime.Now,
        //            IsCompleted = false // Pending
        //        }
        //    };

        //    return pendingPrescriptions;
        //}

        public LabTestPrescription GetLabTestPrescriptionById(int id)
        {
            _logger.LogInformation("Fetching LabTestPrescription with ID {Id}", id);

            var prescription = _labTestRepository.GetLabTestPrescriptionById(id);

            if (prescription == null)
            {
                _logger.LogWarning("LabTestPrescription with ID {Id} not found", id);
            }

            return prescription;
        }

        public void UpdateLabTestPrescription(LabTestPrescription labTestPrescription)
        {
            _labTestRepository.UpdateLabTestPrescription(labTestPrescription);
        }

        public void AddLabTestReport(LabTestReport labTestReport)
        {
            _labTestRepository.AddLabTestReport(labTestReport);
        }

        public List<LabTestReport> GetAllLabTestReports()
        {
            return _labTestRepository.GetAllLabTestReports();
        }
    }

}


