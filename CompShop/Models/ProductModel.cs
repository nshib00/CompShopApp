using BLL.DTO;
using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BLL.Model
{
    public class ProductModel
    {
        private readonly AppDbContext _context = new AppDbContext();

        public int CreateProduct(ProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return product.Id;
        }

        public void UpdateProduct(ProductDto dto)
        {
            var product = _context.Products.Find(dto.Id);
            if (product != null)
            {
                product.Name = dto.Name;
                product.Description = dto.Description;
                product.Price = dto.Price;
                product.CategoryId = dto.CategoryId;

                _context.SaveChanges();
            }
        }

        public void DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public List<ProductDto> GetAllProducts()
        {
            return _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductDto(p))
                .ToList();
        }

        public ProductDto? GetProductById(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            return product != null ? new ProductDto(product) : null;
        }

        public List<ProductShortDto> GetShortProductList()
        {
            return _context.Products
                .Select(p => new ProductShortDto(p))
                .ToList();
        }
    }
}
