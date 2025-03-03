using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swizom.ViewDataModels;
using SwizomDbContext;
using SwizomDbContext.Models;

namespace Swizom.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MenuCategoryController : Controller
    {
        private readonly AppDbContext _context;

        public MenuCategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
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
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Restaurants = await _context.Restaurants.ToListAsync();
            return View();  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuCategory category)
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
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.MenuCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewBag.Restaurants = await _context.Restaurants.ToListAsync();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuCategory category)
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
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.MenuCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.MenuCategories.FindAsync(id);
            if (category != null)
            {
                _context.MenuCategories.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
