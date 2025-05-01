using BLL.DTO;
using BLL.Services.Interfaces;
using DAL.Context;

namespace BLL.Services
{
    public class OrderStatusService : IOrderStatusService
    {
        private readonly AppDbContext _context = new AppDbContext();

        public List<OrderStatusDto> GetOrderStatuses()
        {
            var statuses = _context.OrderStatuses.ToList();
            return statuses.Select(s => new OrderStatusDto(s)).ToList();
        }
    }
}
