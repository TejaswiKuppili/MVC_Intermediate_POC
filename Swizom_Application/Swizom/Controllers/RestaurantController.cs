using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwizomDbContext;
using SwizomDbContext.Models;

namespace Swizom.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class RestaurantController : Controller
    {
        private readonly AppDbContext _context;

        public RestaurantController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            var restaurants = await _context.Restaurants.ToListAsync();
            var paginatedRestaurants = restaurants.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)restaurants.Count() / pageSize);
            return View(paginatedRestaurants);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return View(restaurant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Restaurant restaurant)
        {
            if (id != restaurant.RestaurantID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(restaurant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return View(restaurant);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurant = await _context.Restaurants
                .Include(r => r.MenuCategories).FirstOrDefaultAsync(r => r.RestaurantID == id);

            if (restaurant != null)
            {
                _context.MenuCategories.RemoveRange(restaurant.MenuCategories);
                _context.Restaurants.Remove(restaurant);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
