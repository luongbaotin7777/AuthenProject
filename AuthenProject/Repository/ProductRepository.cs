﻿using AuthenProject.Entities;
using AuthenProject.Repository.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenProject.Repository
{
    public class ProductRepository:RepositoryBase<Product>,IProductRepository
    {
        public ProductRepository(ApplicationDbContext applicationDbContext)
            :base(applicationDbContext)
        {

        }
    }
}