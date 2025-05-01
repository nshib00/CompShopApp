using BLL.DTO;
using BLL.Services.Interfaces;
using DAL.Context;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context = new AppDbContext();

        public int CreateCart(int customerId)
        {
            var cart = new Cart
            {
                CustomerId = customerId,
                Items = new List<CartItem>()
            };

            _context.Carts.Add(cart);
            _context.SaveChanges();

            return cart.Id;
        }

        public CartDto? GetCartByUserId(int customerId)
        {
            var cart = _context.Carts
                .Include(c => c.Items) // Включаем коллекцию элементов корзины
                    .ThenInclude(ci => ci.Product) // Включаем связанные товары в каждом элементе корзины
                .FirstOrDefault(c => c.CustomerId == customerId);

            return cart != null ? new CartDto(cart) : null;
        }

        public void AddProductToCart(int cartId, ProductDto productDto, int quantity)
        {
            var cart = _context.Carts
                .Include(c => c.Items)
                .FirstOrDefault(c => c.Id == cartId);

            if (cart == null) return;

            var existingItem = cart.Items.FirstOrDefault(p => p.ProductId == productDto.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var product = _context.Products.Find(productDto.Id);
                if (product == null) return;

                var newItem = new CartItem
                {
                    Product = product,
                    Quantity = quantity,
                    CartId = cart.Id
                };

                cart.Items.Add(newItem);
            }

            _context.SaveChanges();
        }

        public void IncreaseProductQuantity(int cartId, int productId, int increaseBy)
        {
            var cartItem = _context.CartItems
                .FirstOrDefault(ci => ci.CartId == cartId && ci.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += increaseBy;
                _context.SaveChanges();
            }
        }

        public void DecreaseProductQuantity(int cartId, int productId, int decreaseBy)
        {
            var cartItem = _context.CartItems
                .FirstOrDefault(ci => ci.CartId == cartId && ci.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity -= decreaseBy;

                if (cartItem.Quantity <= 0)
                    return;

                _context.SaveChanges();
            }
        }


        public void RemoveProductFromCart(int cartId, int productId)
        {
            var item = _context.CartItems
                .FirstOrDefault(ci => ci.CartId == cartId && ci.ProductId == productId);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                _context.SaveChanges();
            }
        }

        public void DeleteCart(int id)
        {
            var cart = _context.Carts
                .Include(c => c.Items)
                .FirstOrDefault(c => c.Id == id);

            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.Items);
                _context.Carts.Remove(cart);
                _context.SaveChanges();
            }
        }

        public CartDto? GetCartById(int id)
        {
            var cart = _context.Carts
                .Include(c => c.Items.Select(p => p.Product))
                .FirstOrDefault(c => c.Id == id);

            return cart != null ? new CartDto(cart) : null;
        }
    }
}
