using DAL;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DTO
{
    public class CartDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<CartItemDto> Products { get; set; }

        public CartDto() { }

        public CartDto(Cart cart)
        {
            Id = cart.Id;
            CustomerId = cart.CustomerId;
            Products = cart.Products?.Select(p => new CartItemDto(p)).ToList();
        }
    }

    public class CartUpdateDto
    {
        public List<CartItemUpdateDto> Items { get; set; }
    }

}
