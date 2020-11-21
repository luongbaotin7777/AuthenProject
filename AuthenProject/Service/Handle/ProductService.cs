﻿using AuthenProject.Common;
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
        private readonly IUnitOfWork _unitofwork;
        public ProductService(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public async Task<MessageReponse> CreateProduct(ProductRequestDtos request)
        {
            var exists = await _unitofwork.Product.FirstOrDefaultAsync(x => x.Name == request.Name);
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
            await _unitofwork.Product.CreateAsync(product);
            await _unitofwork.SaveAsync();
            return new MessageReponse()
            {
                Message = "Create successed",
                IsSuccess = false
            };
        }

        public async Task<MessageReponse> DeleteProduct(int Id)
        {
            var product = await _unitofwork.Product.FindByIdAsync(Id);
            if (product == null) throw new Exception($"Id not found, Please re-enter the correct Id ");
            _unitofwork.Product.Delete(product);
            await _unitofwork.SaveAsync();
            return new MessageReponse()
            {
                Message = "Delete successed",
                IsSuccess = false
            };

        }

        public async Task<List<ProductResponseDtos>> FindProduct(string Name, string Price)
        {
            var product = _unitofwork.Product.GetbyWhereCondition(x => x.Name.Contains(Name) || x.Price.ToString().Contains(Price));
            var result = await product.Select(x => new ProductResponseDtos()
            {
                Id = x.Id,
                Name  = x.Name,
                Price = x.Price,
                Description = x.Description
            }).ToListAsync();
            return result;
        }

        public async Task<List<ProductResponseDtos>> GetAllProduct()
        {

            var products = await _unitofwork.Product.GetAllAsync();
            var data1= new List<ProductResponseDtos>();
            foreach(var product in products)
            {
                var data = new ProductResponseDtos()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                   
                };
                data1.Add(data);
            }
            return data1;
            
                
        }

        public async Task<ProductResponseDtos> GetProductById(int Id)
        {
            var product = await _unitofwork.Product.FindByIdAsync(Id);
            if (product == null) throw new Exception($"Cannot find a product with id: {Id}");
            var data = new ProductResponseDtos()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
            return data;

        }

        public async Task<MessageReponse> UpdateProduct(int Id,ProductRequestDtos request)
        {
            var product = await _unitofwork.Product.FindByIdAsync(Id);
            if (product == null) throw new Exception("Id not Found");
            var existing = await _unitofwork.Product.FirstOrDefaultAsync(x => x.Name == request.Name && x.Id != Id);
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
               
                _unitofwork.Product.Update(product);
                await _unitofwork.SaveAsync();
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
