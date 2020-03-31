using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using MasterLock.DTO;
using MasterLock.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MasterLock.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult GetAll()
        {
            var model = _context.Products.Select(p => new ProductDTO
            {
                title = p.Name,
                price = p.Price.ToString(),
                url = p.Image
            }).ToList();
            Thread.Sleep(2000);

            return Ok(model);
        }
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody]ProductCreateDTO model)
        {
            
            if(!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    invalid = "Не валідна модель"
                });
            }
            var faker = new Faker();
            Product product = new Product
            {
                Name = model.title,
                Image = faker.Image.PicsumUrl(400, 400, false, false, null),
                Price = Double.Parse(model.price)
            };
            _context.Products.Add(product);
            
            return Ok(
            new
            {
                id = product.Id
            });
        }

    }
}