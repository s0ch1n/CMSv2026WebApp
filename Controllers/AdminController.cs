using CMSv2026WebApp.Services;
using CMSv2026WebApp.ViewModel;
using Microsoft.AspNetCore.Mvc;

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
            if (!IsUserInRole(1))
            {
                return RedirectToAction("Login", "Accounts");
            }
            var viewModel = new UserRegistrationViewModel
            {
                Staffs = _userService.GetAllStaffs(),
                Roles = _userService.GetAllStaffRoles()

            };
            ViewData["PageTitle"] = "Admin";
            ViewBag.Role = "Admin";
            ViewBag.Roles = viewModel.Roles;
            //return Content("Admin Dashboard)
            return View(viewModel);
        }

        //To Check RoleId
        private bool IsUserInRole(int requiredRoleId)
        {
            //Get the roleId from cookies
            var roleId = Request.Cookies["RoleId"];

            //If not match --> Redirect to Login

            return roleId != null
                && int.TryParse(roleId, out int userRoleId)
                && userRoleId == requiredRoleId;
        }

        //POST: /Admin/RegisterUser
        public IActionResult RegisterNewUser(UserRegistrationViewModel viewModel)
        {
            if (!IsUserInRole(1))
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (ModelState.IsValid)
            {
                _userService.InsertStaff(viewModel.Staff);
                TempData["SuccessMessage"] = $"Staff '{viewModel.Staff.UserName}' registered successfully";

                return RedirectToAction("Index");
            }

            viewModel.Staffs = _userService.GetAllStaffs();
            viewModel.Roles = _userService.GetAllStaffRoles();
            ViewBag.Roles = viewModel.Roles;
            TempData["ErrorMessage"] = "Failed to register user. Please check the form";
            return View("Index", viewModel);
        }

        //POST: /Admin/IsActive
        [HttpPost]
        public IActionResult ToggleUserStatus(int userId, bool isActive)
        {
            try
            {
                _userService.UpdateStaffStatus(userId, isActive);
                return Json(new { success = true, message = "Staff status updated successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }
}
