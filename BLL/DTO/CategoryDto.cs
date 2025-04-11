using DAL;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductCount { get; set; }

        public CategoryDto() { }

        public CategoryDto(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            ProductCount = category.Products?.Count ?? 0;
        }
    }

    public class CategoryWithProductsDto : CategoryDto
    {
        public List<ProductShortDto> Products { get; set; }

        public CategoryWithProductsDto(Category category) : base(category)
        {
            Products = category.Products?.Select(p => new ProductShortDto(p)).ToList();
        }
    }
}
