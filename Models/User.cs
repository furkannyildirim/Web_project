using System.ComponentModel.DataAnnotations;

namespace BooksStore.Models
{
    public class User
    {      
         [Key]
         public int UserId { get; set; }

         public string? Name { get; set; }

         public string? UserName { get; set; }

         public string? Surname { get; set; }

     
         public string? Phone { get; set; }

         public string? Password { get; set; }

         public int RoleID { get; set; }

        public UserRole Role { get; set; } = null!;

        public ICollection<Order> Orders { get; set; }  = new List<Order>();


    }
}