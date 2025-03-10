using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.Services
{
    public interface IMedicineService
    {
        IEnumerable<Medicine> GetAllMedicines();
        Medicine GetMedicineByName(string name);
        void AddMedicine(Medicine medicine);
        void UpdateMedicine(Medicine medicine);
        void DeleteMedicine(string name);
        List<MedicineType> GetMedicineTypes();

       
    }
}

