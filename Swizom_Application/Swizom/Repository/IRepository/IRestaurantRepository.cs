using Swizom.ViewDataModels;
using SwizomDbContext.Models;

namespace Swizom.Repository.IRepository
{
    public interface IRestaurantRepository : IGenericRepository<Restaurant>
    {
        Task<(IEnumerable<Restaurant>, int)> GetAllRestaurantsAsync(string search, int page, int pageSize);
    }
}
