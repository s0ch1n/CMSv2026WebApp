using CMSv2026WebApp.Services;
using CMSv2026WebApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CMSv2026WebApp.Controllers
{
    public class AdminController : Controller
    {
        //Fields
        private readonly IUserService _userService;

        //DI
        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        //GET
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult StaffList()
        {
            var staffList = _userService.GetAllStaffs();
            return View(staffList);
        }
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterNewStaff(UserRegistrationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
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
        // GET: Admin/EditStaff/13

        // Controller Action - Get Staff Details for Editing
        [HttpPost]
        public IActionResult ToggleUserStatus(int staffId, bool isActive)
        {
            Console.WriteLine($"Received staffId: {staffId}, isActive: {isActive}"); // Debugging
            var staff = _userService.GetStaffById(staffId);

            if (staff == null)
            {
                return Json(new { success = false, message = "Staff not found." });
            }

            staff.IsActive = isActive;
            _userService.UpdateStaff(staff);

            return Json(new { success = true, message = "Status updated successfully!" });
        }
        // Edit Staff Page
        [HttpGet]
        public IActionResult EditStaff(int staffId)
        {
            var staff = _userService.GetStaffById(staffId);
            if (staff == null)
            {
                return NotFound();
            }

            var viewModel = new UserRegistrationViewModel
            {
                Staff = staff,
                Roles = _userService.GetAllStaffRoles()
            };

            ViewBag.Roles = _userService.GetAllStaffRoles();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult UpdateStaff([FromBody] UserRegistrationViewModel viewModel)
        {
            if (viewModel == null || viewModel.Staff == null || viewModel.Staff.StaffId == 0)
            {
                return Json(new { success = false, message = "Invalid input data!" });
            }

            var existingStaff = _userService.GetStaffById(viewModel.Staff.StaffId);
            if (existingStaff == null)
            {
                return Json(new { success = false, message = "Staff not found!" });
            }

            // Update only the fields that are provided
            if (!string.IsNullOrEmpty(viewModel.Staff.FullName))
                existingStaff.FullName = viewModel.Staff.FullName;

            if (!string.IsNullOrEmpty(viewModel.Staff.Gender))
                existingStaff.Gender = viewModel.Staff.Gender;

            if (viewModel.Staff.DateOfBirth != default)
                existingStaff.DateOfBirth = viewModel.Staff.DateOfBirth;

            if (!string.IsNullOrEmpty(viewModel.Staff.MobileNumber))
                existingStaff.MobileNumber = viewModel.Staff.MobileNumber;

            if (viewModel.Staff.RoleId.HasValue)
                existingStaff.RoleId = viewModel.Staff.RoleId.Value;

            existingStaff.IsActive = viewModel.Staff.IsActive; // Always update IsActive

            var updatedStaff = _userService.UpdateStaff(existingStaff);
            if (updatedStaff != null)
            {
                return Json(new { success = true, message = "Staff details updated successfully!" });
            }

            return Json(new { success = false, message = "Failed to update staff details!" });
        }

        //[HttpPost("DeactivateStaff/{staffId}")]
        //public IActionResult DeactivateStaff(int staffId)
        //{
        //    bool result = _userService.DeactivateStaff(staffId);
        //    if (result)
        //    {
        //        return Ok("Staff member successfully deactivated.");
        //    }
        //    else
        //    {
        //        return BadRequest("Failed to deactivate staff.");
        //    }
        //}


    }
}