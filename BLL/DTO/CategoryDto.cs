using DAL.Entities;

namespace BLL.DTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductCount { get; set; }
        public string? ParentCategoryName { get; set; }

        public CategoryDto() { }

        public CategoryDto(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            ProductCount = category.Products?.Count ?? 0;
            ParentCategoryName = category.ParentCategory?.Name;
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

    public class CategoryCreateDto
    {
        public required string Name { get; set; }
        public int? ParentCategoryId { get; set; }
    }

    public class CategoryUpdateDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int? ParentCategoryId { get; set; }
    }

    public class CategoryWithHierarchyDto : CategoryDto
    {
        public CategoryShortDto? ParentCategory { get; set; }
        public List<CategoryShortDto> SubCategories { get; set; }
        public List<ProductShortDto> Products { get; set; }

        public CategoryWithHierarchyDto(Category category) : base(category)
        {
            ParentCategory = category.ParentCategory != null
                ? new CategoryShortDto(category.ParentCategory)
                : null;

            SubCategories = category.SubCategories?
                .Select(sc => new CategoryShortDto(sc))
                .ToList() ?? new List<CategoryShortDto>();

            Products = category.Products?
                .Select(p => new ProductShortDto(p))
                .ToList() ?? new List<ProductShortDto>();
        }
    }

    public class CategoryTreeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CategoryTreeDto> SubCategories { get; set; }
    }

    public class CategoryShortDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CategoryShortDto(CategoryDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
        }

        public CategoryShortDto(Category cat)
        {
            Id = cat.Id;
            Name = cat.Name;
        }
    }
}
