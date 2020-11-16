using AuthenProject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthenProject.Repository.RepositoryBase
{
   public interface IRepositoryBase<T>
    {
        IQueryable<T> GetAll();
        Task<List<T>> GetAllAsync();

        T FindById(Guid id);
        Task<T> FindByIdAsync(Guid id);

        T SingleOrDefault(Expression<Func<T, bool>> expression);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);

        IQueryable<T> GetbyCondition(Expression<Func<T, bool>>expression);
        Task<List<T>> GetByConditionAsync(Expression<Func<T, bool>> expression);
        void Create(T entity);
        Task CreateAsync(T entity);
        Task CreateRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);
    }
}
