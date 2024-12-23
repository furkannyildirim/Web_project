using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BooksStore.Models;

namespace BooksStore.Data{
    public static class SeedData{
       public static void EnsurePopulated(IApplicationBuilder app){

       DataContext context = app.ApplicationServices
            .CreateScope().ServiceProvider.GetRequiredService<DataContext>();
            


        if(context.Database.GetPendingMigrations().Any()){
            context.Database.Migrate();
        }



        if(!context.Roles.Any()){
            context.Roles.AddRange(
                 new UserRole{
                    RoleName = "Admin"
                },
        
                new UserRole{
                    RoleName = "Customer",
                }
            );
            context.SaveChanges();
        }



        if(!context.Genres.Any()){
            context.Genres.AddRange(
                new Genre{
                    Name = "Novel"
                },
                new Genre{
                    Name = "Science"
                },
                new Genre{
                    Name = "History"
                }
            );
            context.SaveChanges();
        }

        




        UserRole? adminRole = context.Roles.FirstOrDefault(r => r.RoleName == "Admin");

    

        if (!context.Users.Any())
        {
            if (adminRole != null)
            {
                var passwordCoder = new PasswordHasher<object>(); // PASSWORDLARI VERI TABANIMIZA SIFRELENMIS BIR SEKILDE KAYDETTIM
                context.Users.AddRange(
                    new User
                    {
                        UserName = "FatihFurkan",
                        Name = "Furkan",
                        Surname = "Yildirim",
                        Password = "FatihFurkan12$",
                        Role = adminRole,
                        Phone = "555555555"
                        
                    },
                    new User{
                        UserName = "AbdullahOnur",
                        Name = "Abdullah",
                        Surname = "Onur",
                        Password = "Abdullahonur1$",
                        Role = adminRole,
                        Phone = "66666666"

                        
                    },
                     new User{
                        UserName = "IsmailDal",
                        Name = "Ismail",
                        Surname = "Dal",
                        Password = "Ismaildal2$",
                        Role = adminRole ,
                        Phone = "444444444"

                        
                    },
                     new User{
                        UserName = "EnesAlbayrak",
                        Name = "Enes",
                        Surname = "Albayrak",
                        Password = "Enesalbayrak3$",
                        Role = adminRole,
                        Phone="333333333"
                        
                    }
                );
                context.SaveChanges();
            }
        }
       

       
       }
    }
}