using Swizom.ViewDataModels;
using SwizomDbContext.Models;

namespace Swizom.Services.IServices
{
    public interface IMenuCategoryService
    {
        Task<(IEnumerable<MenuCategoryDTO>, int)> GetMenuCategoriesAsync(int page, int pageSize);
        Task<MenuCategory?> GetMenuCategoryAsync(int id);
        Task<IEnumerable<Restaurant>> GetRestaurantsAsync();
        Task<bool> CreateMenuCategoryAsync(MenuCategory menuCategory);
        Task<bool> UpdateMenuCategoryAsync(int id, MenuCategory menuCategory);
        Task<bool> DeleteMenuCategoryAsync(int id);
    }
}
