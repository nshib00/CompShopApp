using BLL.DTO;

namespace BLL.Services.Interfaces
{
    public interface ICartService
    {
        int CreateCart(int customerId);
        CartDto? GetCartByUserId(int customerId);
        void AddProductToCart(int cartId, ProductDto productDto, int quantity);
        void IncreaseProductQuantity(int cartId, int productId, int increaseBy);
        void DecreaseProductQuantity(int cartId, int productId, int decreaseBy);
        void RemoveProductFromCart(int cartId, int productId);
        void DeleteCart(int id);
        CartDto? GetCartById(int id);
    }
}