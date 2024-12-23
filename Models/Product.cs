using System.ComponentModel.DataAnnotations;

namespace BooksStore.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Kitap adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Kitap adı en fazla 100 karakter olabilir.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır.")]
        public decimal? Price { get; set; }  

        public string? Image { get; set; }

         [Required(ErrorMessage = "Yazar adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Yazar adı en fazla 100 karakter olabilir.")]
        public string? Author { get; set; }

        [Required(ErrorMessage = "Özet zorunludur.")]
        [StringLength(500, ErrorMessage = "Özet en fazla 500 karakter olabilir.")]
        public string? Summary { get; set; }

        
        public int GenreId { get; set; }

        // [Required(ErrorMessage = "Tür seçilmelidir.")]
        public Genre Genre { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}