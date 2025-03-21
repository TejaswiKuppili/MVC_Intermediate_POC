using Microsoft.AspNetCore.Mvc;
using SwizomDbContext.Models;
using SwizomDbContext;
using Microsoft.EntityFrameworkCore;
using Swizom.ViewDataModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using Swizom.Utility;
using Swizom.Services.IServices;

namespace Swizom.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    //[Authorize(Policy = "AdminAndEmployee")]
    public class MenuItemController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMenuItemService _service;
        private readonly IMemoryCache _cache;
        private readonly ExceptionHandler _exceptionHandler;

        public MenuItemController(AppDbContext context, IMemoryCache cache, IMenuItemService service, ExceptionHandler exceptionHandler)
        {
            _context = context;
            _cache = cache;
            _service = service;
            _exceptionHandler = exceptionHandler;
        }

        // GET: MenuItem/Index
        public async Task<IActionResult> Index()
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () => View(await _service.GetMenuItemsAsync()), "Error in Index method");
        }

        // GET: MenuItem/Create
        public async Task<IActionResult> Create()
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                ViewBag.Categories = await _service.GetCategoriesAsync();
                ViewBag.Restaurants = await _service.GetRestaurantsAsync();
                return View();
            }, "Error in Create method");
        }

        // POST: MenuItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuItem menuItem)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                if (await _service.CreateMenuItemAsync(menuItem))
                    return RedirectToAction(nameof(Index));
                return View(menuItem);
            }, "Error in Create method");
        }

        // GET: MenuItem/Edit
        public async Task<IActionResult> Edit(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                var menuItem = await _service.GetMenuItemAsync(id);

                if (menuItem == null)
                {
                    return NotFound();
                }

                ViewBag.Categories = await _service.GetCategoriesAsync();
                ViewBag.Restaurants = await _service.GetRestaurantsAsync();

                return View(menuItem);
            }, "Error in Edit method");
        }

        // POST: MenuItem/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuItem menuItem)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                if (await _service.UpdateMenuItemAsync(id, menuItem))
                    return RedirectToAction(nameof(Index));
                return View(menuItem);
            }, "Error in Edit method");
        }

        // GET: MenuItem/Delete
        public async Task<IActionResult> Delete(int id)
        {
            var menuItem = await _service.GetMenuItemAsync(id);
            if (menuItem == null) return NotFound();
            return View(menuItem);
        }

        // POST: MenuItem/Delete
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                await _service.DeleteMenuItemAsync(id);
                return RedirectToAction(nameof(Index));
            }, "Error in Delete method");
        }
    }
}