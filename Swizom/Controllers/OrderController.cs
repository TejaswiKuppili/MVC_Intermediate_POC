using Microsoft.AspNetCore.Mvc;
using SwizomDbContext.Models;
using SwizomDbContext;
using Microsoft.EntityFrameworkCore;
using Swizom.ViewDataModels;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace Swizom.Controllers
{
    [Authorize(Policy = "AdminAndEmployee")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Order/Index
        public async Task<IActionResult> Index()
        {
            var orders = await (from o in _context.Orders
                                join oi in _context.OrderItems on o.OrderID equals oi.OrderID
                                join m in _context.MenuItems on oi.ItemID equals m.ItemID
                                select new OrderDTO
                                {
                                    OrderID = o.OrderID,
                                    OrderDate = o.OrderDate,
                                    CustomerName = o.CustomerName,
                                    CustomerPhone = o.CustomerPhone,
                                    DeliveryAddress = o.DeliveryAddress,
                                    TotalAmount = o.TotalAmount,
                                    Status = o.Status,
                                    OrderItems = new List<OrderItemDTO>
                                    {
                                        new OrderItemDTO
                                        {
                                            OrderItemID = oi.OrderItemID,
                                            Quantity = oi.Quantity,
                                            Price = oi.Price,
                                            MenuItemName = m.Name,
                                            MenuItemPrice = m.Price
                                        }
                                    }
                                }).ToListAsync();

            return View(orders);
        }

        // GET: Order/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.MenuItems = await _context.MenuItems.ToListAsync();
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order, int[] ItemID, int[] Quantity)
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
        }

        // GET: Order/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Order/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Order/Delete/{id}
        public async Task<IActionResult> Delete(int id)
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
        }

        // POST: Order/Delete/{id}
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
