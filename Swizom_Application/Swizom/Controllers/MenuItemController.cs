using Microsoft.AspNetCore.Mvc;
using SwizomDbContext.Models;
using SwizomDbContext;
using Microsoft.EntityFrameworkCore;
using Swizom.ViewDataModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;

namespace Swizom.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class MenuItemController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        public MenuItemController(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: MenuItem/Index
        public async Task<IActionResult> Index()
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
        }

        // GET: MenuItem/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.MenuCategories.ToListAsync();
            ViewBag.Restaurants = await _context.Restaurants.ToListAsync();
            return View();
        }

        // POST: MenuItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuItem menuItem)
        {
            var category = await _context.MenuCategories.FindAsync(menuItem.CategoryID);
            var restaurant = await _context.Restaurants.FindAsync(menuItem.RestaurantID);

            if (category is null && restaurant is null)
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
        }

        // GET: MenuItem/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            ViewBag.Categories = await _context.MenuCategories.ToListAsync();
            ViewBag.Restaurants = await _context.Restaurants.ToListAsync();
            return View(menuItem);
        }

        // POST: MenuItem/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuItem menuItem)
        {
            if (id != menuItem.ItemID)
            {
                return NotFound();
            }

            var category = await _context.MenuCategories.FindAsync(menuItem.CategoryID);
            var restaurant = await _context.Restaurants.FindAsync(menuItem.RestaurantID);

            if (category is null && restaurant is null)
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
        }

        // GET: MenuItem/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            return View(menuItem);
        }

        // POST: MenuItem/Delete/{id}
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem != null)
            {
                _context.MenuItems.Remove(menuItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
