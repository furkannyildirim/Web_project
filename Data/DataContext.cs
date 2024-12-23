using Microsoft.EntityFrameworkCore;
using BooksStore.Models;

namespace BooksStore.Data
 
{
    public class DataContext: DbContext
    {
         public DataContext(DbContextOptions<DataContext> options): base(options)
        {
            
        }
        public DbSet<Product> Products  => Set<Product>();

        public DbSet<Genre> Genres => Set<Genre>(); 

         public DbSet<Order> Orders => Set<Order>();                         

        public DbSet<User> Users => Set<User>(); 

        public DbSet<UserRole> Roles => Set<UserRole>(); 
    }

    
}