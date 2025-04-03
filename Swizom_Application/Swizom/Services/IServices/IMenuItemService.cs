using Swizom.ViewDataModels;
using SwizomDbContext.Models;

namespace Swizom.Services.IServices
{
    public interface IMenuItemService
    {
        Task<(IEnumerable<MenuItemDTO>, int)> GetMenuItemsAsync(string search, int page, int pageSize);
        Task<MenuItem?> GetMenuItemAsync(int id);
        Task<IEnumerable<MenuCategory>> GetCategoriesAsync();
        Task<IEnumerable<Restaurant>> GetRestaurantsAsync();
        Task<bool> CreateMenuItemAsync(MenuItem menuItem);
        Task<bool> UpdateMenuItemAsync(int id, MenuItem menuItem);
        Task<bool> DeleteMenuItemAsync(int id);
    }
}
