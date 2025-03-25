using Microsoft.EntityFrameworkCore;
using Swizom.Repository.IRepository;
using Swizom.Utility;
using Swizom.ViewDataModels;
using SwizomDbContext;
using SwizomDbContext.Models;

namespace Swizom.Repository
{
    public class MenuCategoryRepository : GenericRepository<MenuCategory>, IMenuCategoryRepository 
    {
        public MenuCategoryRepository(AppDbContext context, ExceptionHandler exceptionHandler) : base(context, exceptionHandler) { }

        public async Task<(IEnumerable<MenuCategoryDTO>, int)> GetAllMenuCategoriesAsync(int page, int pageSize)
        {
            var categories = from c in _context.MenuCategories
                          join r in _context.Restaurants on c.RestaurantID equals r.RestaurantID
                          select new MenuCategoryDTO
                          {
                              CategoryID = c.CategoryID,
                              Name = c.Name,
                              RestaurantName = r.Name
                          };

            var totalCategories = await categories.CountAsync();
            var paginatedCategories = await categories
                                        .Skip((page - 1) * pageSize)
                                        .Take(pageSize)
                                        .AsNoTracking()
                                        .ToListAsync();

            return (paginatedCategories, totalCategories);
        }
    }
}
