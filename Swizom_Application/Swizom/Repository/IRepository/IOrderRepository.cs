using Swizom.ViewDataModels;
using SwizomDbContext.Models;

namespace Swizom.Repository.IRepository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<(IEnumerable<OrderDTO>, int)> GetAllOrdersAsync(int page, int pageSize);
        Task<Order?> GetOrderWithDetailsAsync(int id);
    }
}
