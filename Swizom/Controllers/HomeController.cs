using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swizom.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Swizom.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == "Employee")
            {
                return RedirectToAction("Index", "Order");
            }
            return View();
        }

        public IActionResult Users()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            // If the user does not have a specific role, redirect to Access Denied
            if (string.IsNullOrEmpty(userRole) || userRole != "Admin")
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var userInfo = new
            {
                Name = User.Identity.Name,
                Email = User.FindFirst(ClaimTypes.Email)?.Value,
                ObjectId = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value,
                Roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList()
            };

            return View(userInfo);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
