using AuthenProject.Common;
using AuthenProject.Dtos;
using AuthenProject.EFModel;
using AuthenProject.Entities;
using AuthenProject.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenProject.Service.Handle
{
    public class ProductService:IProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<MessageReponse> CreateProduct(ProductRequest request)
        {
            var product = new Product()
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description

            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return new MessageReponse()
            {
                Message = "Create successed",
                IsSuccess = false
            };
        }

        public async Task<MessageReponse> DeleteProduct(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            if (product == null) throw new Exception($"Id not found, Please re-enter the correct Id ");
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return new MessageReponse()
            {
                Message = "Delete successed",
                IsSuccess = false
            };

        }

        public async Task<List<ProductReponse>> GetAllProduct(string Name, string Price)
        {
            if (!string.IsNullOrEmpty(Name) || !string.IsNullOrEmpty(Price))
            {
                var product = _context.Products.Where(x => x.Name.Contains(Name) || x.Price.ToString().Contains(Price));
                var result = await product.Select(x => new ProductReponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Description = x.Description
                }).ToListAsync();
                return result;
            }
            else
            {
                var product = await _context.Products.Select(x => new ProductReponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Description = x.Description
                }).ToListAsync();
                return product;
            }


        }

        public async Task<ProductReponse> GetProductById(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
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
            if(await _context.Products.AnyAsync(x=>x.Name == request.Name && x.Id != Id))
            {
                return new MessageReponse()
                {
                    Message = "Name already exists",
                    IsSuccess = false
                };
            }
            else
            {
                var product = await _context.Products.FindAsync(Id);
                if (product == null) throw new Exception("Id not Found");
                product.Name = request.Name;
                product.Price = request.Price;
                product.Description = request.Description;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return new MessageReponse()
                {
                    Message = "Update successed",
                    IsSuccess = false
                };
            }
            
        }
    }
}
