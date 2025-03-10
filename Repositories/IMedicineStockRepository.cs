using CMSv2026WebApp.Models;

namespace CMSv2026WebApp.Repositories
{
    public interface IMedicineStockRepository
    {
        IEnumerable<MedicineStock> GetAll();
        void Add(MedicineStock stock);
        void Update(MedicineStock stock);
        void Delete(int id);
    }
}
