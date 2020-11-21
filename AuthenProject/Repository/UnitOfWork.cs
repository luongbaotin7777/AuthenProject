using AuthenProject.Repository.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenProject.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private  IUserRepository _user;
        private  IRoleRepository _role;
        private IProductRepository _product;
       
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public IUserRepository User 
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_context);
                }
                return _user;
            }
        }
        public IRoleRepository Role 
        {
            get
            {
                if (_role == null)
                {
                    _role = new RoleRepository(_context);
                }
                return _role;
            }
        }

        public IProductRepository Product
        {
            get
            {
                if(_product == null)
                {
                    _product = new ProductRepository(_context);
                }
                return _product;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
           await _context.SaveChangesAsync();
           
        }
        
    }
}
