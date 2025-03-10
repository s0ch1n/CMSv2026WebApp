using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CMSv2026WebApp.Models;
using CMSv2026WebApp.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMSv2026WebApp.Controllers
{
    public class PharmacistsController : Controller
    {
        private readonly IMedicineService _medicineService;

        public PharmacistsController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }
        //For Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }
        //for name during searching
        [HttpGet]
        public IActionResult SearchMedicine(string searchTerm)
        {
            var medicines = _medicineService.GetAllMedicines()
                            .Where(m => m.MedicineName.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))
                            .ToList();

            return Json(medicines); // ✅ Returns JSON for AJAX
        }

        public IActionResult Index(string searchName)
        {
            IEnumerable<Medicine> medicines;

            if (!string.IsNullOrWhiteSpace(searchName))
            {
                // Fetch medicines that start with the search term (case-insensitive)
                medicines = _medicineService.GetAllMedicines()
                            .Where(m => m.MedicineName.StartsWith(searchName, StringComparison.OrdinalIgnoreCase))
                            .ToList();
            }
            else
            {
                medicines = _medicineService.GetAllMedicines();
            }

            ViewData["SearchName"] = searchName; // Store search value for UI
            return View(medicines);
        }


        // ✅ View details of a medicine by name
        public IActionResult Details(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Medicine name is required.");
            }

            var medicine = _medicineService.GetMedicineByName(name);
            if (medicine == null)
            {
                return NotFound();
            }
            return View(medicine);
        }

        // ✅ GET: Medicine/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.MedicineTypes = _medicineService.GetMedicineTypes()
                .Select(mt => new SelectListItem
                {
                    Value = mt.MedicineTypeId.ToString(),
                    Text = mt.MedicineTypeName
                }).ToList();

            return View();
        }


        // ✅ POST: Medicine/Create
        [HttpPost]
        public IActionResult Create(Medicine medicine)
        {
            if (!ModelState.IsValid)  // ✅ Ensures form validation
            {
                _medicineService.AddMedicine(medicine);
                TempData["SuccessMessage"] = "Medicine added successfully!";
                return RedirectToAction("Index");  // ✅ Redirects to the medicine list
            }

            ViewBag.MedicineTypes = _medicineService.GetMedicineTypes()
                .Select(mt => new SelectListItem
                {
                    Value = mt.MedicineTypeId.ToString(),
                    Text = mt.MedicineTypeName
                }).ToList();

            return View(medicine);  // ✅ Reloads the form if validation fails
        }


        // ✅ Show form to edit a medicine
        public IActionResult Edit(string name)
        {
            var medicine = _medicineService.GetMedicineByName(name);
            if (medicine == null)
            {
                return NotFound();
            }
            return View(medicine);
        }

        // ✅ Update existing medicine (POST)
        [HttpPost]
        public IActionResult Edit(Medicine medicine)
        {
            if (!ModelState.IsValid)
            {
                _medicineService.UpdateMedicine(medicine);
                return RedirectToAction("Index");
            }
            return View(medicine);
        }

        // ✅ Delete medicine by name (POST)
        [HttpPost]
        public IActionResult Delete(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            var medicine = _medicineService.GetMedicineByName(name);
            if (medicine == null)
            {
                return NotFound();
            }

            _medicineService.DeleteMedicine(name);
            return RedirectToAction("Index");
        }

        
    }
}
