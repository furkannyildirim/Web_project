using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BooksStore.Models;
using BooksStore.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BooksStore.Controllers;

public class HomeController : Controller
{
    private readonly DataContext _context;

    // Initializes the DataContext for database connection
    public HomeController(DataContext context)
    {
        _context = context;
    }

    // Returns the homepage view
    public IActionResult Index()
    {
        return View();
    }

    // Displays the create book page for users with "Admin" role
    [Authorize(Roles="Admin")]
    public IActionResult Create()
    {
        // Fetching genres from the database
        var genres = _context.Genres.ToList();
        
        // Sending genres to ViewBag as a SelectList for dropdown
        ViewBag.Genres = new SelectList(genres, "GenreId", "Name");
        
        return View();
    }

    // Handles the book creation for users with "Admin" role
    [HttpPost]
    [Authorize(Roles="Admin")]
    public async Task<IActionResult> Create(BookInfo model)
    {
        if (ModelState.IsValid)
        {
            // Checking if a product with the same name already exists in the database
            var controlProduct = _context.Products.FirstOrDefault(p => p.Name == model.Name);
            
            if (controlProduct != null)
            {
                ModelState.AddModelError("", "This book has already been registered.");
                return View(model);
            }

            // If there's an image, it's saved to the temporary folder
            if (model.Image != null && model.Image.Length > 0)
            {
                // Getting the file name and extension
                var fileName = Path.GetFileName(model.Image.FileName); // Example: "image.jpg"
                var fileExtension = Path.GetExtension(model.Image.FileName); // Example: ".jpg"
                
                // Saving the file to a specific directory
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/bookImages");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath); // Create the directory if it doesn't exist
                }

                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream); // Save the image asynchronously
                }

                // Fetching the genre from the database
                var genre = _context.Genres.FirstOrDefault(g => g.GenreId == model.GenreId);

                // Creating a new book object to save in the database
                var bookToCreate = new Product
                {
                    Name = model.Name,
                    Price = model.Price,
                    Image = "/bookImages/" + fileName, // Saving the image path in the database
                    Author = model.Author,
                    Genre = genre!,
                    Summary = model.Summary
                };

                _context.Products.Add(bookToCreate);
                await _context.SaveChangesAsync(); // Saving changes to the database
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "File upload failed.");
            }
        }

        // Fetching genres again to send to ViewBag for the SelectList
        var genres = _context.Genres.ToList();
        ViewBag.Genres = new SelectList(genres, "GenreId", "Name");
        
        return View(model);
    }

    // Displays the list of products with optional search and genre filtering
    public async Task<IActionResult> List(string searchTerm, int? genreId)
    {
        // Listing the genres for the dropdown filter
        ViewBag.Genres = new SelectList(await _context.Genres.ToListAsync(), "GenreId", "Name");

        // Building the query for products
        var productsQuery = _context.Products.Include(p => p.Genre).AsQueryable();

        // If there's a search term, filter the products by name or author
        if (!string.IsNullOrEmpty(searchTerm))
        {
            productsQuery = productsQuery.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()) || p.Author.ToLower().Contains(searchTerm.ToLower()));
        }

        // If a genre is selected, filter the products by genre
        if (genreId.HasValue && genreId.Value > 0)
        {
            productsQuery = productsQuery.Where(p => p.GenreId == genreId.Value);
        }

        var products = await productsQuery.ToListAsync(); // Fetch the filtered products from the database
        return View(products);
    }

    // Displays a list of users for admin users
    [Authorize(Roles="Admin")]
    public IActionResult UserList()
    {
        // Fetching users from the database
        var users = _context.Users.Include(u => u.Role);
        
        return View(users);
    }
}
