using System.Collections.Generic;
using CMSv2026WebApp.Models;
using CMSv2026WebApp.Repositories;

namespace CMSv2026WebApp.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly IMedicineRepository _medicineRepository;

        public MedicineService(IMedicineRepository medicineRepository)
        {
            _medicineRepository = medicineRepository;
        }

        // ✅ Get all medicines
        public IEnumerable<Medicine> GetAllMedicines()
        {
            return _medicineRepository.GetAllMedicines();
        }

        // ✅ Get medicine by name
        public Medicine GetMedicineByName(string name)
        {
            return _medicineRepository.GetMedicineByName(name);
        }

        public void AddMedicine(Medicine medicine)
        {
            if (medicine == null)
            {
                throw new ArgumentNullException(nameof(medicine), "Medicine cannot be null.");
            }
            _medicineRepository.AddMedicine(medicine);
        }

        // ✅ Update existing medicine
        public void UpdateMedicine(Medicine medicine)
        {
            _medicineRepository.UpdateMedicine(medicine);
        }

        public void DeleteMedicine(string name)
        {
            _medicineRepository.DeleteMedicine(name);
        }

        public List<MedicineType> GetMedicineTypes()
        {
            return _medicineRepository.GetMedicineTypes();
        }


        
    }
}
