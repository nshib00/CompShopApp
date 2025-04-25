using ComputerShop.Commands;
using ComputerShop.Models;
using ComputerShop.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows;

public class CartVM : INotifyPropertyChanged
{
    private readonly CartModel _cartModel;
    private ObservableCollection<CartItemDto> _cartItems;
    private double _totalCartPrice;

    // Событие для закрытия окна
    public event Action CloseWindowRequested;

    // Свойство для списка товаров в корзине
    public ObservableCollection<CartItemDto> CartItems
    {
        get => _cartItems;
        set
        {
            if (_cartItems != value)
            {
                if (_cartItems != null)
                    _cartItems.CollectionChanged -= OnCartItemsChanged;

                _cartItems = value;

                if (_cartItems != null)
                    _cartItems.CollectionChanged += OnCartItemsChanged;

                InitItemCommands();
                OnPropertyChanged(nameof(CartItems));
                RecalculateTotal();
            }
        }
    }

    // Свойство для общей суммы корзины
    public double TotalCartPrice
    {
        get => _totalCartPrice;
        private set
        {
            if (_totalCartPrice != value)
            {
                _totalCartPrice = value;
                OnPropertyChanged(nameof(TotalCartPrice));
            }
        }
    }

    // Команды для продолжения покупок и оформления заказа
    public ICommand ContinueShoppingCommand { get; set; }
    public ICommand CheckoutCommand { get; set; }

    // Конструктор
    public CartVM(int userId)
    {
        _cartModel = new CartModel();
        LoadCart(userId);

        ContinueShoppingCommand = new RelayCommand(ContinueShopping);
        CheckoutCommand = new RelayCommand(Checkout);
    }

    // Метод для загрузки корзины
    public void LoadCart(int userId)
    {
        var cart = _cartModel.GetCartByUserId(userId);

        if (cart != null)
        {
            CartItems = new ObservableCollection<CartItemDto>(cart.Items);
            InitItemCommands();
        }
    }

    // Инициализация команд для каждого товара
    private void InitItemCommands()
    {
        if (CartItems == null) return;

        foreach (var item in CartItems)
        {
            item.PropertyChanged -= OnCartItemPropertyChanged;
            item.PropertyChanged += OnCartItemPropertyChanged;

            item.IncreaseQuantityCommand = new RelayCommand(_ =>
            {
                item.Quantity++;
            });

            item.DecreaseQuantityCommand = new RelayCommand(_ =>
            {
                if (item.Quantity > 1)
                    item.Quantity--;
            });

            item.RemoveItemCommand = new RelayCommand(_ =>
            {
                CartItems.Remove(item);
                RecalculateTotal();
            });
        }
    }

    // Метод для изменения количества товара в корзине
    private void OnCartItemPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CartItemDto.Quantity) || e.PropertyName == nameof(CartItemDto.TotalPrice))
        {
            RecalculateTotal();
        }
    }

    private void OnCartItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        // Отписка от удалённых элементов
        if (e.OldItems != null)
        {
            foreach (CartItemDto oldItem in e.OldItems)
            {
                oldItem.PropertyChanged -= OnCartItemPropertyChanged;
            }
        }

        // Подписка на новые элементы
        if (e.NewItems != null)
        {
            foreach (CartItemDto newItem in e.NewItems)
            {
                newItem.PropertyChanged += OnCartItemPropertyChanged;

                newItem.IncreaseQuantityCommand = new RelayCommand(_ =>
                {
                    newItem.Quantity++;
                });

                newItem.DecreaseQuantityCommand = new RelayCommand(_ =>
                {
                    if (newItem.Quantity > 1)
                        newItem.Quantity--;
                });

                newItem.RemoveItemCommand = new RelayCommand(_ =>
                {
                    CartItems.Remove(newItem);
                    RecalculateTotal();
                });
            }
        }

        // Обновляем общую сумму после изменений
        RecalculateTotal();
    }



    // Метод для пересчета общей суммы корзины
    public void RecalculateTotal()
    {
        if (CartItems != null)
            TotalCartPrice = CartItems.Sum(item => item.Price * item.Quantity);
        else
            TotalCartPrice = 0;
    }

    // Метод для продолжения покупок (закрытие окна корзины)
    private void ContinueShopping(object obj)
    {
        CloseWindowRequested?.Invoke();
    }

    // Метод для оформления заказа
    private void Checkout(object obj)
    {
        // Если корзина пуста, показать сообщение
        if (CartItems == null || CartItems.Count == 0)
        {
            MessageBox.Show("Ваша корзина пуста. Добавьте товары перед оформлением заказа.", "Оформление заказа", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        // Открыть окно оформления заказа
        var orderWindow = new OrderWindow(); // передаем список товаров
        orderWindow.Show();

        // Закрыть окно корзины после открытия окна заказа
        CloseWindowRequested?.Invoke();
    }

    // Событие изменения свойств
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
