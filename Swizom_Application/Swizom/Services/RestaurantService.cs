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
        private readonly ExceptionHandler _exceptionHandler;

        public RestaurantService(IRestaurantRepository repository, ExceptionHandler exceptionHandler)
        {
            _repository = repository;
            _exceptionHandler = exceptionHandler;
        }

        public async Task<(IEnumerable<Restaurant>, int)> GetRestaurantsAsync(int page, int pageSize)
        {
            return await _repository.GetAllRestaurantsAsync(page, pageSize);
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
