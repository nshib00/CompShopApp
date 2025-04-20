using DAL.Entities;

public class CartItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public CartItemDto() { }

    public CartItemDto(CartItem cartItem)
    {
        ProductId = cartItem.ProductId;
        ProductName = cartItem.Product.Name;
        Price = cartItem.Product.Price;
        Quantity = cartItem.Quantity;
    }
}