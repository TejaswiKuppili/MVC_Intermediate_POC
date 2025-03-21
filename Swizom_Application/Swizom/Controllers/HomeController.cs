using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Swizom.Models;
using Swizom.Utility;
using System.Diagnostics;
using System.Security.Claims;

namespace Swizom.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ExceptionHandler _exceptionHandler;

        public HomeController(ILogger<HomeController> logger, ExceptionHandler exceptionHandler)
        {
            _logger = logger;
            _exceptionHandler = exceptionHandler;
        }

        public async Task<IActionResult> Index()
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userRole == "Employee")
                {
                    return RedirectToAction("Index", "Order");
                }
                return View();
            }, "Index");
        }

        public async Task<IActionResult> Users()
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // If the user does not have a specific role, redirect to Access Denied
                if (string.IsNullOrEmpty(userRole) || userRole != "Admin")
                {
                    return RedirectToAction("AccessDenied", "Account");
                }

                var userInfo = new
                {
                    Name = User.Identity?.Name ?? "Unknown",
                    Email = User.FindFirst(ClaimTypes.Email)?.Value ?? "Not Available",
                    ObjectId = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value ?? "Not Available",
                    Roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList()
                };

                return View(userInfo);
            }, "Users");
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
