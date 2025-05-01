using DAL.Models;

namespace BLL.DTO
{
    public class OrderDetailDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice
        {
            get => Quantity * UnitPrice;
        }
        public Product? Product { get; set; }

        public OrderDetailDto() { }

        public OrderDetailDto(OrderDetail detail)
        {
            ProductId = detail.ProductId;
            Product = detail.Product;
            ProductName = detail.Product.Name;
            Quantity = detail.Quantity;
            UnitPrice = detail.UnitPrice;
        }
    }
}
