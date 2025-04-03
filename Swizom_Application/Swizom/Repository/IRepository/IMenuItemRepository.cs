using Swizom.ViewDataModels;
using SwizomDbContext.Models;

namespace Swizom.Repository.IRepository
{
    public interface IMenuItemRepository : IGenericRepository<MenuItem>
    {
        Task<(IEnumerable<MenuItemDTO>, int)> GetAllMenuItemsAsync(string search, int page, int pageSize);
    }
}
