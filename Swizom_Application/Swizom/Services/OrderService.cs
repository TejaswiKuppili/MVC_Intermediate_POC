using Swizom.Repository;
using Swizom.Repository.IRepository;
using Swizom.Services.IServices;
using Swizom.ViewDataModels;
using SwizomDbContext.Models;

namespace Swizom.Services
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IGenericRepository<MenuItem> _menuItemRepository;

        public OrderService(IOrderRepository repository, IGenericRepository<MenuItem> menuItemRepository)
        {
            _repository = repository;
            _menuItemRepository = menuItemRepository;
        }

        public async Task<(IEnumerable<OrderDTO>, int)> GetOrdersAsync(string search, int page, int pageSize)
        {
            return await _repository.GetAllOrdersAsync(search, page, pageSize);
        }

        public async Task<OrderDTO> GetOrderAsync(int id)
        {
            var order = await _repository.GetOrderWithDetailsAsync(id);

            if (order == null)
                return null;

            return new OrderDTO
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
        }

        public async Task<IEnumerable<MenuItem>> GetMenuItemsAsync()
        {
            return await _menuItemRepository.GetAllAsync();
        }
        
        public async Task<bool> CreateOrderAsync(Order order, int[] ItemID, int[] Quantity)
        {
            order.OrderDate = DateTime.Now;
            order.Status = OrderStatus.Pending;
            order.TotalAmount = 0;
            order.OrderItems = new List<OrderItem>();

            for (int i = 0; i < ItemID.Length; i++)
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(ItemID[i]); // Fetch menu item
                if (menuItem != null)
                {
                    var orderItem = new OrderItem
                    {
                        ItemID = ItemID[i],
                        Quantity = Quantity[i],
                        Price = menuItem.Price // Use menuItem.Price
                    };
                    order.TotalAmount += orderItem.Quantity * orderItem.Price; // Correct total calculation
                    order.OrderItems.Add(orderItem);
                }
            }

            await _repository.AddAsync(order);
            return true;
        }

        public async Task<bool> UpdateOrderAsync(int id, OrderDTO orderDto)
        {
            var order = await _repository.GetOrderWithDetailsAsync(orderDto.OrderID);

            if (order == null)
                return false;

            // Update order details
            order.CustomerName = orderDto.CustomerName;
            order.CustomerPhone = orderDto.CustomerPhone;
            order.DeliveryAddress = orderDto.DeliveryAddress;
            order.TotalAmount = orderDto.TotalAmount;
            order.Status = orderDto.Status;

            await _repository.UpdateAsync(order);
            return true;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
