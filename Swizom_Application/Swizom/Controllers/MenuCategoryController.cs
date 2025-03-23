using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swizom.Utility;
using Swizom.ViewDataModels;
using SwizomDbContext;
using SwizomDbContext.Models;

namespace Swizom.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    //[Authorize(Policy = "AdminAndEmployee")]
    public class MenuCategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ExceptionHandler _exceptionHandler;

        public MenuCategoryController(AppDbContext context, ExceptionHandler exceptionHandler)
        {
            _context = context;
            _exceptionHandler = exceptionHandler;
        }

        public async Task<IActionResult> Index()
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                var categories = await (from m in _context.MenuCategories
                                        join r in _context.Restaurants on m.RestaurantID equals r.RestaurantID
                                        select new MenuCategoryDTO
                                        {
                                            CategoryID = m.CategoryID,
                                            Name = m.Name,
                                            RestaurantName = r.Name
                                        }).ToListAsync();
                return View(categories);
            }, "Error fetching menu categories.");
        }

        public async Task<IActionResult> Create()
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                ViewBag.Restaurants = await _context.Restaurants.ToListAsync();
                return View();
            }, "Error loading create menu category page.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuCategory category)
        {
            return await _exceptionHandler.HandleExceptionsAsync<IActionResult>(async () =>
            {
                var restaurant = await _context.Restaurants.FindAsync(category.RestaurantID);
                if (restaurant == null)
                {
                    ModelState.AddModelError("RestaurantID", "Selected restaurant does not exist.");
                    ViewBag.Restaurants = await _context.Restaurants.ToListAsync();
                    return View(category);
                }
                category.Restaurant = restaurant;
                _context.MenuCategories.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }, "Error creating menu category.");
        }

        public async Task<IActionResult> Edit(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync<IActionResult>(async () =>
            {
                var category = await _context.MenuCategories.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }
                ViewBag.Restaurants = await _context.Restaurants.ToListAsync();
                return View(category);
            }, "Error loading edit menu category page.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuCategory category)
        {
            return await _exceptionHandler.HandleExceptionsAsync<IActionResult>(async () =>
            {
                if (id != category.CategoryID)
                {
                    return NotFound();
                }
                var restaurant = await _context.Restaurants.FindAsync(category.RestaurantID);
                if (restaurant == null)
                {
                    ModelState.AddModelError("RestaurantID", "Selected restaurant does not exist.");
                    ViewBag.Restaurants = await _context.Restaurants.ToListAsync();
                    return View(category);
                }
                category.Restaurant = restaurant;
                _context.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }, "Error updating menu category.");
        }

        public async Task<IActionResult> Delete(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync<IActionResult>(async () =>
            {
                var category = await _context.MenuCategories.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }, "Error loading delete menu category page.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                var category = await _context.MenuCategories.FindAsync(id);
                if (category != null)
                {
                    _context.MenuCategories.Remove(category);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }, "Error deleting menu category.");
        }
    }
}
