using DAL.Entities;

namespace BLL.DTO
{
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;

        public OrderDetailDto() { }

        public OrderDetailDto(OrderDetail orderDetail)
        {
            Id = orderDetail.Id;
            ProductId = orderDetail.ProductId;
            ProductName = orderDetail.Product?.Name;
            Quantity = orderDetail.Quantity;
            UnitPrice = orderDetail.UnitPrice;
        }
    }
}
