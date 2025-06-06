﻿using BLL.DTO;
using BLL.Services.Interfaces;
using DAL.Context;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context = new AppDbContext();

        public int CreateProduct(FullProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId,
                ManufacturerId = dto.ManufacturerId,
                ImagePath = dto.ImagePath,
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return product.Id;
        }

        public void UpdateProduct(FullProductDto dto)
        {
            var product = _context.Products.Find(dto.Id);
            if (product != null)
            {
                product.Name = dto.Name;
                product.Description = dto.Description;
                product.Price = dto.Price;
                product.CategoryId = dto.CategoryId;
                product.StockQuantity = dto.StockQuantity;
                product.ManufacturerId = dto.ManufacturerId;
                product.ImagePath = dto.ImagePath;

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

        public List<ProductDto> GetProductsByCategory(int categoryId)
        {
            return _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
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

        public FullProductDto? GetFullProductById(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .FirstOrDefault(p => p.Id == id);

            return product != null ? new FullProductDto(product) : null;
        }

        public List<ProductShortDto> GetShortProductList()
        {
            return _context.Products
                .Select(p => new ProductShortDto(p))
                .ToList();
        }

        public List<ProductDto> GetProductsBySearch(string searchText, int? categoryId = null)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(p => p.Name.Contains(searchText) || p.Description.Contains(searchText));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            return query
                .Select(p => new ProductDto(p))
                .ToList();
        }
    }
}
