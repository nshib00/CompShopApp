using BLL.DTO;
using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComputerShop.Models
{
    public class OrderModel
    {
        private readonly AppDbContext _context = new AppDbContext();

        public void CreateOrder(OrderCreateDto dto)
        {
            var order = new Order
            {
                CustomerId = dto.CustomerId,
                OrderDate = DateTime.Now,
                StatusId = 1, // например: "Ожидает подтверждения"
                DeliveryCost = dto.DeliveryCost,
                OrderDetails = dto.Items.Select(item => new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList(),
                Delivery = new Delivery
                {
                    Address = dto.DeliveryAddress
                }
            };

            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public List<OrderDto> GetOrdersByCustomer(int customerId)
        {
            var orders = _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Status)
                .Include(o => o.Delivery)
                .Where(o => o.CustomerId == customerId)
                .ToList();

            return orders.Select(o => new OrderDto(o)).ToList();
        }
    }
}
