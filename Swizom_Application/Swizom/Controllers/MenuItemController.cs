using Microsoft.AspNetCore.Mvc;
using SwizomDbContext.Models;
using SwizomDbContext;
using Microsoft.EntityFrameworkCore;
using Swizom.ViewDataModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using Swizom.Services;

namespace Swizom.Controllers
{
    //[Authorize(Policy = "AdminOnly")]
    [Authorize(Policy = "AdminAndEmployee")]
    public class MenuItemController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly ExceptionHandler _exceptionHandler;

        public MenuItemController(AppDbContext context, IMemoryCache cache, ExceptionHandler exceptionHandler)
        {
            _context = context;
            _cache = cache;
            _exceptionHandler = exceptionHandler;
        }

        // GET: MenuItem/Index
        public async Task<IActionResult> Index()
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                var menuItems = await (from m in _context.MenuItems
                                       join c in _context.MenuCategories on m.CategoryID equals c.CategoryID
                                       join r in _context.Restaurants on m.RestaurantID equals r.RestaurantID
                                       select new MenuItemDTO
                                       {
                                           ItemID = m.ItemID,
                                           Name = m.Name,
                                           Description = m.Description,
                                           Price = m.Price,
                                           CategoryName = c.Name,
                                           RestaurantName = r.Name
                                       }).ToListAsync();
                return View(menuItems);
            }, "Error in Index method");
        }

        // GET: MenuItem/Create
        public async Task<IActionResult> Create()
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                ViewBag.Categories = await _context.MenuCategories.ToListAsync();
                ViewBag.Restaurants = await _context.Restaurants.ToListAsync();
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
                var category = await _context.MenuCategories.FindAsync(menuItem.CategoryID);
                var restaurant = await _context.Restaurants.FindAsync(menuItem.RestaurantID);

                if (category is null || restaurant is null)
                {
                    ModelState.AddModelError("CategoryID", "Selected category does not exist.");
                    ModelState.AddModelError("RestaurantID", "Selected restaurant does not exist.");
                    ViewBag.Categories = await _context.MenuCategories.ToListAsync();
                    ViewBag.Restaurants = await _context.Restaurants.ToListAsync();
                    return View(menuItem);
                }

                menuItem.Category = category;
                menuItem.Restaurant = restaurant;
                _context.MenuItems.Add(menuItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }, "Error in Create POST method");
        }

        // GET: MenuItem/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                var menuItem = await _context.MenuItems.FindAsync(id);
                if (menuItem == null)
                {
                    return NotFound();
                }
                ViewBag.Categories = await _context.MenuCategories.ToListAsync();
                ViewBag.Restaurants = await _context.Restaurants.ToListAsync();
                return View(menuItem);
            }, "Error in Edit method");
        }

        // POST: MenuItem/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuItem menuItem)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                if (id != menuItem.ItemID)
                {
                    return NotFound();
                }

                var category = await _context.MenuCategories.FindAsync(menuItem.CategoryID);
                var restaurant = await _context.Restaurants.FindAsync(menuItem.RestaurantID);

                if (category is null || restaurant is null)
                {
                    ModelState.AddModelError("CategoryID", "Selected category does not exist.");
                    ModelState.AddModelError("RestaurantID", "Selected restaurant does not exist.");
                    ViewBag.Categories = await _context.MenuCategories.ToListAsync();
                    ViewBag.Restaurants = await _context.Restaurants.ToListAsync();
                    return View(menuItem);
                }

                menuItem.Category = category;
                menuItem.Restaurant = restaurant;
                _context.Update(menuItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }, "Error in Edit POST method");
        }

        // GET: MenuItem/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                var menuItem = await _context.MenuItems.FindAsync(id);
                if (menuItem == null)
                {
                    return NotFound();
                }
                return View(menuItem);
            }, "Error in Delete method");
        }

        // POST: MenuItem/Delete/{id}
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                var menuItem = await _context.MenuItems.FindAsync(id);
                if (menuItem != null)
                {
                    _context.MenuItems.Remove(menuItem);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }, "Error in DeleteConfirmed method");
        }
    }
}
