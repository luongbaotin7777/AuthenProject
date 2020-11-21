using AuthenProject.Common;
using AuthenProject.Dtos;
using AuthenProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenProject.Service.Interface
{
    public interface IProductService
    {
        Task<MessageReponse> CreateProduct(ProductRequestDtos request);
        Task<List<ProductResponseDtos>> GetAllProduct();
        Task<ProductResponseDtos> GetProductById(int ProductId);
        Task<MessageReponse> UpdateProduct(int Id,ProductRequestDtos request);
        Task<MessageReponse> DeleteProduct(int Id);
        Task<List<ProductResponseDtos>> FindProduct(string Name, string Price);
    }
}
