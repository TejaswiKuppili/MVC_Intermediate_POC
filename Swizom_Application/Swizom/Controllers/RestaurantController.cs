using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swizom.Services.IServices;
using Swizom.Utility;
using SwizomDbContext;
using SwizomDbContext.Models;

namespace Swizom.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    //[Authorize(Policy = "AdminAndEmployee")]
    public class RestaurantController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IRestaurantService _service;
        private readonly ExceptionHandler _exceptionHandler;

        public RestaurantController(AppDbContext context, ExceptionHandler exceptionHandler, IRestaurantService service)
        {
            _context = context;
            _exceptionHandler = exceptionHandler;
            _service = service;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                var (restaurants, totalCount) = await _service.GetRestaurantsAsync(page, pageSize);

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                return View(restaurants);
            }, "Error in Index method");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Restaurant restaurant)
        {
            return await _exceptionHandler.HandleExceptionsAsync<IActionResult>(async () =>
            {
                if (await _service.CreateRestaurantAsync(restaurant))
                    return RedirectToAction(nameof(Index));
                return View(restaurant);
            }, "Error in Create method");
        }

        public async Task<IActionResult> Edit(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync<IActionResult>(async () =>
            {
                var restaurant = await _service.GetRestaurantAsync(id);
                if (restaurant == null)
                {
                    return NotFound();
                }
                return View(restaurant);
            }, "Error in Edit method");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Restaurant restaurant)
        {
            return await _exceptionHandler.HandleExceptionsAsync<IActionResult>(async () =>
            {
                if (id != restaurant.RestaurantID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await _service.UpdateRestaurantAsync(id, restaurant);
                    return RedirectToAction(nameof(Index));
                }
                return View(restaurant);
            }, "Error in Edit method");
        }

        public async Task<IActionResult> Delete(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync<IActionResult>(async () =>
            {
                var restaurant = await _service.GetRestaurantAsync(id);
                if (restaurant == null)
                {
                    return NotFound();
                }
                return View(restaurant);
            }, "Error in Delete method");
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                await _service.DeleteRestaurantAsync(id);
                return RedirectToAction(nameof(Index));
            }, "Error in DeleteConfirmed method");
        }
    }
}
