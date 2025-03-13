using CMSv2026WebApp.Services;
using CMSv2026WebApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CMSv2026WebApp.Controllers
{
    public class AdminController : Controller
    {
        // Fields
        private readonly IUserService _userService;

        // DI
        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: Admin/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: Admin/StaffList
        public IActionResult StaffList()
        {
            var staffList = _userService.GetAllStaffs();
            return View(staffList);
        }

        // GET: Admin/StaffManagement
        public IActionResult StaffManagement()
        {
            var viewModel = new UserRegistrationViewModel
            {
                Staffs = _userService.GetAllStaffs(),
                Roles = _userService.GetAllStaffRoles(),
                Specializations = _userService.GetAllSpecializations()
            };
            ViewData["PageTitle"] = "Admin";
            ViewBag.Role = "Admin";
            ViewBag.Roles = viewModel.Roles;
            ViewBag.Specializations = viewModel.Specializations;
            return View(viewModel);
        }

        // POST: Admin/RegisterNewStaff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterNewStaff(UserRegistrationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Roles = _userService.GetAllStaffRoles();
                viewModel.Specializations = _userService.GetAllSpecializations();
                ViewBag.Roles = viewModel.Roles;
                ViewBag.Specializations = viewModel.Specializations;
                return View("StaffManagement", viewModel); // Return view with validation errors
            }

            try
            {
                _userService.InsertStaff(viewModel.Staff);
                TempData["SuccessMessage"] = "Staff registered successfully!";
                return RedirectToAction("StaffManagement");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error registering staff: " + ex.Message;
                return RedirectToAction("StaffManagement");
            }
        }

        // POST: Admin/ToggleUserStatus
        [HttpPost]
        public IActionResult ToggleUserStatus(int staffId, bool isActive)
        {
            var staff = _userService.GetStaffById(staffId);

            if (staff == null)
            {
                return Json(new { success = false, message = "Staff not found." });
            }

            staff.IsActive = isActive;
            _userService.UpdateStaff(staff);

            return Json(new { success = true, message = "Status updated successfully!" });
        }

        // GET: Admin/EditStaff
        [HttpGet]
        public IActionResult EditStaff(int staffId)
        {
            try
            {
                var staff = _userService.GetStaffById(staffId);
                if (staff == null)
                {
                    TempData["ErrorMessage"] = "Staff not found!";
                    return RedirectToAction("StaffManagement");
                }

                var viewModel = new UserRegistrationViewModel
                {
                    Staff = staff,
                    Roles = _userService.GetAllStaffRoles(),
                    Specializations = _userService.GetAllSpecializations()
                };

                ViewBag.Roles = viewModel.Roles;
                ViewBag.Specializations = viewModel.Specializations;
                return Json(new { staff = viewModel.Staff });
            }
            catch (Exception ex)
            {
                // Use logging instead of Console.WriteLine
                Console.WriteLine($"Error in EditStaff action: {ex.Message}");
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                return RedirectToAction("StaffManagement");
            }
        }

        // POST: Admin/UpdateStaff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStaff([FromBody] UserRegistrationViewModel viewModel)
        {
            if (viewModel == null || viewModel.Staff == null || viewModel.Staff.StaffId == 0)
            {
                return Json(new { success = false, message = "Invalid staff details provided!" });
            }

            var existingStaff = _userService.GetStaffById(viewModel.Staff.StaffId);
            if (existingStaff == null)
            {
                return Json(new { success = false, message = "Staff not found!" });
            }

            // Update fields safely
            existingStaff.FullName = !string.IsNullOrEmpty(viewModel.Staff.FullName) ? viewModel.Staff.FullName : existingStaff.FullName;
            existingStaff.Gender = !string.IsNullOrEmpty(viewModel.Staff.Gender) ? viewModel.Staff.Gender : existingStaff.Gender;
            existingStaff.DateOfBirth = viewModel.Staff.DateOfBirth != default ? viewModel.Staff.DateOfBirth : existingStaff.DateOfBirth;
            existingStaff.DateOfJoining = viewModel.Staff.DateOfJoining != default ? viewModel.Staff.DateOfJoining : existingStaff.DateOfJoining;
            existingStaff.MobileNumber = !string.IsNullOrEmpty(viewModel.Staff.MobileNumber) ? viewModel.Staff.MobileNumber : existingStaff.MobileNumber;
            existingStaff.UserName = !string.IsNullOrEmpty(viewModel.Staff.UserName) ? viewModel.Staff.UserName : existingStaff.UserName;
            existingStaff.Password = !string.IsNullOrEmpty(viewModel.Staff.Password) ? viewModel.Staff.Password : existingStaff.Password;
            existingStaff.RoleId = viewModel.Staff.RoleId ?? existingStaff.RoleId;
            existingStaff.EmailAddress = !string.IsNullOrEmpty(viewModel.Staff.EmailAddress) ? viewModel.Staff.EmailAddress : existingStaff.EmailAddress;
            existingStaff.Qualification = !string.IsNullOrEmpty(viewModel.Staff.Qualification) ? viewModel.Staff.Qualification : existingStaff.Qualification;
            existingStaff.IsActive = viewModel.Staff.IsActive;

            if (viewModel.Staff.RoleId == 2)
            {
                existingStaff.Doctor.SpecializationId = viewModel.Staff.Doctor.SpecializationId;
                existingStaff.Doctor.ConsultationFee = viewModel.Staff.Doctor.ConsultationFee;
            }

            try
            {
                _userService.UpdateStaff(existingStaff);
                return Json(new { success = true, message = "Staff details updated successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error updating staff: " + ex.Message });
            }
        }
    }
}
