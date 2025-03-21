using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swizom.Utility;
using SwizomDbContext;
using SwizomDbContext.Models;

namespace Swizom.Controllers
{
    [Authorize(Policy = "Adminonly")]
    //[Authorize(Policy = "AdminAndEmployee")]
    public class RestaurantController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ExceptionHandler _exceptionHandler;

        public RestaurantController(AppDbContext context, ExceptionHandler exceptionHandler)
        {
            _context = context;
            _exceptionHandler = exceptionHandler;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                var restaurants = await _context.Restaurants.ToListAsync();

                if (restaurants == null)
                {
                    return StatusCode(500, "Failed to retrieve restaurants.");
                }

                var paginatedRestaurants = restaurants.Skip((page - 1) * pageSize).Take(pageSize);

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)restaurants.Count() / pageSize);

                return View(paginatedRestaurants);
            }, "Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Restaurant restaurant)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                if (ModelState.IsValid)
                {
                    _context.Add(restaurant);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(restaurant);
            }, "Create");
        }
        public async Task<IActionResult> Edit(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                var restaurant = await _context.Restaurants.FindAsync(id);
                if (restaurant == null)
                {
                    return NotFound();
                }
                return View(restaurant);
            }, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Restaurant restaurant)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
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
            }, "Edit");
        }

        public async Task<IActionResult> Delete(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                var restaurant = await _context.Restaurants.FindAsync(id);
                if (restaurant == null)
                {
                    return NotFound();
                }
                return View(restaurant);
            }, "Delete");
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
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
            }, "DeleteConfirmed");
        }
    }
}
