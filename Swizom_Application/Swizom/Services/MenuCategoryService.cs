using Swizom.Repository;
using Swizom.Repository.IRepository;
using Swizom.Services.IServices;
using Swizom.ViewDataModels;
using SwizomDbContext.Models;

namespace Swizom.Services
{
    public class MenuCategoryService: IMenuCategoryService
    {
        private readonly IMenuCategoryRepository _repository;
        private readonly IGenericRepository<Restaurant> _restaurantRepository;

        public MenuCategoryService(IMenuCategoryRepository repository, IGenericRepository<Restaurant> restaurantRepository)
        {
            _repository = repository;
            _restaurantRepository = restaurantRepository;
        }

        public async Task<(IEnumerable<MenuCategoryDTO>, int)> GetMenuCategoriesAsync(int page, int pageSize)
        {
            return await _repository.GetAllMenuCategoriesAsync(page, pageSize);
        }

        public async Task<MenuCategory?> GetMenuCategoryAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Restaurant>> GetRestaurantsAsync()
        {
            return await _restaurantRepository.GetAllAsync();
        }

        public async Task<bool> CreateMenuCategoryAsync(MenuCategory menuCategory)
        {
            await _repository.AddAsync(menuCategory);
            return true;
        }

        public async Task<bool> UpdateMenuCategoryAsync(int id, MenuCategory menuCategory)
        {
            if (id != menuCategory.CategoryID)
            {
                return false;
            }
            await _repository.UpdateAsync(menuCategory);
            return true;
        }

        public async Task<bool> DeleteMenuCategoryAsync(int id)
        {
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
