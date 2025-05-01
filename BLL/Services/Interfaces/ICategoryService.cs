using BLL.DTO;

namespace BLL.Services.Interfaces
{
    public interface ICategoryService
    {
        int CreateCategory(CategoryCreateDto dto);
        void UpdateCategory(CategoryUpdateDto dto);
        void DeleteCategory(int id);
        List<CategoryDto> GetCategoriesWithData();
        CategoryWithHierarchyDto? GetCategoryWithHierarchy(int id);
        List<CategoryTreeDto> GetCategoryTree();
        List<CategoryDto> SearchCategories(string query);
    }
}
