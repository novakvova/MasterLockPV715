using Bogus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterLock.Entities
{
    public class SeederDatabase
    {
        public static void SeedData(IServiceProvider services,
          IWebHostEnvironment env,
          IConfiguration config)
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var manager = scope.ServiceProvider.GetRequiredService<UserManager<DbUser>>();
                var managerRole = scope.ServiceProvider.GetRequiredService<RoleManager<DbRole>>();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                //SeedNewUsers(context);
                //SeedProduct(context);
                SeedUsers(manager, managerRole);
            }
        }
        private static void SeedUsers(UserManager<DbUser> userManager,
            RoleManager<DbRole> roleManager)
        {
            var roleName = "Admin";
            if (roleManager.FindByNameAsync(roleName).Result == null)
            {
                var result = roleManager.CreateAsync(new DbRole
                {
                    Name = roleName
                }).Result;
            }

            if (userManager.FindByEmailAsync("admin@gmail.com").Result == null)
            {
                string email = "admin@gmail.com";
                var user = new DbUser
                {
                    Email = email,
                    UserName = email,
                    Name = "Nikita",
                    Surname = "Kaida",
                    Country = "Ukraine",
                    City = "Rivne",
                    Age = "18",
                    Url = "http://st03.kakprosto.ru//images/article/2018/10/30/340157_5bd89b73dec2d5bd89b73dec67.jpeg",
                    PhoneNumber = "+11(111)111-11-11"
                };
                var result = userManager.CreateAsync(user, "Qwerty1-").Result;
                result = userManager.AddToRoleAsync(user, roleName).Result;
            }
        }
        private static void SeedProduct(ApplicationDbContext context)
        {
            var faker = new Faker();
            List<Product> products = new List<Product>();


            for (int i = 0; i < 30; i++)
            {
                products.Add(new Product { 
                    Name = faker.Commerce.ProductName(), 
                    Price = faker.Random.Double(1, 1000), 
                    Description = faker.Commerce.Product(), 
                    Image = faker.Image.PicsumUrl(400, 400, false, false, null) 
                });
            }


            for (int i = 0; i < products.Count; i++)
            {
                if (context.Products.SingleOrDefault(r => r.Name == products[i].Name) == null)
                {
                    context.Products.Add(products[i]);
                    context.SaveChanges();
                }
            }

        }

    }
}
