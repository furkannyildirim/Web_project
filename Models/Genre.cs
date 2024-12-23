namespace BooksStore.Models
{
    public class Genre
    {
        public int GenreId { get; set; }

        public string? Name { get; set; }

        public ICollection<Product> Products  { get; set; } = new List<Product>();
    }
}