using CompShop.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using BLL.Model;
using BLL.DTO;
using ComputerShop.Commands;
using ComputerShop.Views;
using DAL.Entities;
using ComputerShop.Models;

namespace CompShop.ViewModels
{
    public class ProductVM : INotifyPropertyChanged
    {
        private readonly ProductModel _productModel = new();
        private readonly CategoryModel _categoryModel = new();
        private ObservableCollection<ProductDto> _products;
        private ObservableCollection<CategoryDto> _categories;
        private CategoryDto _selectedCategory;
        private ProductDto _selectedProduct;

        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }

        public ProductVM()
        {
            AddProductCommand = new RelayCommand(AddProduct, CanExecuteAddProduct);
            EditProductCommand = new RelayCommand(EditProduct, CanExecuteEditProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct, CanExecuteDeleteProduct);

            LoadCategories();
            LoadProducts();
        }

        // Свойства
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
                    LoadProducts(); // Загружаем товары при изменении категории
                }
            }
        }

        public ProductDto SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    OnPropertyChanged(nameof(SelectedProduct));
                }
            }
        }

        // Метод для загрузки категорий
        private void LoadCategories()
        {
            Categories = new ObservableCollection<CategoryDto>(_categoryModel.GetAllCategories());
        }

        // Метод для загрузки товаров
        private void LoadProducts()
        {
            if (SelectedCategory == null)
            {
                // Если категория не выбрана, загружаем все товары
                Products = new ObservableCollection<ProductDto>(_productModel.GetAllProducts());
            }
            else
            {
                // Загружаем товары по выбранной категории
                Products = new ObservableCollection<ProductDto>(_productModel.GetProductsByCategory(SelectedCategory.Id));
            }
        }

        private void AddProduct(object parameter)
        {
            var viewModel = new AddEditProductVM();
            var addProductWindow = new AddProductWindow
            {
                DataContext = viewModel
            };
            addProductWindow.ShowDialog();
            LoadProducts(); // обновляем список после закрытия
        }

        private bool CanExecuteAddProduct(object parameter)
        {
            return true;
        }

        private void EditProduct(object parameter)
        {
            if (parameter is ProductDto selectedProduct)
            {
                var editWindow = new EditProductWindow(selectedProduct);
                editWindow.ShowDialog();
                LoadProducts(); // обновляем
            }
        }

        private bool CanExecuteEditProduct(object parameter)
        {
            return parameter is ProductDto;
        }

        private void DeleteProduct(object parameter)
        {
            if (parameter is ProductDto selectedProduct)
            {
                _productModel.DeleteProduct(selectedProduct.Id);
                LoadProducts();
            }
        }

        private bool CanExecuteDeleteProduct(object parameter)
        {
            return parameter is ProductDto;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
