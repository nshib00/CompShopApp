using DAL;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DTO
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public int StatusId { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }

        public OrderDto() { }

        public OrderDto(Order order)
        {
            Id = order.Id;
            CustomerId = order.CustomerId;
            OrderDate = order.OrderDate;
            StatusId = order.StatusId;
            OrderDetails = order.OrderDetails?.Select(od => new OrderDetailDto(od)).ToList();
        }
    }

    public class OrderCreateDto
    {
        public int CustomerId { get; set; }
        public List<OrderDetailDto> Items { get; set; }
    }
}
