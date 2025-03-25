using Swizom.Repository.IRepository;
using Swizom.ViewDataModels;
using SwizomDbContext.Models;
using SwizomDbContext;
using Microsoft.EntityFrameworkCore;
using Swizom.Utility;
using Microsoft.AspNetCore.Diagnostics;

namespace Swizom.Repository
{
    public class MenuItemRepository : GenericRepository<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(AppDbContext context, ExceptionHandler exceptionHandler) : base(context, exceptionHandler) { }

        public async Task<(IEnumerable<MenuItemDTO>, int)> GetAllMenuItemsAsync(int page, int pageSize)
        {
            var menuItems = from m in _context.MenuItems
                              join c in _context.MenuCategories on m.CategoryID equals c.CategoryID
                              join r in _context.Restaurants on m.RestaurantID equals r.RestaurantID
                              select new MenuItemDTO
                              {
                                  ItemID = m.ItemID,
                                  Name = m.Name,
                                  Description = m.Description,
                                  Price = m.Price,
                                  CategoryName = c.Name,
                                  RestaurantName = r.Name
                              };
            var totalMenuItems = await menuItems.CountAsync();
            var paginatedMenuItems = await menuItems
                                        .Skip((page - 1) * pageSize)
                                        .Take(pageSize)
                                        .AsNoTracking()
                                        .ToListAsync();

            return (paginatedMenuItems, totalMenuItems);
        }
    }
}
