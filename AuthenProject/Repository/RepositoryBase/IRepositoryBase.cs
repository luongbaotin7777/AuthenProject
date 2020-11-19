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
        T FindById(int id);
        Task<T> FindByIdAsync(int id);

       
        T SingleOrDefault(Expression<Func<T, bool>> expression);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);

        IQueryable<T> GetbyWhereCondition(Expression<Func<T, bool>> expression);
        Task<List<T>> GetByWhereConditionAsync(Expression<Func<T, bool>> expression);
        Task<bool> GetByAnyConditionAsync(Expression<Func<T, bool>> expression);
        void Create(T entity);
        Task CreateAsync(T entity);
        Task CreateRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);
    }
}
