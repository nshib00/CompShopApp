using BLL.DTO;
using ComputerShop.Commands;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using BLL.Services;
using BLL.Services.Interfaces;

public class AddEditProductVM : INotifyPropertyChanged
{
    private readonly IProductService _productService = new ProductService();
    private readonly ICategoryService _categoryService = new CategoryService();
    private readonly IManufacturerService _manufacturerService = new ManufacturerService();

    public ObservableCollection<CategoryDto> Categories { get; } = new();
    public ObservableCollection<ManufacturerDto> Manufacturers { get; } = new();

    public ICommand SaveProductCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    public ICommand BrowseImageCommand { get; set; }

    private bool _isEditMode;
    private int _id;

    private string _name;
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    private string _description;
    public string Description
    {
        get => _description;
        set { _description = value; OnPropertyChanged(); }
    }

    private decimal? _price;
    public decimal? Price
    {
        get => _price;
        set { _price = value; OnPropertyChanged(); }
    }

    private int? _stockQuantity;
    public int? StockQuantity
    {
        get => _stockQuantity;
        set { _stockQuantity = value; OnPropertyChanged(); }
    }

    private int? _categoryId;
    public int? CategoryId
    {
        get => _categoryId;
        set { _categoryId = value; OnPropertyChanged(); }
    }

    private int? _manufacturerId;
    public int? ManufacturerId
    {
        get => _manufacturerId;
        set { _manufacturerId = value; OnPropertyChanged(); }
    }

    private string _imagePath;
    public string ImagePath
    {
        get => _imagePath;
        set { _imagePath = value; OnPropertyChanged(); }
    }

    public AddEditProductVM()
    {
        InitializeCommands();
        LoadData();
    }

    public AddEditProductVM(FullProductDto product) : this()
    {
        if (product != null)
        {
            _isEditMode = true;
            _id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            StockQuantity = product.StockQuantity;
            CategoryId = product.CategoryId;
            ManufacturerId = product.ManufacturerId;
            ImagePath = product.ImagePath ?? "";
        }
    }

    private void InitializeCommands()
    {
        SaveProductCommand = new RelayCommand(SaveProduct);
        CancelCommand = new RelayCommand(Cancel);
        BrowseImageCommand = new RelayCommand(BrowseImage);
    }

    private void LoadData()
    {
        Categories.Clear();
        foreach (var category in _categoryService.GetCategoriesWithData())
            Categories.Add(category);

        Manufacturers.Clear();
        foreach (var manufacturer in _manufacturerService.GetAllManufacturers())
            Manufacturers.Add(manufacturer);
    }

    private void SaveProduct(object obj)
    {
        var product = new FullProductDto
        {
            Id = _id,
            Name = Name,
            Description = Description,
            Price = Price ?? 0,
            StockQuantity = StockQuantity ?? 0,
            CategoryId = CategoryId,
            ManufacturerId = ManufacturerId,
            ImagePath = ImagePath
        };

        if (_isEditMode)
            _productService.UpdateProduct(product);
        else
            _productService.CreateProduct(product);

        MessageBox.Show("Товар успешно сохранён", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

        CloseWindow(obj as Window);
    }

    private void Cancel(object obj)
    {
        CloseWindow(obj as Window);
    }

    private void BrowseImage(object obj)
    {
        OpenFileDialog dialog = new OpenFileDialog
        {
            Filter = "Image files (*.png;*.jpg)|*.png;*.jpg"
        };
        if (dialog.ShowDialog() == true)
        {
            ImagePath = dialog.FileName;
        }
    }

    private void CloseWindow(Window window)
    {
        if (window != null)
            window.Close();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
