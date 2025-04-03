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

        public async Task<(IEnumerable<MenuCategoryDTO>, int)> GetAllMenuCategoriesAsync(string search, int page, int pageSize)
        {
            var categories = _context.MenuCategories
                              .Join(_context.Restaurants,
                                    c => c.RestaurantID,
                                    r => r.RestaurantID,
                                    (c, r) => new MenuCategoryDTO
                                    {
                                        CategoryID = c.CategoryID,
                                        Name = c.Name,
                                        RestaurantName = r.Name
                                    })
                              .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                categories = categories.Where(c => c.Name.ToLower().Contains(search) ||
                                                   c.RestaurantName.ToLower().Contains(search));
            }

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
