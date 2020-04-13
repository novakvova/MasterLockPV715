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
        [HttpPost]
        public IActionResult Get([FromBody]UserSortingModel sorting)
        {
            var users = _context.Users.Select(a => a).AsQueryable();
            if (string.Compare("phone",sorting.Type) == 0)
            {
                users = _context.Users.Select(a => a).Where(a=> string.Compare(a.PhoneNumber,sorting.Value) == 0).AsQueryable();
            }
            else if (string.Compare("name", sorting.Type) == 0)
            {
                string name = sorting.Value.Split(' ')[0];
                string surname = sorting.Value.Split(' ')[1];

                users = users.Select(a => a).Where
                (a => string.Compare(a.Name.ToLower(), name.ToLower()) == 0 
                && string.Compare(a.Surname.ToLower(), surname.ToLower()) == 0)
                .AsQueryable();
            }
            else if (string.Compare("role", sorting.Type) == 0)
            {
             //   users = _context.Users.Select(a => a).Where(a => string.Compare(a.StringRole.ToLower(), sorting.Value.ToLower())== 0).AsQueryable();
            }
            else if(string.Compare("date", sorting.Type) == 0)
            { 
                users = _context.Users.Select(a => a).Where(a => string.Compare(a.Date.ToLower(), sorting.Value.ToLower()) == 0).AsQueryable();
            }
            return Ok(users);
        }
    }
}