using Swizom.Repository.IRepository;
using Swizom.ViewDataModels;
using SwizomDbContext.Models;
using SwizomDbContext;
using Microsoft.EntityFrameworkCore;

namespace Swizom.Repository
{
    public class MenuItemRepository : GenericRepository<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<MenuItemDTO>> GetAllMenuItemsAsync()
        {
            return await (from m in _context.MenuItems
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
                          }).AsNoTracking().ToListAsync();
        }
    }
}
