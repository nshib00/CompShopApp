using BLL.DTO;
using BLL.Services.Interfaces;
using ComputerShop.Commands;
using CompShop.Services.Interfaces;
using ComputerShop.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

public class OrderVM : INotifyPropertyChanged
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;
    private readonly IDialogService _dialogService;

    private ObservableCollection<OrderDetailDto> _orderDetails;
    private string _deliveryAddress;
    private decimal _totalAmount;
    private int _currentUserId;

    // Событие для закрытия OrderWindow
    public event Action OrderSuccessClosed;

    // Public свойства
    public ObservableCollection<OrderDetailDto> OrderDetails
    {
        get => _orderDetails;
        set
        {
            _orderDetails = value;
            OnPropertyChanged(nameof(OrderDetails));
            RecalculateTotal();
        }
    }

    public string DeliveryAddress
    {
        get => _deliveryAddress;
        set
        {
            _deliveryAddress = value;
            OnPropertyChanged(nameof(DeliveryAddress));
        }
    }

    public decimal TotalAmount
    {
        get => _totalAmount;
        private set
        {
            _totalAmount = value;
            OnPropertyChanged(nameof(TotalAmount));
        }
    }

    // Команды
    public ICommand ConfirmOrderCommand { get; }
    public ICommand BackCommand { get; }
    public ICommand ReturnToCatalogCommand { get; }

    // Конструктор (с DI)
    public OrderVM(IOrderService orderService, ICartService cartService, IDialogService dialogService)
    {
        _orderService = orderService;
        _cartService = cartService;
        _dialogService = dialogService;

        ConfirmOrderCommand = new RelayCommand(_ => ConfirmOrder());
        BackCommand = new RelayCommand(_ => _dialogService.CloseDialog());
        ReturnToCatalogCommand = new RelayCommand(_ => CloseOrderSuccess());
    }

    public void Initialize(int userId)
    {
        _currentUserId = userId;
        LoadCartItemsAsOrderDetails();
    }

    private void LoadCartItemsAsOrderDetails()
    {
        var cart = _cartService.GetCartByUserId(_currentUserId);
        if (cart != null)
        {
            var details = cart.Items.Select(item => new OrderDetailDto
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = (decimal)item.Price,
            });

            OrderDetails = new ObservableCollection<OrderDetailDto>(details);
        }
    }

    private void ConfirmOrder()
    {
        if (string.IsNullOrWhiteSpace(DeliveryAddress))
        {
            MessageBox.Show("Введите адрес доставки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (OrderDetails == null || !OrderDetails.Any())
        {
            MessageBox.Show("Корзина пуста.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var order = new OrderCreateDto
        {
            CustomerId = _currentUserId,
            DeliveryAddress = DeliveryAddress,
            DeliveryCost = new Random().Next(200, 700),
            Items = OrderDetails.ToList()
        };

        _orderService.CreateOrder(order);

        // Показать окно успеха с правильным DataContext
        var successWindow = new OrderSuccess
        {
            DataContext = this
        };
        successWindow.Show();

        // Закрыть окно оформления заказа
        _dialogService.CloseDialog();
    }

    private void CloseOrderSuccess()
    {
        // Вызвать событие (закрытие окна OrderSuccess)
        OrderSuccessClosed?.Invoke();

        // Найти и закрыть окно OrderSuccess
        var window = Application.Current.Windows
            .OfType<OrderSuccess>()
            .FirstOrDefault();

        window?.Close();
    }

    private void RecalculateTotal()
    {
        TotalAmount = OrderDetails?.Sum(x => x.TotalPrice) ?? 0;
    }

    // INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
