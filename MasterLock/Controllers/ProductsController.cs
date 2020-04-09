using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using MasterLock.DTO;
using MasterLock.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MasterLock.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public ProductsController(ApplicationDbContext context,
            IConfiguration configuration,
            IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            _configuration = configuration;
        }
        public IActionResult GetAll()
        {
            string domain = (string)_configuration.GetValue<string>("BackendDomain");
            var model = _context.Products.Select(p => new ProductDTO
            {
                id=p.Id,
                title = p.Name,
                price = p.Price.ToString(),
                url = $"{domain}android/{p.Image}"
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
            double price=0.0;
            try
            {
                price=Double.Parse(model.price);
            }
            catch(Exception ex)
            {
                return BadRequest(new
                {
                    invalid = "Формат ціни 0.0"
                });
            }

            var imageName = Path.GetRandomFileName() + ".jpg";
            string savePath = _env.ContentRootPath;
            string folderImage = "images";
            savePath = Path.Combine(savePath, folderImage);
            savePath = Path.Combine(savePath, imageName);
            
            try
            {
                byte[] byteBuffer = Convert.FromBase64String(model.imageBase64);
                using (MemoryStream memoryStream = new MemoryStream(byteBuffer))
                {
                    memoryStream.Position = 0;
                    using (Image imgReturn = Image.FromStream(memoryStream))
                    {
                        memoryStream.Close();
                        byteBuffer = null;
                        var bmp = new Bitmap(imgReturn);
                        bmp.Save(savePath, ImageFormat.Jpeg);
                    }
                }
            }
            catch
            {
                return BadRequest(new
                {
                    invalid = "Помилка обробки фото"
                });
            }

            Product product = new Product
            {
                Name = model.title,
                Image = imageName,
                Price = price,
                Description="Капець"
            };
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok(
            new
            {
                id = product.Id
            });
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var p =_context.Products.SingleOrDefault(p => p.Id == id);
            if (p!=null)
            {
                var imageName = p.Name;
                string savePath = _env.ContentRootPath;
                string folderImage = "images";
                savePath = Path.Combine(savePath, folderImage);
                savePath = Path.Combine(savePath, imageName);
                if(System.IO.File.Exists(savePath))
                {
                    System.IO.File.Delete(savePath);
                }
                _context.Products.Remove(p);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest(new
                {
                    invalid = "Такого продукта немає!"
                });
            }

            
        }

    }
}