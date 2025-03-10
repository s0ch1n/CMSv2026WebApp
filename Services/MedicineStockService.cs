using CMSv2026WebApp.Models;
using CMSv2026WebApp.Repositories;

namespace CMSv2026WebApp.Services
{
    public class MedicineStockService:IMedicineStockService
    {
        private readonly IMedicineStockRepository _repository;

        public MedicineStockService(IMedicineStockRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<MedicineStock> GetAllStocks() => _repository.GetAll();

        public void AddStock(MedicineStock stock) => _repository.Add(stock);

        public void UpdateStock(MedicineStock stock) => _repository.Update(stock);

        public void DeleteStock(int id) => _repository.Delete(id);
    }
}
    

