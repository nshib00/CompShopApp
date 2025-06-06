﻿using BLL.Services.Interfaces;
using CompShop.Services.Interfaces;
using ComputerShop.Commands;
using ComputerShop.Views;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ComputerShop.ViewModels
{
    public class CartVM : INotifyPropertyChanged
    {
        private readonly ICartService _cartService;
        private readonly IDialogService _dialogService;

        private ObservableCollection<CartItemDto> _cartItems;
        private double _totalCartPrice;
        private int _currentUserId;
        private int _cartId;

        public event Action CartUpdated;

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

        public ICommand ContinueShoppingCommand { get; }
        public ICommand CheckoutCommand { get; }

        public CartVM(ICartService cartService, IDialogService dialogService)
        {
            _cartService = cartService;
            _dialogService = dialogService;

            ContinueShoppingCommand = new RelayCommand(ContinueShopping);
            CheckoutCommand = new RelayCommand(Checkout);
        }

        public void Initialize(int userId)
        {
            _currentUserId = userId;
            LoadCart();
        }

        public void LoadCart()
        {
            var cart = _cartService.GetCartByUserId(_currentUserId);

            if (cart != null)
            {
                _cartId = cart.Id;
                CartItems = new ObservableCollection<CartItemDto>(cart.Items);
                InitItemCommands();
            }
        }

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
                    _cartService.IncreaseProductQuantity(_cartId, item.ProductId, 1);
                });

                item.DecreaseQuantityCommand = new RelayCommand(_ =>
                {
                    if (item.Quantity > 1)
                    {
                        item.Quantity--;
                        _cartService.DecreaseProductQuantity(_cartId, item.ProductId, 1);
                    }
                });

                item.RemoveItemCommand = new RelayCommand(_ =>
                {
                    _cartService.RemoveProductFromCart(_cartId, item.ProductId);
                    CartItems.Remove(item);
                    RecalculateTotal();
                });
            }
        }

        private void OnCartItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CartItemDto.Quantity) || e.PropertyName == nameof(CartItemDto.TotalPrice))
            {
                RecalculateTotal();
            }
            CartUpdated?.Invoke();
        }

        private void OnCartItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (CartItemDto oldItem in e.OldItems)
                {
                    oldItem.PropertyChanged -= OnCartItemPropertyChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (CartItemDto newItem in e.NewItems)
                {
                    newItem.PropertyChanged += OnCartItemPropertyChanged;

                    newItem.IncreaseQuantityCommand = new RelayCommand(_ =>
                    {
                        newItem.Quantity++;
                        _cartService.IncreaseProductQuantity(_cartId, newItem.ProductId, 1);
                    });

                    newItem.DecreaseQuantityCommand = new RelayCommand(_ =>
                    {
                        if (newItem.Quantity > 1)
                        {
                            newItem.Quantity--;
                            _cartService.DecreaseProductQuantity(_cartId, newItem.ProductId, 1);
                        }
                    });

                    newItem.RemoveItemCommand = new RelayCommand(_ =>
                    {
                        _cartService.RemoveProductFromCart(_cartId, newItem.ProductId);
                        CartItems.Remove(newItem);
                        RecalculateTotal();
                    });
                }
            }
            CartUpdated?.Invoke();
            RecalculateTotal();
        }

        private void RecalculateTotal()
        {
            TotalCartPrice = CartItems?.Sum(item => item.Price * item.Quantity) ?? 0;
        }

        private void ContinueShopping(object obj)
        {
            _dialogService.CloseDialog();
        }

        private void Checkout(object obj)
        {
            if (CartItems == null || CartItems.Count == 0)
            {
                MessageBox.Show("Ваша корзина пуста. Добавьте товары перед оформлением заказа.", "Оформление заказа", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var orderWindow = new OrderWindow(_currentUserId);
            orderWindow.Show();
            _dialogService.CloseDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
