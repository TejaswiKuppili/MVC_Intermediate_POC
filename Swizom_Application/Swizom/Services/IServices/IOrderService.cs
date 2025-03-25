using Swizom.ViewDataModels;
using SwizomDbContext.Models;

namespace Swizom.Services.IServices
{
    public interface IOrderService
    {
        Task<(IEnumerable<OrderDTO>, int)> GetOrdersAsync(int page, int pageSize);
        Task<OrderDTO> GetOrderAsync(int id);
        Task<IEnumerable<MenuItem>> GetMenuItemsAsync();
        Task<bool> CreateOrderAsync(Order order, int[] ItemID, int[] Quantity);
        Task<bool> UpdateOrderAsync(int id, OrderDTO orderDto);
        Task<bool> DeleteOrderAsync(int id);
    }
}
