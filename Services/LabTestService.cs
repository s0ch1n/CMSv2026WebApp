using CMSv2026WebApp.Models;
using CMSv2026WebApp.Repositories;
using CMSv2026WebApp.Services;

namespace CMSv2026WebApp.Services
{
    public class LabTestService : ILabTestService
    {
        private readonly ILabTestRepository _labTestRepository;
        public LabTestService(ILabTestRepository labTestRepository)
        {
            _labTestRepository = labTestRepository;
        }
        public void AddLabTest(LabTest labTest)
        {
            _labTestRepository.AddLabTest(labTest);
        }
        public List<LabTest> GetAllLabTests()
        {
            return _labTestRepository.GetAllLabTests();
        }
        public List<LabTestCategory> GetLabTestCategories()
        {
            return _labTestRepository.GetLabTestCategories();
        }

        public LabTestPrescription GetLabTestPrescriptionById(int labTestPrescriptionId)
        {
            throw new NotImplementedException();
        }

        public List<LabTestPrescription> GetLabTestPrescriptions()
        {
            throw new NotImplementedException();
        }

        public void UpdateLabTestPrescription(LabTestPrescription labTestPrescription)
        {
            throw new NotImplementedException();
        }
    }
}


