using BLL.DTO;
using BLL.Services;
using BLL.Services.Interfaces;
using CompShop.ViewModels;
using ComputerShop.Views;
using DAL.Context;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CompShop.Views.Pages
{
    public partial class CategoryPage : Page
    {
        private List<CategoryDto> _categories;
        private readonly ICategoryService _categoryService;

        public CategoryPage()
        {
            _categoryService = new CategoryService();
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            using var db = new AppDbContext();
            _categories = db.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ParentCategoryName = c.ParentCategory != null ? c.ParentCategory.Name : "-"
                }).ToList();

            CategoryGrid.ItemsSource = _categories;
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            using var db = new AppDbContext();
            var categories = db.Categories
                .Select(c => new CategoryShortDto(c))
                .ToList();

            var addVM = new AddCategoryVM(_categoryService);
            addVM.SetCategories(new ObservableCollection<CategoryShortDto>(categories));
            addVM.OnCategoryAdded += LoadCategories;

            var addWindow = new AddCategoryWindow();
            addVM.CloseAction = () => addWindow.Close();

            addWindow.DataContext = addVM;
            addWindow.ShowDialog();
        }

        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {

            if (CategoryGrid.SelectedItem is CategoryDto selected)
            {
                using var db = new AppDbContext();
                var category = db.Categories.FirstOrDefault(c => c.Id == selected.Id);

                if (category != null)
                {
                    var categoriesWithoutCurrent = db.Categories
                        .Where(c => c.Id != category.Id)
                        .Select(c => new CategoryShortDto(c))
                        .ToList();

                    var editVM = new EditCategoryVM(_categoryService, category.Id);
                    editVM.SetCategories(new ObservableCollection<CategoryShortDto>(categoriesWithoutCurrent));
                    var editWindow = new EditCategoryWindow { DataContext = editVM };
                    editWindow.CloseAction = () => editWindow.Close();
                    editWindow.ShowDialog();
                    LoadCategories();
                }
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryGrid.SelectedItem is CategoryDto selected)
            {
                using var db = new AppDbContext();
                var category = db.Categories.Find(selected.Id);
                if (category != null)
                {
                    db.Categories.Remove(category);
                    db.SaveChanges();
                    LoadCategories();
                }
            }
        }
    }
}
