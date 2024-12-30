using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApplication.Models;
using ProductApplication.Data;
using Microsoft.EntityFrameworkCore;

namespace ProductApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _context.Products
                .Include(p => p.Category) // Include Category details
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.CategoryId,
                    CategoryName = p.Category.CategoryName
                }).ToList();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .Where(p => p.ProductId == id)
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.CategoryId,
                    CategoryName = p.Category.CategoryName
                }).FirstOrDefault();

            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product product)
        {
            if (id != product.ProductId) return BadRequest();

            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
