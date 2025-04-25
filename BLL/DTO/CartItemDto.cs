using DAL.Entities;
using System.ComponentModel;
using System.Windows.Input;

public class CartItemDto
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
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(TotalPrice)); // Обновляем сумму для конкретного товара
            }
        }
    }

    public double TotalPrice => Price * Quantity; // Сумма для конкретного товара

    public ICommand IncreaseQuantityCommand { get; set; }
    public ICommand DecreaseQuantityCommand { get; set; }
    public ICommand RemoveItemCommand { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName) =>
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