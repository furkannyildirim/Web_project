using Microsoft.AspNetCore.Mvc;
using BooksStore.Models;
using BooksStore.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BooksStore.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BooksStore.Controllers
{
    public class CartController: Controller
    {
        private readonly DataContext _context;

        // Initializes the DataContext for database connection
        public CartController(DataContext context)
        {
            _context = context;
        }

        // Displays the details of a product
        public IActionResult Details(int id)
        {
            var product = _context.Products.Include(p => p.Genre).FirstOrDefault(p => p.ProductId == id); // Fetch the product from the database
            if (product == null)
            {
                return NotFound(); // Returns 404 if the product is not found
            }
            return View(product); // Sends the product to the view
        }

        // Displays the edit page for products, only accessible by Admin
        [Authorize(Roles="Admin")]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Include(p => p.Genre).FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            var genres = _context.Genres.ToList();
            ViewBag.Genres = new SelectList(genres, "GenreId", "Name"); // Sends genres to the view

            return View(product);
        }

        // Handles the product edit POST request, only accessible by Admin
        [HttpPost]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Edit(BookInfo bookInfo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Genres = new SelectList(_context.Genres, "Id", "Name"); // Fills genres again in case of validation errors
                return View(bookInfo);
            }

            // Fetch the existing product to update
            var product = await _context.Products.FindAsync(bookInfo.ProductId);
            if (product == null)
            {
                return NotFound();
            }

            // Mapping data from ViewModel to Product model
            product.Name = bookInfo.Name;
            product.Price = bookInfo.Price;
            product.Author = bookInfo.Author;
            product.Summary = bookInfo.Summary;
            product.GenreId = bookInfo.GenreId;

            // Process the image if a new one is uploaded
            if (bookInfo.Image != null && bookInfo.Image.Length > 0)
            {
                // Get the file name and extension
                var fileName = Path.GetFileName(bookInfo.Image.FileName); // Example: "image.jpg"
                var fileExtension = Path.GetExtension(bookInfo.Image.FileName); // Example: ".jpg"
                
                // Save the file to a specific directory
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/bookImages");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await bookInfo.Image.CopyToAsync(stream); // Save the image asynchronously
                }

                product.Image = "/bookImages/" + fileName; // Update product image path
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("List", "Home"); // Redirect to the list after updating
        }

        // Deletes a product, only accessible by Admin
        [HttpGet]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Delete the product
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("List", "Home"); // Redirect to the list after deletion
        }

        // Displays the payment page for a product
        [HttpGet]
        public IActionResult Payment(int ProductId)
        {
            if (!User.Identity.IsAuthenticated) // Check if the user is logged in
            {
                TempData["ErrorMessage"] = "You must log in to make a purchase.";
                return RedirectToAction("Login", "Account");
            }

            var ProductView = new ProductInfo
            {
                ProductId = ProductId
            };
            return View(ProductView);
        }

        // Creates an order for the product
        public IActionResult CreateOrder(int ProductId)
        {
            var loginUser = _context.Users
                .FirstOrDefault(u => u.UserName == User.Identity.Name);
            var product = _context.Products.FirstOrDefault(p => p.ProductId == ProductId);

            var newOrder = new Order
            {
                OrderDate = DateTime.Now,
                Product = product,
                User = loginUser
            };
            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            return RedirectToAction("Profile", "Cart"); // Redirect to the profile after creating the order
        }

        // Displays the user's profile with their orders
        public IActionResult Profile()
        {
            var userId = User.Identity.Name; // Get the user's name or ID
            var user = _context.Users
                .Include(u => u.Orders)  // Get the user's related orders
                .ThenInclude(o => o.Product)  // Include the product related to each order
                .FirstOrDefault(u => u.UserName == userId);

            if (user == null)
            {
                return NotFound(); // Returns error if the user is not found
            }

            var viewModel = new UserProfileViewModel
            {
                UserName = user.UserName,
                FullName = $"{user.Name} {user.Surname}",
                Orders = user.Orders?.ToList() // Check if orders are null
            };

            return View(viewModel);
        }

        // Deletes an order
        [HttpPost]
        public IActionResult DeleteOrder(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);

            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }

            return RedirectToAction("Profile", "Cart"); // Redirect to the profile after deleting the order
        }
    }
}
