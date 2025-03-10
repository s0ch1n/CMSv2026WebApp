using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.Services
{
    public interface IMedicineStockService
    {
        IEnumerable<MedicineStock> GetAllStocks();
        void AddStock(MedicineStock stock);
        void UpdateStock(MedicineStock stock);
        void DeleteStock(int id);
    }
}
