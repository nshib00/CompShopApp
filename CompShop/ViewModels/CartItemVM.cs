using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ComputerShop.Commands;
using BLL.Services.Interfaces;

namespace ComputerShop.ViewModels
{
    public class CartItemVM : INotifyPropertyChanged
    {
        private readonly ICartService _cartService;
        private readonly int _cartId;
        public int ProductId { get; }
        public string ProductName { get; }
        public decimal Price { get; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }

        public decimal TotalPrice => Price * Quantity;

        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand RemoveItemCommand { get; }

        public ICartService CartService => _cartService;

        public CartItemVM(ICartService cartService, int cartId, CartItemDto dto)
        {
            _cartService = cartService;
            _cartId = cartId;

            ProductId = dto.ProductId;
            ProductName = dto.ProductName;
            Price = (decimal)dto.Price;
            Quantity = dto.Quantity;

            IncreaseQuantityCommand = new RelayCommand(_ =>
            {
                _cartService.IncreaseProductQuantity(_cartId, ProductId, 1);
                Quantity += 1;
            });

            DecreaseQuantityCommand = new RelayCommand(_ =>
            {
                if (Quantity > 1)
                {
                    _cartService.IncreaseProductQuantity(_cartId, ProductId, -1);
                    Quantity -= 1;
                }
            });

            RemoveItemCommand = new RelayCommand(_ =>
            {
                _cartService.RemoveProductFromCart(_cartId, ProductId);
                OnRemoveRequested?.Invoke(this);
            });
        }

        public event Action<CartItemVM>? OnRemoveRequested;
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
