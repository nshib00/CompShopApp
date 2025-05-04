using BLL.DTO;
using BLL.Services.Interfaces;
using CompShop.Views;
using ComputerShop.Commands;
using ComputerShop.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace CompShop.ViewModels
{
    public class ProductVM : INotifyPropertyChanged
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        private ObservableCollection<ProductDto> _products;
        private ObservableCollection<CategoryDto> _categories;
        private CategoryDto _selectedCategory;
        private ProductDto _selectedProduct;

        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }

        public ProductVM(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;

            AddProductCommand = new RelayCommand(AddProduct, CanExecuteAddProduct);
            EditProductCommand = new RelayCommand(EditProduct, CanExecuteEditProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct, CanExecuteDeleteProduct);

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
                    LoadProducts();
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

        private void LoadCategories()
        {
            Categories = new ObservableCollection<CategoryDto>(_categoryService.GetCategoriesWithData());
        }

        private void LoadProducts()
        {
            if (SelectedCategory == null)
            {
                Products = new ObservableCollection<ProductDto>(_productService.GetAllProducts());
            }
            else
            {
                Products = new ObservableCollection<ProductDto>(_productService.GetProductsByCategory(SelectedCategory.Id));
            }
        }

        private void AddProduct(object parameter)
        {
            var addProductWindow = new AddProductWindow();
            addProductWindow.ShowDialog();
            LoadProducts();
        }

        private bool CanExecuteAddProduct(object parameter) => true;

        private void EditProduct(object parameter)
        {
            if (parameter is ProductDto selectedProduct)
            {
                var fullProduct = _productService.GetFullProductById(selectedProduct.Id);
                if (fullProduct != null)
                {
                    var editWindow = new EditProductWindow();
                    var vm = new AddEditProductVM(editWindow);
                    vm.SetProduct(fullProduct);
                    editWindow.DataContext = vm;

                    editWindow.ShowDialog();
                    LoadProducts();
                }
            }
        }



        private bool CanExecuteEditProduct(object parameter) => parameter is ProductDto;

        private void DeleteProduct(object parameter)
        {
            if (parameter is ProductDto selectedProduct)
            {
                _productService.DeleteProduct(selectedProduct.Id);
                LoadProducts();
            }
        }

        private bool CanExecuteDeleteProduct(object parameter) => parameter is ProductDto;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
