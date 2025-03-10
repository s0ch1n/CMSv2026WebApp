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
            return View();
        }

        public IActionResult StaffManagement()
        {
            var viewModel = new UserRegistrationViewModel
            {
                Staffs = _userService.GetAllStaffs(),
                Roles = _userService.GetAllStaffRoles()
            };
            ViewData["PageTitle"] = "Admin";
            ViewBag.Role = "Admin";
            ViewBag.Roles = viewModel.Roles;
            return View(viewModel);
        }
        [HttpPost("{staffId}")]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterNewStaff(UserRegistrationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _userService.InsertStaff(viewModel.Staff);
                TempData["SuccessMessage"] = $"Staff '{viewModel.Staff.UserName}' registered successfully";
                return RedirectToAction("StaffManagement");
            }

            // Log model state errors
            foreach (var modelStateKey in ModelState.Keys)
            {
                var modelStateVal = ModelState[modelStateKey];
                foreach (var error in modelStateVal.Errors)
                {
                    var errorMessage = error.ErrorMessage;
                    // You can log this to a file or the console
                    Console.WriteLine($"ModelState Error: {modelStateKey} - {errorMessage}");
                }
            }

            viewModel.Staffs = _userService.GetAllStaffs();
            viewModel.Roles = _userService.GetAllStaffRoles();
            ViewBag.Roles = viewModel.Roles;
            TempData["ErrorMessage"] = "Failed to register user. Please check the form";
            return View("StaffManagement", viewModel);
        }
        [HttpGet("{staffId}")]
        public IActionResult EditStaff(int staffId)
{
    // Retrieve staff data from the database using your service
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

    return View(viewModel);
}

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult EditStaff(UserRegistrationViewModel viewModel)
{
    if (ModelState.IsValid)
    {
        var updatedStaff = _userService.UpdateStaff(viewModel.Staff);
        TempData["SuccessMessage"] = "Staff details updated successfully!";
        return RedirectToAction("StaffManagement");
    }

    // Log errors and return to the view if validation fails
    foreach (var modelStateKey in ModelState.Keys)
    {
        var modelStateVal = ModelState[modelStateKey];
        foreach (var error in modelStateVal.Errors)
        {
            Console.WriteLine($"ModelState Error: {modelStateKey} - {error.ErrorMessage}");
        }
    }

    viewModel.Roles = _userService.GetAllStaffRoles();
    return View("EditStaff", viewModel);
}

    }
    }


