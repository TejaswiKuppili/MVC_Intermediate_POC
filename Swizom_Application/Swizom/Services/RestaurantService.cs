using Swizom.Repository.IRepository;
using Swizom.Services.IServices;
using Swizom.Utility;
using Swizom.ViewDataModels;
using SwizomDbContext.Models;

namespace Swizom.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _repository;

        public RestaurantService(IRestaurantRepository repository)
        {
            _repository = repository;
        }

        public async Task<(IEnumerable<Restaurant>, int)> GetRestaurantsAsync(string search, int page, int pageSize)
        {
            return await _repository.GetAllRestaurantsAsync(search, page, pageSize);
        }

        public async Task<Restaurant?> GetRestaurantAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> CreateRestaurantAsync(Restaurant restaurant)
        {
            await _repository.AddAsync(restaurant);
            return true;
        }

        public async Task<bool> UpdateRestaurantAsync(int id, Restaurant restaurant)
        {
            if (id != restaurant.RestaurantID)
            {
                return false;
            }
            await _repository.UpdateAsync(restaurant);
            return true;
        }

        public async Task<bool> DeleteRestaurantAsync(int id)
        {
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
