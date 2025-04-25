using BLL.DTO;
using BLL.Model;
using ComputerShop.Commands;
using ComputerShop.Models;
using ComputerShop.ViewModels;
using ComputerShop.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

public class CustomerVM : INotifyPropertyChanged
{
    private readonly ProductModel _productModel;
    private readonly CategoryModel _categoryModel;
    private readonly CartModel _cartModel; // Для работы с корзиной
    private ObservableCollection<ProductDto> _products;
    private ObservableCollection<CategoryDto> _categories;
    private CategoryDto _selectedCategory;
    private string _searchText;
    private UserDto? _currentUser;

    // Свойства для отображения информации в UI
    private int _cartItemCount;
    public int CartItemCount
    {
        get => _cartItemCount;
        set
        {
            if (_cartItemCount != value)
            {
                _cartItemCount = value;
                OnPropertyChanged(nameof(CartItemCount));  // Уведомление об изменении
            }
        }
    }

    public string UserName => _currentUser.FirstName; // Предположим, что имя хранится в FirstName

    public ICommand LogoutCommand { get; }
    public ICommand AddToCartCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand OpenCartCommand { get; }

    public CustomerVM(UserDto user)
    {
        _currentUser = user;
        _productModel = new ProductModel();
        _categoryModel = new CategoryModel();
        _cartModel = new CartModel();

        LogoutCommand = new RelayCommand(Logout);
        AddToCartCommand = new RelayCommand(AddToCart);
        SearchCommand = new RelayCommand(ExecuteSearch);
        OpenCartCommand = new RelayCommand(OpenCart);

        LoadCategories();
        LoadProducts();
        UpdateCartItemCount(); // Обновление количества товаров в корзине
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
        var allCategories = new CategoryDto
        {
            Id = 0,
            Name = "Все категории"
        };

        var categoriesWithAll = new ObservableCollection<CategoryDto> { allCategories };

        foreach (var cat in _categoryModel.GetAllCategories())
            categoriesWithAll.Add(cat);

        Categories = categoriesWithAll;
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

    private void UpdateCartItemCount()
    {
        // Получаем корзину для текущего пользователя
        var cart = _cartModel.GetCartByUserId(_currentUser.Id);

        // Если корзина существует, то рассчитываем сумму всех количеств товаров
        if (cart?.Items != null)
        {
            CartItemCount = cart.Items.Sum(item => item.Quantity); // Суммируем все значения Quantity
        }
        else
        {
            CartItemCount = 0; // Если корзина пуста, устанавливаем 0
        }

        // Уведомление об изменении значения
        OnPropertyChanged(nameof(CartItemCount));
    }


    private void OpenCart(object parameter)
    {
        var cartViewModel = new CartVM(_currentUser.Id);
        var cartWindow = new CartWindow(cartViewModel);
        cartWindow.ShowDialog();
    }

    public void Logout(object parameter)
    {
        // Очистка текущего пользователя
        _currentUser = null;

        // Логика для закрытия текущего окна (если нужно)
        Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is CartWindow)?.Close();  // Закрыть корзину, если она открыта

        // Возвращаем на экран входа (MainWindow)
        var loginWindow = new MainWindow();
        loginWindow.Show();

        // Закрываем текущее окно (предполагается, что оно было открыто после логина)
        var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is Window);
        currentWindow?.Close();
    }

    public void AddToCart(object parameter)
    {
        var product = parameter as ProductDto;

        if (product == null) return;

        // Получаем текущую корзину для пользователя (если она существует)
        var cartId = _cartModel.GetCartByUserId(_currentUser.Id)?.Id;
        if (cartId == null)
        {
            // Если корзина не найдена, создаем новую
            cartId = _cartModel.CreateCart(_currentUser.Id);
        }

        // Добавляем товар в корзину
        _cartModel.AddProductToCart(cartId.Value, product, 1); // Добавляем товар с количеством 1

        // Обновляем количество товаров в корзине
        UpdateCartItemCount();
    }

    private void ExecuteSearch(object parameter)
    {
        // Логика поиска
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
