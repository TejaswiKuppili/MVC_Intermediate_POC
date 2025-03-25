using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swizom.Repository.IRepository;
using Swizom.Services.IServices;
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
        private readonly IMenuCategoryService _service;
        private readonly ExceptionHandler _exceptionHandler;

        public MenuCategoryController(AppDbContext context, ExceptionHandler exceptionHandler, IMenuCategoryService service)
        {
            _context = context;
            _exceptionHandler = exceptionHandler;
            _service = service;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                var (categories, totalCount) = await _service.GetMenuCategoriesAsync(page, pageSize);

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                return View(categories);
            }, "Error fetching menu categories.");
        }

        public async Task<IActionResult> Create()
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                ViewBag.Restaurants = await _service.GetRestaurantsAsync();
                return View();
            }, "Error loading create menu category page.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuCategory category)
        {
            return await _exceptionHandler.HandleExceptionsAsync<IActionResult>(async () =>
            {
                if(await _service.CreateMenuCategoryAsync(category))
                    return RedirectToAction(nameof(Index));
                 return View(category);
            }, "Error creating menu category.");
        }

        public async Task<IActionResult> Edit(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync<IActionResult>(async () =>
            {
                var category = await _service.GetMenuCategoryAsync(id);

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
                if (await _service.UpdateMenuCategoryAsync(id, category))
                    return RedirectToAction(nameof(Index));
                 return View(category);
            }, "Error updating menu category.");
        }

        public async Task<IActionResult> Delete(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync<IActionResult>(async () =>
            {
                var category = await _service.GetMenuCategoryAsync(id);
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
                await _service.DeleteMenuCategoryAsync(id);
                return RedirectToAction(nameof(Index));
            }, "Error deleting menu category.");
        }
    }
}
