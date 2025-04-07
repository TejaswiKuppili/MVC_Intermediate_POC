using Microsoft.AspNetCore.Mvc;
using SwizomDbContext.Models;
using SwizomDbContext;
using Microsoft.EntityFrameworkCore;
using Swizom.ViewDataModels;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Swizom.Utility;
using Swizom.Services.IServices;
using Microsoft.Extensions.Caching.Memory;

namespace Swizom.Controllers
{
    [Authorize(Policy = "AdminAndEmployee")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IOrderService _service;
        private ExceptionHandler _exceptionHandler;
        private readonly IMemoryCache _cache;

        public OrderController(AppDbContext context, ExceptionHandler exceptionHandler, IOrderService service, IMemoryCache cache)
        {
            _context = context;
            _exceptionHandler = exceptionHandler;
            _service = service;
            _cache = cache;
        }

        // GET: Order/Index
        public async Task<IActionResult> Index(string search = "", int page = 1, int pageSize = 4)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                string cacheKey = $"Order_{search}_{page}_{pageSize}";

                var (cachedData, _) = await CacheHelper.GetOrSetAsync(_cache, cacheKey, () => _service.GetOrdersAsync(search, page, pageSize));

                var (orders, totalCount) = cachedData;

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                ViewBag.SearchQuery = search; //Ensures the search value stays in the input field

                return View(orders);
            }, "Error fetching orders.");
        }

        // GET: Order/Create
        public async Task<IActionResult> Create()
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                ViewBag.MenuItems = await _service.GetMenuItemsAsync();
                return View();
            }, "Error loading create order page.");
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order, int[] ItemID, int[] Quantity)
        {
            return await _exceptionHandler.HandleExceptionsAsync<IActionResult>(async () =>
            {
                if(await _service.CreateOrderAsync(order, ItemID, Quantity))
                    return RedirectToAction(nameof(Index));
                return View(order);
            }, "Error creating order.");
        }

        // GET: Order/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync<IActionResult>(async () =>
            {
                var order = await _service.GetOrderAsync(id);

                if (order == null)
                {
                    return NotFound();
                }
                return View(order);
            }, "Error in Edit action");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderDTO orderDto)
        {
            return await _exceptionHandler.HandleExceptionsAsync<IActionResult>(async () =>
            {
                if (id != orderDto.OrderID)
                {
                    return NotFound();
                }

                if(await _service.UpdateOrderAsync(id, orderDto))
                    return RedirectToAction(nameof(Index));
                return View(orderDto);
            }, "Error updating order.");
        }

        // GET: Order/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync<IActionResult>(async () =>
            {
                var order = await _service.GetOrderAsync(id);
                if (order == null)
                {
                    return NotFound();
                }
                
                return View(order);
            }, "Error loading delete order page.");
        }

        // POST: Order/Delete/{id}
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                var order = await _service.DeleteOrderAsync(id);
                return RedirectToAction(nameof(Index));
            }, "Error deleting order.");
        }
    }
}
