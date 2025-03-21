using Swizom.Repository.IRepository;
using Swizom.Services.IServices;
using Swizom.ViewDataModels;
using SwizomDbContext.Models;

namespace Swizom.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _repository;
        private readonly IGenericRepository<MenuCategory> _categoryRepository;
        private readonly IGenericRepository<Restaurant> _restaurantRepository;

        public MenuItemService(IMenuItemRepository repository,
                               IGenericRepository<MenuCategory> categoryRepository,
                               IGenericRepository<Restaurant> restaurantRepository)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _restaurantRepository = restaurantRepository;
        }

        public async Task<IEnumerable<MenuItemDTO>> GetMenuItemsAsync()
        {
            return await _repository.GetAllMenuItemsAsync();
        }
        public async Task<MenuItem?> GetMenuItemAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async Task<IEnumerable<MenuCategory>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }
        public async Task<IEnumerable<Restaurant>> GetRestaurantsAsync()
        {
            return await _restaurantRepository.GetAllAsync();
        }

        public async Task<bool> CreateMenuItemAsync(MenuItem menuItem)
        {
            await _repository.AddAsync(menuItem);
            return true;
        }

        public async Task<bool> UpdateMenuItemAsync(int id, MenuItem menuItem)
        {
            if (id != menuItem.ItemID)
            {
                return false;
            }
            await _repository.UpdateAsync(menuItem);
            return true;
        }

        public async Task<bool> DeleteMenuItemAsync(int id)
        {
            await _repository.DeleteAsync(id);
            return true;
        }
    }

}
