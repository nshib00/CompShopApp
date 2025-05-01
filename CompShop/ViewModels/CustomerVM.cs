using BLL.DTO;
using BLL.Services;
using BLL.Services.Interfaces;
using ComputerShop.Commands;
using ComputerShop.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

public class CustomerVM : INotifyPropertyChanged
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly ICartService _cartService;
    private ObservableCollection<ProductDto> _products;
    private ObservableCollection<CategoryDto> _categories;
    private CategoryDto _selectedCategory;
    private string _searchText;
    private UserDto _currentUser;

    public event Action CartUpdated;

    private int _cartItemCount;
    public int CartItemCount
    {
        get => _cartItemCount;
        set
        {
            if (_cartItemCount != value)
            {
                _cartItemCount = value;
                OnPropertyChanged(nameof(CartItemCount));
            }
        }
    }

    public string UserName => _currentUser.FirstName;

    public ICommand LogoutCommand { get; }
    public ICommand AddToCartCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand OpenCartCommand { get; }

    public CustomerVM(UserDto user)
    {
        _currentUser = user;
        _productService = new ProductService();
        _categoryService = new CategoryService();
        _cartService = new CartService();

        LogoutCommand = new RelayCommand(Logout);
        AddToCartCommand = new RelayCommand(AddToCart);
        SearchCommand = new RelayCommand(ExecuteSearch);
        OpenCartCommand = new RelayCommand(OpenCart);

        LoadCategories();
        LoadProducts();
        UpdateCartItemCount();

        CartUpdated += UpdateCartItemCount;
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

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                LoadProducts();
            }
        }
    }

    public void LoadCategories()
    {
        var allCategories = new CategoryDto { Id = 0, Name = "Все категории" };
        var categoriesWithAll = new ObservableCollection<CategoryDto> { allCategories };

        foreach (var cat in _categoryService.GetCategoriesWithData())
            categoriesWithAll.Add(cat);

        Categories = categoriesWithAll;
    }

    public void LoadProducts()
    {
        if (SelectedCategory == null && string.IsNullOrEmpty(SearchText))
        {
            Products = new ObservableCollection<ProductDto>(_productService.GetAllProducts());
        }
        else
        {
            Products = new ObservableCollection<ProductDto>(_productService.GetProductsBySearch(SearchText, SelectedCategory?.Id));
        }
    }

    private void UpdateCartItemCount()
    {
        var cart = _cartService.GetCartByUserId(_currentUser.Id);
        CartItemCount = cart?.Items.Sum(item => item.Quantity) ?? 0;
        OnPropertyChanged(nameof(CartItemCount));
    }

    private void OpenCart(object parameter)
    {
        if (_currentUser != null)
        {
            var cartViewModel = new CartVM(_currentUser.Id);
            cartViewModel.CartUpdated += UpdateCartItemCount;
            var cartWindow = new CartWindow(cartViewModel);

            cartWindow.Closed += (s, e) => UpdateCartItemCount();
            cartWindow.ShowDialog();
        }
    }

    public void Logout(object parameter)
    {
        _currentUser = null;
        Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is CartWindow)?.Close();
        new MainWindow().Show();
        Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is Window)?.Close();
    }

    public void AddToCart(object parameter)
    {
        var product = parameter as ProductDto;
        if (product == null) return;

        var cartId = _cartService.GetCartByUserId(_currentUser.Id)?.Id ?? _cartService.CreateCart(_currentUser.Id);
        _cartService.AddProductToCart(cartId, product, 1);

        UpdateCartItemCount();
    }

    private void ExecuteSearch(object parameter) { }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
