using CMSv2026WebApp.Models;
using CMSv2026WebApp.ViewModel;

namespace CMSv2026WebApp.Services
{
    public interface IPharmacistService
    {
        List<Medicine> GetAllMedicines();
        List<MedicineType> GetMedicineTypes();
        Medicine GetMedicineById(int id);
        int AddMedicine(Medicine medicine);
        void UpdateMedicine(Medicine medicine);
        void DeleteMedicine(int id);
        MedicineStock GetStockByMedicineId(int medicineId);
        void AddStock(int medicineId, int quantity);
        void ReduceStock(int medicineId, int quantity);
        void RemoveStock(int medicineId);
        MedicinePrescription GetTodaysPrescriptionById(int prescriptionId);
        void DispatchMedicine(int medicineId, int quantity);
        List<DispatchMedicineViewModel> GetTodaysPrescriptions();
    }
}
