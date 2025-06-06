﻿using BLL.DTO;
using BLL.Services.Interfaces;
using DAL.Context;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context = new AppDbContext();

        public int CreateCategory(CategoryCreateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                ParentCategoryId = dto.ParentCategoryId,
                Products = new List<Product>(),
                SubCategories = new List<Category>()
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return category.Id;
        }

        public void UpdateCategory(CategoryUpdateDto dto)
        {
            var category = _context.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefault(c => c.Id == dto.Id);

            if (category == null) return;

            if (dto.ParentCategoryId.HasValue &&
                IsCircularReference(dto.Id, dto.ParentCategoryId.Value))
            {
                throw new InvalidOperationException("Невозможно создать циклическую ссылку между категориями");
            }

            category.Name = dto.Name;
            category.ParentCategoryId = dto.ParentCategoryId;
            _context.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
            var category = _context.Categories
                .Include(c => c.Products)
                .Include(c => c.SubCategories)
                .FirstOrDefault(c => c.Id == id);

            if (category == null) return;

            foreach (var product in category.Products.ToList())
            {
                product.CategoryId = null;
            }

            foreach (var subCategory in category.SubCategories.ToList())
            {
                subCategory.ParentCategoryId = category.ParentCategoryId;
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

        public List<CategoryDto> GetCategoriesWithData()
        {
            return _context.Categories
                .Include(c => c.Products)
                .Include(c => c.SubCategories)
                .Include(c => c.ParentCategory)
                .AsNoTracking()
                .Select(c => new CategoryDto(c))
                .ToList();
        }

        public List<CategoryDto> GetCategories()
        {
            var categories = _context.Categories.ToList();
            return categories.Select(c => new CategoryDto(c)).ToList();
        }

        public CategoryWithHierarchyDto? GetCategoryWithHierarchy(int id)
        {
            var category = _context.Categories
                .Include(c => c.Products)
                .Include(c => c.SubCategories)
                .Include(c => c.ParentCategory)
                .AsNoTracking()
                .FirstOrDefault(c => c.Id == id);

            if (category != null)
                return new CategoryWithHierarchyDto(category);
            return null;
        }

        public List<CategoryTreeDto> GetCategoryTree()
        {
            var rootCategories = _context.Categories
                .Include(c => c.SubCategories)
                .Where(c => c.ParentCategoryId == null)
                .AsNoTracking()
                .ToList();

            return rootCategories.Select(c => BuildCategoryTree(c)).ToList();
        }

        private bool IsCircularReference(int categoryId, int potentialParentId)
        {
            if (categoryId == potentialParentId)
                return true;

            var parentCategory = _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefault(c => c.Id == potentialParentId);

            while (parentCategory != null)
            {
                if (parentCategory.Id == categoryId)
                    return true;

                parentCategory = parentCategory.ParentCategory;
            }

            return false;
        }

        private CategoryTreeDto BuildCategoryTree(Category category)
        {
            return new CategoryTreeDto
            {
                Id = category.Id,
                Name = category.Name,
                SubCategories = category.SubCategories?
                    .Select(sc => BuildCategoryTree(sc))
                    .ToList() ?? new List<CategoryTreeDto>()
            };
        }

        public List<CategoryDto> SearchCategories(string query)
        {
            return _context.Categories
                .Include(c => c.Products)
                .Where(c => c.Name.Contains(query))
                .AsNoTracking()
                .Select(c => new CategoryDto(c))
                .ToList();
        }
    }
}