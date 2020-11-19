using AuthenProject.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthenProject.Repository.RepositoryBase
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ApplicationDbContext _context;
        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
        }


        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        

        public T FindById(Guid id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> FindByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public T FindById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        
        public async Task<T> FindByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }
        public T SingleOrDefault(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().SingleOrDefault(expression);
        }

        public IQueryable<T> GetbyWhereCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).AsNoTracking();
        }

        public async Task<List<T>> GetByWhereConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }
        public async Task<bool> GetByAnyConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().AnyAsync(expression);
        }

        public void Create(T entity)
        {
             _context.Set<T>().Add(entity);
        }

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task CreateRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public void Delete(T entity)
        {
             _context.Set<T>().Remove(entity);
        }

       

        public void Update(T entity)
        {
             _context.Set<T>().Update(entity);
        }

       
    }
}
