using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            //List<ProductDTO> model = new List<ProductDTO>() {

            //    new ProductDTO{
            //        title = "Salo",
            //        price = "99999",
            //        url = "http://10.0.2.2/android/1.jpg"
            //    }
            //};
            var model = _context.Products.Select(p => new ProductDTO
            {
                title = p.Name,
                price = p.Price.ToString(),
                url = p.Image
            }).ToList();
            Thread.Sleep(2000);

            return Ok(model);
        }

    }
}