using DAL.Entities;

namespace BLL.DTO
{
    public class OrderDto
    {
        public int Id { get; set; }
        public decimal DeliveryCost { get; set; }
        public DateTime OrderDate { get; set; }
        public int StatusId { get; set; }
        public int CustomerId { get; set; }

        public string StatusName { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
        public DeliveryDto Delivery { get; set; }

        public OrderDto() { }

        public OrderDto(Order order)
        {
            Id = order.Id;
            DeliveryCost = order.DeliveryCost;
            OrderDate = order.OrderDate;
            StatusId = order.StatusId;
            CustomerId = order.CustomerId;
            StatusName = order.Status?.Name;
            OrderDetails = order.OrderDetails?.Select(od => new OrderDetailDto(od)).ToList();
            Delivery = order.Delivery != null ? new DeliveryDto(order.Delivery) : null;
        }
    }

    public class OrderCreateDto
    {
        public int CustomerId { get; set; }
        public decimal DeliveryCost { get; set; }
        public required string DeliveryAddress { get; set; }
        public List<OrderDetailDto> Items { get; set; }
    }
}
