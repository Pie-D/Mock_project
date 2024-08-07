using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public static class DbInitializer
    {
        public static async Task Initialize(DataContext context, UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                //var user = new User
                //{
                //    UserName = "tuan",
                //    Email = "tuan@test.com"
                //};

                //await userManager.CreateAsync(user, "Pa$$w0rd");
                //await userManager.AddToRoleAsync(user, "Member");

                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@test.com"
                };

                await userManager.CreateAsync(admin, "Pa$$w0rd");
                await userManager.AddToRolesAsync(admin, new[] { "Admin", "Member" });

                context.SaveChanges();
            }
        }
    }
}