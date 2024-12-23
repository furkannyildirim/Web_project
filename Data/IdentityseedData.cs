using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BooksStore.Data;

namespace BooksStore.Models
{
    public static class IdentitySeedData
    {
        
        private const string FurkanPassword = "FatihFurkan12$";
       
        private const string AbdullahPassword = "Abdullahonur1$";
        
        private const string IsmailPassword = "Ismaildal2$";
        
        private const string EnesPassword = "Enesalbayrak3$";
        private const string ADMIN_ROLE = "Admin";
        
        private const string CUSTOMER_ROLE = "Customer";

        public static async Task EnsurePopulatedAsync(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (await roleManager.FindByNameAsync(ADMIN_ROLE) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(ADMIN_ROLE));
                }
                
                if (await roleManager.FindByNameAsync(CUSTOMER_ROLE) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(CUSTOMER_ROLE));
                }


                var userFurkan = await userManager.FindByNameAsync("FatihFurkan");
                if (userFurkan == null)
                {
                    userFurkan = new IdentityUser
                    {
                        UserName = "FatihFurkan",
                        Email = "Furkan@gmail.com"
                    };

                    var result = await userManager.CreateAsync(userFurkan, FurkanPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(userFurkan, ADMIN_ROLE);
                    }
                    else
                    {
                        throw new Exception("Users Couldn't save to the Identity Database");
                    }
                }

                var userAbdullah = await userManager.FindByNameAsync("AbdullahOnur");
                if (userAbdullah == null)
                {
                    userAbdullah = new IdentityUser
                    {
                        UserName = "AbdullahOnur",
                        Email = "Onur@gmail.com"
                    };

                    var result = await userManager.CreateAsync(userAbdullah, AbdullahPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(userAbdullah, ADMIN_ROLE);
                    }
                    else
                    {
                        throw new Exception("Users Couldn't save to the Identity Database");
                    }
                }

                var userIsmail = await userManager.FindByNameAsync("IsmailDal");
                if (userIsmail == null)
                {
                    userIsmail = new IdentityUser
                    {
                        UserName = "IsmailDal",
                        Email = "İsmail@gmail.com"
                    };

                    var result = await userManager.CreateAsync(userIsmail, IsmailPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(userIsmail, ADMIN_ROLE);
                    }
                    else
                    {
                        throw new Exception("Users Couldn't save to the Identity Database");
                    }
                }


                var userEnes = await userManager.FindByNameAsync("EnesAlbayrak");
                if (userEnes == null)
                {
                    userEnes = new IdentityUser
                    {
                        UserName = "EnesAlbayrak",
                        Email = "Enes@gmail.com"
                    };

                    var result = await userManager.CreateAsync(userEnes, EnesPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(userEnes, ADMIN_ROLE);
                    }
                    else
                    {
                        throw new Exception("Users Couldn't save to the Identity Database");
                    }
                }











            }
        }
    }
}