using System.Linq.Expressions;

namespace Instruction.Domain.Core
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<T> FindAsync(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
