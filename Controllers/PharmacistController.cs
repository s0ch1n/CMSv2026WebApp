using CMSv2026WebApp.Services;
using CMSv2026WebApp.Models;
using CMSv2026WebApp.ViewModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMSv2026WebApp.Controllers
{
    
    public class PharmacistController : Controller
    {
        private readonly IPharmacistService _pharmacistService;

        public PharmacistController(IPharmacistService pharmacistService)
        {
            _pharmacistService = pharmacistService;
        }

        public IActionResult Index()
        {
            return base.View();
        }

        [HttpGet]
        public IActionResult View()
        {
            var medicines = _pharmacistService.GetAllMedicines();

            // Convert Medicine to MedicineViewModel
            var medicineViewModels = medicines.Select(m => new MedicineViewModel
            {
                MedicineId = m.MedicineId,
                MedicineName = m.MedicineName,
                MedicineTypeId = m.MedicineTypeId,
                MedicineTypeName = m.MedicineTypeName,
                Unit = m.Unit,
                ExpiryDate = m.ExpiryDate
            }).ToList();

            //ViewBag.MedicineTypes = _pharmacistService.GetMedicineTypes();
            return View(medicineViewModels);
        }

       [HttpGet]
        public IActionResult Create()
        {
            var medicineTypes = _pharmacistService.GetMedicineTypes() ?? new List<MedicineType>(); // Prevent null

            ViewBag.MedicineTypes = new SelectList(medicineTypes, "MedicineTypeId", "MedicineTypeName");

            var model = new MedicineViewModel
            {
                MedicineTypes = medicineTypes.Select(mt => new SelectListItem
                {
                    Value = mt.MedicineTypeId.ToString(),
                    Text = mt.MedicineTypeName
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Adds security against CSRF attacks
        public IActionResult Create(MedicineViewModel model)
        {
            var medicineTypes = _pharmacistService.GetMedicineTypes() ?? new List<MedicineType>(); // Prevent null
            ViewBag.MedicineTypes = new SelectList(medicineTypes, "MedicineTypeId", "MedicineTypeName");

            if (ModelState.IsValid)
            {
                try
                {
                    Medicine newMedicine = new Medicine
                    {
                        MedicineName = model.MedicineName,
                        MedicineTypeId = model.MedicineTypeId,
                        ExpiryDate = model.ExpiryDate,
                        Unit = model.Unit,
                        IsActive = true
                    };

                    int medicineId = _pharmacistService.AddMedicine(newMedicine);
                    _pharmacistService.AddStock(medicineId, model.Unit);

                    TempData["ShowToast"] = true;
                    return RedirectToAction("Index");
                }
                catch (ApplicationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while adding the medicine. Please try again.");
                }
            }

            model.MedicineTypes = medicineTypes.Select(mt => new SelectListItem
            {
                Value = mt.MedicineTypeId.ToString(),
                Text = mt.MedicineTypeName
            }).ToList();

            return View(model);
        }



        [HttpGet]
        public IActionResult Edit(int id)
        {
            var medicine = _pharmacistService.GetMedicineById(id);
            if (medicine == null)
            {
                return NotFound();
            }
            ViewBag.MedicineTypes = _pharmacistService.GetMedicineTypes();
            return View(medicine);
        }

        [HttpPost]
        public IActionResult Edit(Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _pharmacistService.UpdateMedicine(medicine);
                    TempData["SuccessMessage"] = "Medicine updated successfully!";
                    TempData["ShowToast"] = true;
                    return RedirectToAction("Index");
                }
                catch (ApplicationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the medicine. Please try again.");
                }
            }
            ViewBag.MedicineTypes = _pharmacistService.GetMedicineTypes();
            return View(medicine);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _pharmacistService.DeleteMedicine(id);
            _pharmacistService.RemoveStock(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DispatchMedicine()
        {
            var prescriptions = _pharmacistService.GetTodaysPrescriptions();

            var dispatchViewModels = prescriptions.Select(p => new DispatchMedicineViewModel
            {
                PrescriptionId = p.PrescriptionId,
                MedicineId = p.MedicineId,
                MedicineName = p.MedicineName,
                AvailableStock = _pharmacistService.GetStockByMedicineId(p.MedicineId)?.StockInHand ?? 0,
                PatientName = p.Appointment?.Patient?.PatientName ?? "Unknown",
                DoctorName = p.Appointment?.Doctor?.Staff?.FullName ?? "Unknown",
                Dosage = p.Dosage,
                Frequency = p.Frequency,
                Duration = p.Duration
            }).ToList();

            return View(dispatchViewModels);
        }


        [HttpPost]
        public IActionResult DispatchMedicine(int prescriptionId, int medicineId, int dispatchQuantity)
        {
            try
            {
                var prescription = _pharmacistService.GetTodaysPrescriptionById(prescriptionId);
                if (prescription == null)
                {
                    TempData["Error"] = "Prescription not found!";
                    return RedirectToAction("DispatchMedicine");
                }

                var stock = _pharmacistService.GetStockByMedicineId(medicineId);
                if (stock == null || stock.StockInHand < dispatchQuantity)
                {
                    TempData["Error"] = "Insufficient stock!";
                    return RedirectToAction("DispatchMedicine");
                }

                _pharmacistService.ReduceStock(medicineId, dispatchQuantity);
                _pharmacistService.DispatchMedicine(medicineId, dispatchQuantity);
                TempData["Success"] = "Medicine dispatched successfully!";
            }
            catch (Exception)
            {
                TempData["Error"] = "An error occurred while dispatching the medicine.";
            }
            return RedirectToAction("DispatchMedicine");
        }

    }

}
