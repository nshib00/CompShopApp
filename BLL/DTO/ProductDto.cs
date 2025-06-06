﻿using DAL.Models;

namespace BLL.DTO
{
    public class BaseProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductDto : BaseProductDto
    {
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? ImagePath { get; set; }

        public int StockQuantity { get; set; }

        public ProductDto() { }

        public ProductDto(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;

            Price = product.Price;
            CategoryId = product.CategoryId;
            CategoryName = product.Category?.Name;
            StockQuantity = product.StockQuantity;
            ImagePath = product.ImagePath;
        }
    }

    public class ProductShortDto : BaseProductDto
    {
        public ProductShortDto() { }

        public ProductShortDto(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Price = product.Price;
        }
    }

    public class FullProductDto : BaseProductDto
    {
        public string Description { get; set; }
        public int StockQuantity { get; set; }

        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public int? ManufacturerId { get; set; }
        public string? ManufacturerName { get; set; }

        public string? ImagePath { get; set; }

        public FullProductDto() { }

        public FullProductDto(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            StockQuantity = product.StockQuantity;

            CategoryId = product.Category?.Id;
            CategoryName = product.Category?.Name;

            ManufacturerId = product.Manufacturer?.Id;
            ManufacturerName = product.Manufacturer?.Name;

            ImagePath = null;
        }
    }
}
