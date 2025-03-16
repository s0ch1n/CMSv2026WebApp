using CMSv2026WebApp.Models;
using CMSv2026WebApp.Repositories;
using CMSv2026WebApp.ViewModel;


namespace CMSv2026WebApp.Services
{
    public class PharmacistService : IPharmacistService
    {
        //field

        private readonly IPharmacistRepository _pharmacistRepository;

        //DI
        public PharmacistService(IPharmacistRepository pharmacistRepository)
        {
            _pharmacistRepository = pharmacistRepository;
        }

        public int AddMedicine(Medicine medicine)
        {
           return _pharmacistRepository.AddMedicine(medicine);
            
        }

        public void AddStock(int medicineId, int quantity)
        {
            _pharmacistRepository.AddStock(medicineId, quantity);
        }

        public void DeleteMedicine(int id)
        {
            _pharmacistRepository.DeleteMedicine(id);
        }

        public void DispatchMedicine(int medicineId, int quantity)
        {
            _pharmacistRepository.DispatchMedicine(medicineId, quantity);
        }

        public List<Medicine> GetAllMedicines()
        {
            return _pharmacistRepository.GetAllMedicines();
        }

        public Medicine GetMedicineById(int id)
        {
            return _pharmacistRepository.GetMedicineById(id);
        }

        public List<MedicineType> GetMedicineTypes()
        {
            return _pharmacistRepository.GetMedicineTypes() ?? new List<MedicineType>();
        }

        public MedicineStock GetStockByMedicineId(int medicineId)
        {
            return _pharmacistRepository.GetStockByMedicineId(medicineId);
        }

        public MedicinePrescription GetTodaysPrescriptionById(int prescriptionId)
        {
            return _pharmacistRepository.GetTodaysPrescriptionById(prescriptionId);
        }

        public List<DispatchMedicineViewModel> GetTodaysPrescriptions()
        {
            return _pharmacistRepository.GetTodaysPrescriptions();
        }

        public void ReduceStock(int medicineId, int quantity)
        {
            _pharmacistRepository.ReduceStock(medicineId, quantity);
        }

        public void RemoveStock(int medicineId)
        {
            _pharmacistRepository.RemoveStock(medicineId);
        }

        public void UpdateMedicine(Medicine medicine)
        {
            _pharmacistRepository.UpdateMedicine(medicine);
        }
    }
}
