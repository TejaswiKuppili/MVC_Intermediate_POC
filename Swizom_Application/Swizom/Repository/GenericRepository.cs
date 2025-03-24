using Microsoft.EntityFrameworkCore;
using Swizom.Repository.IRepository;
using Swizom.Utility;
using SwizomDbContext;
using SwizomDbContext.Models;

namespace Swizom.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public readonly ExceptionHandler _exceptionHandler;

        public GenericRepository(AppDbContext context, ExceptionHandler exceptionHandler)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _exceptionHandler = exceptionHandler;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () => await _dbSet.AsNoTracking().ToListAsync(), "Error in GetAllAsync");
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _exceptionHandler.HandleExceptionsAsync(async () => await _dbSet.FindAsync(id), "Error in GetByIdAsync");
        }

        public async Task AddAsync(T entity)
        {
            await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                _dbSet.Add(entity);
                await _context.SaveChangesAsync();
                return Task.CompletedTask;
            }, "Error in AddAsync");
        }

        public async Task UpdateAsync(T entity)
        {
            await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return Task.CompletedTask;
            }, "Error in UpdateAsync");
        }

        public async Task DeleteAsync(int id)
        {
            await _exceptionHandler.HandleExceptionsAsync(async () =>
            {
                var entity = await GetByIdAsync(id);
                if (entity != null)
                {
                    // Check for related entities before deletion
                    await HandleRelatedEntitiesBeforeDelete(entity);

                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                }
                return Task.CompletedTask;
            }, "Error in DeleteAsync");
        }

        private async Task HandleRelatedEntitiesBeforeDelete<T>(T entity) where T : class
        {
            if (entity is Restaurant restaurant)
            {
                var relatedMenuCategories = _context.MenuCategories
                    .Where(mc => mc.RestaurantID == restaurant.RestaurantID);
                var relatedMenuItems = _context.MenuItems.Where(mi => mi.RestaurantID == restaurant.RestaurantID);

                _context.MenuCategories.RemoveRange(relatedMenuCategories);
                _context.MenuItems.RemoveRange(relatedMenuItems);
                await _context.SaveChangesAsync();
            }
        }
    
    }
}
