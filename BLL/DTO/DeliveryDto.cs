using DAL.Entities;

namespace BLL.DTO
{
    public class DeliveryDto
    {
        public string Address { get; set; }
        public DateTime? DeliveryDate { get; set; }

        public DeliveryDto() { }

        public DeliveryDto(Delivery delivery)
        {
            Address = delivery.Address;
            DeliveryDate = delivery.DeliveryDate;
        }
    }
}
