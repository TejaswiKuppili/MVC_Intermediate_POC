using Swizom.ViewDataModels;
using SwizomDbContext.Models;

namespace Swizom.Repository.IRepository
{
    public interface IMenuCategoryRepository : IGenericRepository<MenuCategory>
    {
        Task<(IEnumerable<MenuCategoryDTO>, int)> GetAllMenuCategoriesAsync(string search, int page, int pageSize);
    }
}
