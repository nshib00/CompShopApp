using DAL.Entities;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

public class CartItemDto : INotifyPropertyChanged
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; }

    private int _quantity;
    public int Quantity
    {
        get => _quantity;
        set
        {
            if (_quantity != value && value >= 1)
            {
                _quantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalPrice)); // Обновляем сумму для конкретного товара
            }
        }
    }

    public double TotalPrice => Price * Quantity;

    public ICommand IncreaseQuantityCommand { get; set; }
    public ICommand DecreaseQuantityCommand { get; set; }
    public ICommand RemoveItemCommand { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public CartItemDto() { }

    public CartItemDto(CartItem cartItem)
    {
        ProductId = cartItem.ProductId;
        ProductName = cartItem.Product.Name;
        Price = (double)cartItem.Product.Price;
        Quantity = cartItem.Quantity;
    }
}
