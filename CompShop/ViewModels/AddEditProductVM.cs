using BLL.DTO;
using BLL.Services;
using BLL.Services.Interfaces;
using ComputerShop.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace CompShop.ViewModels
{
    public class AddEditProductVM : INotifyPropertyChanged
    {
        private readonly IProductService _productService = new ProductService();
        private readonly ICategoryService _categoryService = new CategoryService();
        private readonly IManufacturerService _manufacturerService = new ManufacturerService();
        private readonly Window _window;

        public ObservableCollection<CategoryDto> Categories { get; } = new();
        public ObservableCollection<ManufacturerDto> Manufacturers { get; } = new();

        public event Action OnProductSaved;

        private int _id;
        private string _name;
        private string _description;
        private decimal? _price;
        private int? _stockQuantity;
        private int? _categoryId;
        private int? _manufacturerId;
        private string _imagePath;

        public bool IsEditMode { get; set; }

        public AddEditProductVM(Window window)
        {
            _window = window;
            SaveProductCommand = new RelayCommand(SaveProduct);
            CancelCommand = new RelayCommand(_ => _window.Close());
            BrowseImageCommand = new RelayCommand(BrowseImage);

            LoadCategories();
            LoadManufacturers();
        }

        public void SetProduct(FullProductDto dto)
        {
            if (dto == null) return;

            Id = dto.Id;
            Name = dto.Name;
            Description = dto.Description;
            Price = dto.Price;
            StockQuantity = dto.StockQuantity;
            CategoryId = dto.CategoryId;
            ManufacturerId = dto.ManufacturerId;
            ImagePath = dto.ImagePath;
            IsEditMode = true;
        }

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public decimal? Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(); }
        }

        public int? StockQuantity
        {
            get => _stockQuantity;
            set { _stockQuantity = value; OnPropertyChanged(); }
        }

        public int? CategoryId
        {
            get => _categoryId;
            set { _categoryId = value; OnPropertyChanged(); }
        }

        public int? ManufacturerId
        {
            get => _manufacturerId;
            set { _manufacturerId = value; OnPropertyChanged(); }
        }

        public string ImagePath
        {
            get => _imagePath;
            set { _imagePath = value; OnPropertyChanged(); }
        }

        public ICommand SaveProductCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BrowseImageCommand { get; }

        private void LoadCategories()
        {
            Categories.Clear();
            foreach (var category in _categoryService.GetCategoriesWithData())
                Categories.Add(category);
        }

        private void LoadManufacturers()
        {
            Manufacturers.Clear();
            foreach (var manufacturer in _manufacturerService.GetAllManufacturers())
                Manufacturers.Add(manufacturer);
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Пожалуйста, укажите название товара",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (Price == null || Price <= 0)
            {
                MessageBox.Show("Пожалуйста, укажите корректную цену товара",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (StockQuantity == null || StockQuantity < 0)
            {
                MessageBox.Show("Пожалуйста, укажите корректное количество товара",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (CategoryId == null)
            {
                MessageBox.Show("Пожалуйста, выберите категорию товара",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (ManufacturerId == null)
            {
                MessageBox.Show("Пожалуйста, выберите производителя товара",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void SaveProduct(object obj)
        {
            if (!ValidateFields())
                return;

            var dto = new FullProductDto
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Price = Price ?? 0,
                StockQuantity = StockQuantity ?? 0,
                CategoryId = CategoryId,
                ManufacturerId = ManufacturerId,
                ImagePath = ImagePath
            };

            try
            {
                if (IsEditMode)
                {
                    _productService.UpdateProduct(dto);
                    MessageBox.Show("Товар успешно обновлен!",
                                 "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _productService.CreateProduct(dto);
                    MessageBox.Show("Товар успешно добавлен!",
                                 "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                OnProductSaved?.Invoke();
                _window.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении товара: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BrowseImage(object obj)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg"
            };

            if (dialog.ShowDialog() == true)
            {
                ImagePath = dialog.FileName;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
