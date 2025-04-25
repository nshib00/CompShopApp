using BLL.DTO;
using ComputerShop.Commands;
using ComputerShop.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using ComputerShop.Views;

public class OrderVM : INotifyPropertyChanged
{
    private readonly OrderModel _orderModel = new OrderModel();
    private readonly CartModel _cartModel = new CartModel(); // добавить модель корзины

    private ObservableCollection<OrderDetailDto> _orderDetails;
    private string _deliveryAddress;
    private decimal _totalAmount;

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

    public ICommand ConfirmOrderCommand { get; }
    public ICommand BackCommand { get; }

    public event Action CloseWindowRequested;

    public OrderVM(int userId)
    {
        LoadCartItemsAsOrderDetails(userId); // подгрузка из корзины

        ConfirmOrderCommand = new RelayCommand(_ => ConfirmOrder(userId));
        BackCommand = new RelayCommand(_ => CloseWindowRequested?.Invoke());
    }

    private void LoadCartItemsAsOrderDetails(int userId)
    {
        var cart = _cartModel.GetCartByUserId(userId);
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

    private void ConfirmOrder(int userId)
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
            CustomerId = userId,
            DeliveryAddress = DeliveryAddress,
            DeliveryCost = (new Random().Next() % 500) + 200,
            Items = OrderDetails.ToList()
        };

        _orderModel.CreateOrder(order);

        var successWindow = new OrderSuccess();
        successWindow.Show();

        CloseWindowRequested?.Invoke();
    }


    private void RecalculateTotal()
    {
        TotalAmount = OrderDetails?.Sum(x => x.TotalPrice) ?? 0;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
