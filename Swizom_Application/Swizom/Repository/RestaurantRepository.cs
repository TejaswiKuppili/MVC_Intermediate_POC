using Swizom.Repository.IRepository;
using Swizom.Utility;
using Swizom.ViewDataModels;
using SwizomDbContext.Models;
using SwizomDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Swizom.Repository
{
    public class RestaurantRepository : GenericRepository<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(AppDbContext context, ExceptionHandler exceptionHandler) : base(context, exceptionHandler) { }

        public async Task<(IEnumerable<Restaurant>, int)> GetAllRestaurantsAsync(int page, int pageSize)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                var restaurants = _context.Restaurants.Select(r => new Restaurant
                {
                    RestaurantID = r.RestaurantID,
                    Name = r.Name,
                    Address = r.Address,
                    ContactNumber = r.ContactNumber
                });
                
                var totalRestaurants = await restaurants.CountAsync();
                var paginatedRestaurants = await restaurants
                                            .Skip((page - 1) * pageSize)
                                            .Take(pageSize)
                                            .AsNoTracking()
                                            .ToListAsync();

                return (paginatedRestaurants, totalRestaurants);
            }, "Index");
        }
    }
}
