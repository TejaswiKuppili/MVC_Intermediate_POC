using Microsoft.EntityFrameworkCore;
using Swizom.Repository.IRepository;
using Swizom.Utility;
using Swizom.ViewDataModels;
using SwizomDbContext;
using SwizomDbContext.Models;

namespace Swizom.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context, ExceptionHandler exceptionHandler) : base(context, exceptionHandler) { }

        public async Task<(IEnumerable<OrderDTO>, int)> GetAllOrdersAsync(int page, int pageSize)
        {
            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem);

            var totalOrders = await orders.CountAsync();

            var paginatedOrders = await orders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var orderDTOs = paginatedOrders.Select(o => new OrderDTO
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
            });

            return (orderDTOs, totalOrders);
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(o => o.OrderID == id);
        }
    }
}
