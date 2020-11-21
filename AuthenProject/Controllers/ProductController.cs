using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenProject.Authorization;
using AuthenProject.Dtos;
using AuthenProject.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }

        //POST: api/product
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] ProductRequestDtos request)
        {
            var product = await _service.CreateProduct(request);
            return Ok(product);
        }
        //GET: api/product/(search key = Name,Price)
        [HttpGet]
        
        //[Authorize(Permission.Users.View)]
        public async Task<IActionResult> GetAll()
        {         
            var product = await _service.GetAllProduct();
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        //Get api/product/id
        [HttpGet("{Id}")]
        //[Authorize(Permission.Users.View)]
        public async Task<IActionResult> GetById(int Id)
        {
            var product = await _service.GetProductById(Id);
            if (product == null)
            {
                return NotFound($"Id not Found. Please re-enter the corret Id");
            }
            return Ok(product);
        }
        //PUT: api/product/id
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(int Id,[FromBody] ProductRequestDtos request)
        {

            var product = await _service.UpdateProduct(Id,request);
            if (product == null)
            {
                return BadRequest(product);
            }
            return Ok(product);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var product = await _service.DeleteProduct(Id);
            if (product == null)
            {
                return NotFound(product);
            }
            return Ok(product);
        }
        [HttpGet("Search")]
        public async Task<IActionResult> SearchProduct(string UserName, string Price)
        {
            var product = await _service.FindProduct(UserName, Price);
            if (product == null)
            {
                return NotFound(product);
            }
            return Ok(product);
        }
    }
}
