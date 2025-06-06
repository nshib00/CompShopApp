﻿using BLL.DTO;
using BLL.Services.Interfaces;
using DAL.Context;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context = new AppDbContext();

        public void CreateOrder(OrderCreateDto dto)
        {
            var order = new Order
            {
                CustomerId = dto.CustomerId,
                OrderDate = DateTime.UtcNow,
                StatusId = 1,
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

            foreach (var item in dto.Items)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    product.StockQuantity -= item.Quantity;

                    if (product.StockQuantity < 0)
                    {
                        product.StockQuantity = 0;
                    }

                    _context.Products.Update(product);
                }
            }

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
