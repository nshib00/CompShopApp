using DAL.Entities;
using System.Collections.Generic;
using System.Linq;

public class CartDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public List<CartItemDto> Items { get; set; }

    public CartDto() { }

    public CartDto(Cart cart)
    {
        Id = cart.Id;
        CustomerId = cart.CustomerId;
        Items = cart.Items?.Select(p => new CartItemDto(p)).ToList();
    }

}