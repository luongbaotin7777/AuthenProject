using AuthenProject.Common;
using AuthenProject.Dtos;
using AuthenProject.Entities;
using AuthenProject.Repository;
using AuthenProject.Repository.RepositoryBase;
using AuthenProject.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenProject.Service.Handle
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public ProductService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public async Task<MessageReponse> CreateProduct(ProductRequest request)
        {
            var exists = await _repositoryWrapper.Product.FirstOrDefaultAsync(x => x.Name == request.Name);
            if (exists != null)
            {
                return new MessageReponse()
                {
                    Message = "Name is already exists",
                    IsSuccess = false
                };
            }
            var product = new Product()
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description

            };
            await _repositoryWrapper.Product.CreateAsync(product);
            await _repositoryWrapper.SaveAsync();
            return new MessageReponse()
            {
                Message = "Create successed",
                IsSuccess = false
            };
        }

        public async Task<MessageReponse> DeleteProduct(int Id)
        {
            var product = await _repositoryWrapper.Product.FindByIdAsync(Id);
            if (product == null) throw new Exception($"Id not found, Please re-enter the correct Id ");
            _repositoryWrapper.Product.Delete(product);
            await _repositoryWrapper.SaveAsync();
            return new MessageReponse()
            {
                Message = "Delete successed",
                IsSuccess = false
            };

        }

        public async Task<List<ProductReponse>> FindProduct(string Name, string Price)
        {
            var product = _repositoryWrapper.Product.GetbyWhereCondition(x => x.Name.Contains(Name) || x.Price.ToString().Contains(Price));
            var result = await product.Select(x => new ProductReponse()
            {
                Id = x.Id,
                Name  = x.Name,
                Price = x.Price,
                Description = x.Description
            }).ToListAsync();
            return result;
        }

        public async Task<List<ProductReponse>> GetAllProduct()
        {

            var products = await _repositoryWrapper.Product.GetAllAsync();
            var data1= new List<ProductReponse>();
            foreach(var product in products)
            {
                var data = new ProductReponse()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                   
                };
                data1.Add(data);
            }
            return data1;
            
                
        }

        public async Task<ProductReponse> GetProductById(int Id)
        {
            var product = await _repositoryWrapper.Product.FindByIdAsync(Id);
            if (product == null) throw new Exception($"Cannot find a product with id: {Id}");
            var data = new ProductReponse()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
            return data;

        }

        public async Task<MessageReponse> UpdateProduct(int Id,ProductRequest request)
        {
            var product = await _repositoryWrapper.Product.FindByIdAsync(Id);
            if (product == null) throw new Exception("Id not Found");
            var existing = await _repositoryWrapper.Product.FirstOrDefaultAsync(x => x.Name == request.Name && x.Id != Id);
            if (existing == null)
            {
                if (!string.IsNullOrEmpty(request.Name))
                {
                    product.Name = request.Name;
                }
                else
                {
                    product.Name = product.Name;
                }
                if (request.Price.HasValue)
                {
                    product.Price = request.Price;
                }
                else
                {
                    product.Price = product.Price;
                }
                if (!string.IsNullOrEmpty(request.Description))
                {
                    product.Description = request.Description;
                }
                else
                {
                    product.Description = product.Description;
                }
               
                _repositoryWrapper.Product.Update(product);
                await _repositoryWrapper.SaveAsync();
                return new MessageReponse()
                {
                    Message = "Update successed",
                    IsSuccess = true
                };  
            }
            return new MessageReponse()
                {
                    Message = "Name already exists",
                    IsSuccess = false
                };
            
        }
    }
}
