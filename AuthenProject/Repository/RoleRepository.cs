using AuthenProject.Entities;
using AuthenProject.Repository.RepositoryBase;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenProject.Repository
{
    public class RoleRepository:RepositoryBase<AppRole>,IRoleRepository

    {
       
        public RoleRepository(ApplicationDbContext applicationDbContext)
            :base(applicationDbContext)
        {
          
        }

    }
}
