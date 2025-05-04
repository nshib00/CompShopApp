using BLL.DTO;
using BLL.Services;
using BLL.Services.Interfaces;
using CompShop;
using CompShop.ViewModels;
using CompShop.Views;
using ComputerShop.Commands;
using ComputerShop.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ComputerShop.ViewModels
{
    public class CategoryManagementVM : INotifyPropertyChanged
    {
        private readonly ICategoryService _categoryService;
        private CategoryDto _selectedCategory;
        private string _searchText;

        public event PropertyChangedEventHandler PropertyChanged;

        public CategoryManagementVM(ICategoryService categoryService)
        {
            _categoryService = categoryService;

            LoadCategoriesCommand = new RelayCommand(_ => LoadCategories());
            AddCategoryCommand = new RelayCommand(_ => ShowAddCategoryView());
            EditCategoryCommand = new RelayCommand(_ => ShowEditCategoryView(), CanEditDeleteCategory);
            DeleteCategoryCommand = new RelayCommand(_ => DeleteCategory(), CanEditDeleteCategory);
            SearchCommand = new RelayCommand(_ => SearchCategories());
            ShowCategoryDetailsCommand = new RelayCommand(_ => ShowCategoryDetails());

            LoadCategories();
        }

        public ObservableCollection<CategoryDto> Categories { get; } = new ObservableCollection<CategoryDto>();
        public ObservableCollection<CategoryShortDto> AvailableParentCategories { get; } = new ObservableCollection<CategoryShortDto>();

        public CategoryDto SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        // Команды
        public ICommand LoadCategoriesCommand { get; }
        public ICommand AddCategoryCommand { get; }
        public ICommand EditCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ShowCategoryDetailsCommand { get; }

        private void LoadCategories()
        {
            Categories.Clear();
            var categories = _categoryService.GetCategoriesWithData();
            foreach (var category in categories)
            {
                Categories.Add(category);
            }

            LoadAvailableParentCategories();
        }

        private void LoadAvailableParentCategories()
        {
            AvailableParentCategories.Clear();
            var categories = _categoryService.GetCategoriesWithData();
            foreach (var category in categories)
            {
                AvailableParentCategories.Add(new CategoryShortDto(category));
            }
        }

        private void SearchCategories()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadCategories();
                return;
            }

            Categories.Clear();
            var results = _categoryService.SearchCategories(SearchText);
            foreach (var category in results)
            {
                Categories.Add(category);
            }
        }

        private void ShowAddCategoryView()
        {
            var addCategoryVM = new AddCategoryVM(_categoryService);
            addCategoryVM.OnCategoryAdded += () =>
            {
                LoadCategories();
                MessageBox.Show("Категория успешно добавлена", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            };

            var availableCategories = new ObservableCollection<CategoryShortDto>(AvailableParentCategories);
            addCategoryVM.SetCategories(availableCategories);

            var window = new AddCategoryWindow
            {
                DataContext = addCategoryVM,
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();
        }

        private void ShowEditCategoryView()
        {
            if (SelectedCategory == null) return;

            var editCategoryVM = new EditCategoryVM(_categoryService, SelectedCategory.Id);
            editCategoryVM.OnCategoryUpdated += () =>
            {
                LoadCategories();
                MessageBox.Show("Категория успешно обновлена", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            };

            var availableCategories = new ObservableCollection<CategoryShortDto>(
                AvailableParentCategories.Where(c => c.Id != SelectedCategory.Id).ToList());
            editCategoryVM.SetCategories(availableCategories);

            var window = new EditCategoryWindow
            {
                DataContext = editCategoryVM,
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();
        }

        private void ShowCategoryDetails()
        {
            if (SelectedCategory == null) return;

            var detailsVM = App.ServiceProvider.GetRequiredService<CategoryDetailsVM>();
            detailsVM.Initialize(SelectedCategory.Id);

            var window = new CategoryDetailsWindow
            {
                DataContext = detailsVM,
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();
        }

        private void DeleteCategory()
        {
            if (SelectedCategory == null) return;

            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить категорию '{SelectedCategory.Name}'?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _categoryService.DeleteCategory(SelectedCategory.Id);
                    LoadCategories();
                    MessageBox.Show("Категория успешно удалена", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении категории: {ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanEditDeleteCategory(object parameter)
        {
            return SelectedCategory != null;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}