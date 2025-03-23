using Microsoft.AspNetCore.Mvc;
using SwizomDbContext.Models;
using SwizomDbContext;
using Microsoft.EntityFrameworkCore;
using Swizom.ViewDataModels;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Swizom.Utility;

namespace Swizom.Controllers
{
    [Authorize(Policy = "AdminAndEmployee")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private ExceptionHandler _exceptionHandler;

        public OrderController(AppDbContext context, ExceptionHandler exceptionHandler)
        {
            _context = context;
            _exceptionHandler = exceptionHandler;
        }

        // GET: Order/Index
        public async Task<IActionResult> Index()
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                var orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                    .ToListAsync();

                var groupedOrders = orders.Select(o => new OrderDTO
                {
                    OrderID = o.OrderID,
                    OrderDate = o.OrderDate,
                    CustomerName = o.CustomerName,
                    CustomerPhone = o.CustomerPhone,
                    DeliveryAddress = o.DeliveryAddress,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDTO
                    {
                        OrderItemID = oi.OrderItemID,
                        Quantity = oi.Quantity,
                        Price = oi.Price,
                        MenuItemName = oi.MenuItem.Name,
                        MenuItemPrice = oi.MenuItem.Price
                    }).ToList()
                }).ToList();

                return View(groupedOrders);
            }, "Orders");
        }

        // GET: Order/Create
        public async Task<IActionResult> Create()
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                ViewBag.MenuItems = await _context.MenuItems.ToListAsync();
                return View();
            }, "Create");
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order, int[] ItemID, int[] Quantity)
        {
            return await _exceptionHandler.HandleExceptionsAsync<IActionResult>(async () =>
            {
                if (ItemID.Length > 0)
                {
                    order.OrderDate = DateTime.Now;
                    order.Status = OrderStatus.Pending;
                    order.TotalAmount = 0;

                    for (int i = 0; i < ItemID.Length; i++)
                    {
                        var menuItem = await _context.MenuItems.FindAsync(ItemID[i]);
                        if (menuItem != null)
                        {
                            var orderItem = new OrderItem
                            {
                                ItemID = ItemID[i],
                                Quantity = Quantity[i],
                                Price = menuItem.Price
                            };
                            order.TotalAmount += orderItem.Total;
                            order.OrderItems.Add(orderItem);
                        }
                    }
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.MenuItems = await _context.MenuItems.ToListAsync();
                return View(order);
            }, "Create");
        }

        // GET: Order/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync<IActionResult>(async () =>
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                    .FirstOrDefaultAsync(o => o.OrderID == id);

                if (order == null)
                {
                    return NotFound();
                }

                var orderDto = new OrderDTO
                {
                    OrderID = order.OrderID,
                    OrderDate = order.OrderDate,
                    CustomerName = order.CustomerName,
                    CustomerPhone = order.CustomerPhone,
                    DeliveryAddress = order.DeliveryAddress,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    OrderItems = order.OrderItems.Select(oi => new OrderItemDTO
                    {
                        OrderItemID = oi.OrderItemID,
                        Quantity = oi.Quantity,
                        Price = oi.Price,
                        MenuItemName = oi.MenuItem.Name,
                        MenuItemPrice = oi.MenuItem.Price
                    }).ToList()
                };

                return View(orderDto);
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

                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.OrderID == id);

                if (order == null)
                {
                    return NotFound();
                }

                // Update order details from DTO
                order.CustomerName = orderDto.CustomerName;
                order.CustomerPhone = orderDto.CustomerPhone;
                order.DeliveryAddress = orderDto.DeliveryAddress;
                order.TotalAmount = orderDto.TotalAmount;
                order.Status = orderDto.Status;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }, "Edit");
        }


        // GET: Order/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync<IActionResult>(async () =>
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    return NotFound();
                }
                var orderDTO = new OrderDTO
                {
                    OrderID = order.OrderID,
                    OrderDate = order.OrderDate,
                    CustomerName = order.CustomerName,
                    CustomerPhone = order.CustomerPhone,
                    DeliveryAddress = order.DeliveryAddress,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    OrderItems = order.OrderItems.Select(oi => new OrderItemDTO
                    {
                        OrderItemID = oi.OrderItemID,
                        Quantity = oi.Quantity,
                        Price = oi.Price,
                        MenuItemName = oi.MenuItem.Name,
                        MenuItemPrice = oi.MenuItem.Price
                    }).ToList()
                };
                return View(orderDTO);
            }, "Error in Delete action");
        }

        // POST: Order/Delete/{id}
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                var order = await _context.Orders.FindAsync(id);
                if (order != null)
                {
                    _context.Orders.Remove(order);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }, "Error in DeleteConfirmed action");
        }
    }
}
