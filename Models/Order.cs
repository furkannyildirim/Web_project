using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BooksStore.Models;

public class Order
{
    [Key]
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
