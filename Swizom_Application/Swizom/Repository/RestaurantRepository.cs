using Swizom.Repository.IRepository;
using Swizom.Utility;
using Swizom.ViewDataModels;
using SwizomDbContext.Models;
using SwizomDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models.Security;

namespace Swizom.Repository
{
    public class RestaurantRepository : GenericRepository<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(AppDbContext context, ExceptionHandler exceptionHandler) : base(context, exceptionHandler) { }

        public async Task<(IEnumerable<Restaurant>, int)> GetAllRestaurantsAsync(string search, int page, int pageSize)
        {
                var restaurants = _context.Restaurants.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();
                    restaurants = restaurants.Where(r => r.Name.ToLower().Contains(search) ||
                                             r.Address.ToLower().Contains(search) ||
                                             r.ContactNumber.Contains(search));
                }

                var totalRestaurants = await restaurants.CountAsync();
                var paginatedRestaurants = await restaurants
                                            .Skip((page - 1) * pageSize)
                                            .Take(pageSize)
                                            .AsNoTracking()
                                            .ToListAsync();

                return (paginatedRestaurants, totalRestaurants);
        }
    }
}
