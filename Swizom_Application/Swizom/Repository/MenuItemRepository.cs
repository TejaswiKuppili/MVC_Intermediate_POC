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

        public async Task<(IEnumerable<MenuItemDTO>, int)> GetAllMenuItemsAsync(string search, int page, int pageSize)
        {
            var menuItems = _context.MenuItems
                .AsQueryable() // Enables dynamic filtering
                .Join(_context.MenuCategories,
                      m => m.CategoryID,
                      c => c.CategoryID,
                      (m, c) => new { m, c })
                .Join(_context.Restaurants,
                      mc => mc.m.RestaurantID,
                      r => r.RestaurantID,
                      (mc, r) => new MenuItemDTO
                      {
                          ItemID = mc.m.ItemID,
                          Name = mc.m.Name,
                          Description = mc.m.Description,
                          Price = mc.m.Price,
                          CategoryName = mc.c.Name,
                          RestaurantName = r.Name
                      })
                .AsQueryable(); // Allows further filtering

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                menuItems = menuItems.Where(m => m.Name.ToLower().Contains(search) ||
                                                 m.Description.ToLower().Contains(search) ||
                                                 m.CategoryName.ToLower().Contains(search) ||
                                                 m.RestaurantName.ToLower().Contains(search));
            }

            // Get the total count before pagination
            var totalMenuItems = await menuItems.CountAsync();

            // Apply pagination
            var paginatedMenuItems = await menuItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking() // Optimizes performance by disabling EF tracking
                .ToListAsync();

            return (paginatedMenuItems, totalMenuItems);
        }
    }
}
