using DAL;
using DAL.Entities;

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
        public string CategoryName { get; set; }

        public ProductDto() { }

        public ProductDto(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            CategoryId = product.CategoryId;
            CategoryName = product.Category?.Name;
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
}
