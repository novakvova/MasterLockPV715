using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasterLock.DTO;
using MasterLock.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MasterLock.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var model = _context.Users.Select(a => a).AsQueryable();
            return Ok(model);
        }
    }
}