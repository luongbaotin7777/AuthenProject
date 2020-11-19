using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenProject.Repository
{
   public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IRoleRepository Role { get; }
        IProductRepository Product { get; }
        void Save();
        Task SaveAsync();
    }
}
