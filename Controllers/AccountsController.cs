using Microsoft.AspNetCore.Mvc;

namespace CMSv2026WebApp.Controllers
{
    public class AccountsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
