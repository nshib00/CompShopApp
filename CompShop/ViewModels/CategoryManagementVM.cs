using BLL.DTO;
using ComputerShop.Commands;
using ComputerShop.Models;
using ComputerShop.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ComputerShop.ViewModels
{
    public class CategoryManagementViewModel : INotifyPropertyChanged
    {
        private readonly CategoryModel _categoryModel;
        private CategoryDto _selectedCategory;
        private string _searchText;

        public event PropertyChangedEventHandler PropertyChanged;

        public CategoryManagementViewModel(CategoryModel categoryModel)
        {
            _categoryModel = categoryModel;

            // Инициализация команд
            LoadCategoriesCommand = new RelayCommand(_ => LoadCategories());
            AddCategoryCommand = new RelayCommand(_ => ShowAddCategoryView());
            EditCategoryCommand = new RelayCommand(_ => ShowEditCategoryView(), CanEditDeleteCategory);
            DeleteCategoryCommand = new RelayCommand(_ => DeleteCategory(), CanEditDeleteCategory);
            SearchCommand = new RelayCommand(_ => SearchCategories());
            ShowCategoryDetailsCommand = new RelayCommand(_ => ShowCategoryDetails());

            // Загрузка данных
            LoadCategories();
        }

        // Свойства
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

        // Методы
        private void LoadCategories()
        {
            Categories.Clear();
            var categories = _categoryModel.GetAllCategories();
            foreach (var category in categories)
            {
                Categories.Add(category);
            }

            LoadAvailableParentCategories();
        }

        private void LoadAvailableParentCategories()
        {
            AvailableParentCategories.Clear();
            var categories = _categoryModel.GetAllCategories();
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
            var results = _categoryModel.SearchCategories(SearchText);
            foreach (var category in results)
            {
                Categories.Add(category);
            }
        }

        private void ShowAddCategoryView()
        {
            var viewModel = new AddCategoryVM(_categoryModel)
            {
                Categories = new ObservableCollection<CategoryShortDto>(AvailableParentCategories),
                OnCategoryAdded = () =>
                {
                    LoadCategories();
                    MessageBox.Show("Категория успешно добавлена", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            };

            var window = new AddCategoryWindow
            {
                DataContext = viewModel,
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();
        }

        private void ShowEditCategoryView()
        {
            if (SelectedCategory == null) return;

            var viewModel = new EditCategoryVM(_categoryModel, SelectedCategory.Id)
            {
                Categories = new ObservableCollection<CategoryShortDto>(AvailableParentCategories
                    .Where(c => c.Id != SelectedCategory.Id).ToList()),
                OnCategoryUpdated = () =>
                {
                    LoadCategories();
                    MessageBox.Show("Категория успешно обновлена", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            };

            var window = new EditCategoryWindow
            {
                DataContext = viewModel,
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();
        }

        private void ShowCategoryDetails()
        {
            if (SelectedCategory == null) return;

            var viewModel = new CategoryDetailsVM(_categoryModel, SelectedCategory.Id)
            {
                OnBackRequested = () => { /* можно добавить логику при необходимости */ }
            };

            var window = new CategoryDetailsWindow
            {
                DataContext = viewModel,
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
                    _categoryModel.DeleteCategory(SelectedCategory.Id);
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