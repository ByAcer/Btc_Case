using Instruction.Domain.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Instruction.Repository.Core
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _dbSet = context.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            var result = await _dbSet.AddAsync(entity);
            return result.Entity;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FindAsync(expression);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking().AsQueryable();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }
}
