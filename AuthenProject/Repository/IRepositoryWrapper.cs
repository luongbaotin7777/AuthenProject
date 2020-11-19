using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenProject.Repository
{
    interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IRoleRepository Role { get; }
        void Save();
        Task SaveAsync();
    }
}
