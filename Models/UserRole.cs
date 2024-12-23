using System.ComponentModel.DataAnnotations;

namespace BooksStore.Models
{
    public class UserRole
    {
        [Key]
        public int RoleID { get; set; }

        public string? RoleName { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();


    }
}