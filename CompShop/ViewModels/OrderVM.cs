using BLL.DTO;
using ComputerShop.Commands;
using ComputerShop.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

public class OrderVM : INotifyPropertyChanged
{
    private readonly OrderModel _orderModel = new OrderModel();
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

    public OrderVM(ObservableCollection<OrderDetailDto> items, int userId)
    {
        OrderDetails = items;
        ConfirmOrderCommand = new RelayCommand(_ => ConfirmOrder(userId));
        BackCommand = new RelayCommand(_ => CloseWindowRequested?.Invoke());
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

        MessageBox.Show("Заказ успешно оформлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
