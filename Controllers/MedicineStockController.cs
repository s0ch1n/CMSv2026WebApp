using CMSv2026WebApp.Models;
using CMSv2026WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMSv2026WebApp.Controllers
{
    public class MedicineStockController : Controller
    {
        private readonly IMedicineStockService _service;

        public MedicineStockController(IMedicineStockService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var stocks = _service.GetAllStocks();
            return View(stocks);
        }

        // GET method to load the form
        [HttpGet]
        public IActionResult AddMedicineStock()
        {
            return View();
        }

        // POST method to handle form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddMedicineStock(MedicineStock model)
        {
            if (!ModelState.IsValid)
            {
                _service.AddStock(model); // Ensure this saves to DB
                return RedirectToAction("Index"); // Redirect to list view
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var stock = _service.GetAllStocks().FirstOrDefault(s => s.MedicineStockId == id);
            return View(stock);
        }

        [HttpPost]
        public IActionResult Edit(MedicineStock stock)
        {
            if (!ModelState.IsValid)
            {
                _service.UpdateStock(stock);
                return RedirectToAction("Index");
            }
            return View(stock);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _service.DeleteStock(id);
            return RedirectToAction("Index");
        }
    }
}
    

