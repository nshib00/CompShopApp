using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ComputerShop.Models;
using ComputerShop.Commands;

namespace ComputerShop.ViewModels
{
    public class CartItemVM : INotifyPropertyChanged
    {
        private readonly CartModel _model;
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

        public CartItemVM(CartModel model, int cartId, CartItemDto dto)
        {
            _model = model;
            _cartId = cartId;

            ProductId = dto.ProductId;
            ProductName = dto.ProductName;
            Price = (decimal)dto.Price;
            Quantity = dto.Quantity;

            IncreaseQuantityCommand = new RelayCommand(_ =>
            {
                _model.IncreaseProductQuantity(_cartId, ProductId, 1);
                Quantity += 1;
            });

            DecreaseQuantityCommand = new RelayCommand(_ =>
            {
                if (Quantity > 1)
                {
                    _model.IncreaseProductQuantity(_cartId, ProductId, -1);
                    Quantity -= 1;
                }
            });

            RemoveItemCommand = new RelayCommand(_ =>
            {
                _model.RemoveProductFromCart(_cartId, ProductId);
                OnRemoveRequested?.Invoke(this);
            });
        }

        public event Action<CartItemVM>? OnRemoveRequested;
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
