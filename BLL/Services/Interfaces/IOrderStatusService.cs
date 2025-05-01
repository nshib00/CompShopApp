using BLL.DTO;

namespace BLL.Services.Interfaces
{
    public interface IOrderStatusService
    {
        List<OrderStatusDto> GetOrderStatuses();
    }
}
