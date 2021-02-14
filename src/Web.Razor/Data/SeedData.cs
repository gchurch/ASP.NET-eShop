using ApplicationCore.Entities;
using IdentityServer4.EntityFramework.Options;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Razor.Authorization;

namespace Web.Razor.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new ProductDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ProductDbContext>>(),
                serviceProvider.GetRequiredService<
                    IOptions<OperationalStoreOptions>>()))
            {

                // The admin user can do anything
                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@eshop.co.uk");
                await EnsureRole(serviceProvider, adminID, Constants.ProductAdministratorsRole);

                SeedDB(context, adminID);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                            string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = UserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }

        public static void SeedDB(ProductDbContext context, string adminId)
        {
            // Look for any products.
            if (context.Products.Any())
            {
                return;     // DB has been seeded
            }

            context.Products.AddRange(
                new Product()
                {
                    Title = "Toy",
                    Description = "Plastic",
                    Seller = "Michael",
                    Price = 50,
                    Quantity = 2,
                    ImageUrl = "duck.png",
                    OwnerID = adminId
                },
                new Product()
                {
                    Title = "Book",
                    Description = "Hard back",
                    Seller = "Peter",
                    Price = 25,
                    Quantity = 4,
                    ImageUrl = "book.png",
                    OwnerID = adminId
                },
                new Product()
                {
                    Title = "Lamp",
                    Description = "Bright",
                    Seller = "David",
                    Price = 75,
                    Quantity = 1,
                    ImageUrl = "lamp.png",
                    OwnerID = adminId
                }
            );
            context.SaveChanges();
        }
    }
}
