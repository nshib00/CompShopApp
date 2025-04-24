using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using BLL.DTO;
using BLL.Model;
using ComputerShop.Commands;
using ComputerShop.Models;

namespace CompShop.ViewModels
{
    public class CustomerVM : INotifyPropertyChanged
    {
        private readonly ProductModel _productModel;
        private readonly CategoryModel _categoryModel;
        private ObservableCollection<ProductDto> _products;
        private ObservableCollection<CategoryDto> _categories;
        private CategoryDto _selectedCategory;
        private string _searchText;

        public ICommand LogoutCommand { get; }
        public ICommand BuyProductCommand { get; }

        public CustomerVM()
        {
            _productModel = new ProductModel();
            _categoryModel = new CategoryModel();

            LogoutCommand = new RelayCommand(Logout);
            BuyProductCommand = new RelayCommand(BuyProduct);

            LoadCategories();
            LoadProducts();
        }

        public ObservableCollection<ProductDto> Products
        {
            get => _products;
            set
            {
                if (_products != value)
                {
                    _products = value;
                    OnPropertyChanged(nameof(Products));
                }
            }
        }

        public ObservableCollection<CategoryDto> Categories
        {
            get => _categories;
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    OnPropertyChanged(nameof(Categories));
                }
            }
        }

        public CategoryDto SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                    LoadProducts();  // Загружаем товары при изменении категории
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    LoadProducts();  // Загружаем товары при изменении текста поиска
                }
            }
        }

        public void LoadCategories()
        {
            Categories = new ObservableCollection<CategoryDto>(_categoryModel.GetAllCategories());
        }

        public void LoadProducts()
        {
            if (SelectedCategory == null && string.IsNullOrEmpty(SearchText))
            {
                Products = new ObservableCollection<ProductDto>(_productModel.GetAllProducts());
            }
            else
            {
                Products = new ObservableCollection<ProductDto>(_productModel.GetProductsBySearch(SearchText, SelectedCategory?.Id));
            }
        }

        public void Logout(object parameter)
        {
            // Логика выхода
        }

        public void BuyProduct(object parameter)
        {
            var product = parameter as ProductDto;
            // Логика покупки товара
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
