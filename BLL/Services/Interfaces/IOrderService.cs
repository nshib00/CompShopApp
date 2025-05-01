using BLL.DTO;

namespace BLL.Services.Interfaces
{
    public interface IOrderService
    {
        void CreateOrder(OrderCreateDto dto);
        List<OrderDto> GetOrdersByCustomer(int customerId);
    }
}
