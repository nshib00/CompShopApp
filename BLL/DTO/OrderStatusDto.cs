using DAL.Models;

namespace BLL.DTO
{
    public class OrderStatusDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public OrderStatusDto(OrderStatus status)
        {
            Id = status.Id;
            Name = status.Name;
        }
    }
}
