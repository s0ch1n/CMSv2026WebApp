using System.Security.Claims;
using CMSv2026WebApp.Services;
using CMSv2026WebApp.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CMSv2026WebApp.Controllers
{
    public class AccountsController : Controller
    {
        //fields
        private readonly IUserService _userService;

        //Constructor Dependency Injection
        public AccountsController(IUserService userService)
        {
            _userService = userService;
        }

        //GET Accounts/Login
        [HttpGet]
        public IActionResult Login()
        {
            ViewData["PageTitle"] = "Login";
            return View(new LoginViewModel());
        }

        //POST Accounts/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginVModel)
        {
            // Validate
            if (ModelState.IsValid)
            {
                var availableUser = _userService.AuthenticateTheUser(loginVModel.UserName, loginVModel.Password);
                if (availableUser != null)
                {
                    // Stores in cookies
                    Response.Cookies.Append("StaffId", availableUser.StaffId.ToString(),
                        new CookieOptions { Expires = DateTime.Now.AddHours(1) });
                    Response.Cookies.Append("UserName", availableUser.UserName.ToString(),
                        new CookieOptions { Expires = DateTime.Now.AddHours(1) });
                    Response.Cookies.Append("RoleId", availableUser.RoleId?.ToString() ?? string.Empty,
                        new CookieOptions { Expires = DateTime.Now.AddHours(1) });

                    // Create claims
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, availableUser.StaffId.ToString()),
                new Claim(ClaimTypes.Name, availableUser.UserName),
                new Claim(ClaimTypes.Role, availableUser.RoleId?.ToString() ?? string.Empty)
            };

                    // Add the DoctorId claim if the user is a doctor
                    if (availableUser.RoleId == 2) // Assuming RoleId 2 is for doctors
                    {
                        var doctor = _userService.GetDoctorByStaffId(availableUser.StaffId);
                        if (doctor != null)
                        {
                            claims.Add(new Claim("DoctorId", doctor.DoctorId.ToString()));
                        }
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity)).Wait();

                    // Message
                    TempData["SuccessMessage"] = $"Welcome, {availableUser.UserName} !";

                    // Custom redirect
                    return RedirectToRoleBasedDashboard(availableUser.RoleId ?? 0);
                }

                TempData["ErrorMessage"] = "Invalid Username or Password!";
            }

            ViewData["PageTitle"] = "Login";
            return View(loginVModel);
        }

        public IActionResult Logout()
        {
            //Clear Cookies
            Response.Cookies.Delete("UserId");
            Response.Cookies.Delete("UserName");
            Response.Cookies.Delete("RoleId");
            //Message
            TempData["SuccessMessage"] = "You have been logged out successfully!";
            return RedirectToAction("Login", "Accounts");
        }

        //Custom Redirect to respective dashboard based on roleid
        private IActionResult RedirectToRoleBasedDashboard(int roleId)
        {
            switch (roleId)
            {
                case 1:
                    return RedirectToAction("Index", "Admin");
                case 2:
                    return RedirectToAction("Index", "Doctor");
                case 3:
                    return RedirectToAction("Index", "Receptionist");
                case 4:
                    return RedirectToAction("Index", "Pharmacist");
                case 5:
                    return RedirectToAction("Index", "LabTest");
                default:
                    return RedirectToAction("Login", "Accounts");
            }
        }
    }
}
