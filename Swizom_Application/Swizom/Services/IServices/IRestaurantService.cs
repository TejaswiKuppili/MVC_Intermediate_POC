using Swizom.ViewDataModels;
using SwizomDbContext.Models;

namespace Swizom.Services.IServices
{
    public interface IRestaurantService
    {
        Task<(IEnumerable<Restaurant>, int)> GetRestaurantsAsync(int page, int pageSize);
        Task<Restaurant?> GetRestaurantAsync(int id);
        Task<bool> CreateRestaurantAsync(Restaurant restaurant);
        Task<bool> UpdateRestaurantAsync(int id, Restaurant restaurant);
        Task<bool> DeleteRestaurantAsync(int id);
    }
}
