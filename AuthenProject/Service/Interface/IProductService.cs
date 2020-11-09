using AuthenProject.Common;
using AuthenProject.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenProject.Service.Interface
{
    public interface IProductService
    {
        Task<MessageReponse> CreateProduct(ProductRequest request);
        Task<List<ProductReponse>> GetAllProduct(string Name, string Price);
        Task<ProductReponse> GetProductById(int ProductId);
        Task<MessageReponse> UpdateProduct(int Id,ProductRequest request);
        Task<MessageReponse> DeleteProduct(int Id);
    }
}
