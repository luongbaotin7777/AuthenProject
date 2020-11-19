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
        Task<MessageReponse> CreateProduct(ProductRequest request);
        Task<List<ProductReponse>> GetAllProduct();
        Task<ProductReponse> GetProductById(int ProductId);
        Task<MessageReponse> UpdateProduct(int Id,ProductRequest request);
        Task<MessageReponse> DeleteProduct(int Id);
        Task<List<ProductReponse>> FindProduct(string Name, string Price);
    }
}
