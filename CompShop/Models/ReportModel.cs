using BLL.DTO;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace CompShop.Models
{
    public class ReportModel
    {
        private readonly AppDbContext _context = new AppDbContext();

        public List<OrderDto> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.ToUniversalTime();
            endDate = endDate.ToUniversalTime();

            var orders = _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Status)
                .Include(o => o.Delivery)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .ToList();

            return orders.Select(o => new OrderDto(o)).ToList();
        }

        public List<OrderDto> GetOrdersByDateRangeAndStatus(DateTime startDate, DateTime endDate, string statusName)
        {
            startDate = startDate.ToUniversalTime();
            endDate = endDate.ToUniversalTime();

            var orders = _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Status)
                .Include(o => o.Delivery)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .Where(o => o.Status != null && o.Status.Name == statusName)
                .ToList();

            return orders.Select(o => new OrderDto(o)).ToList();
        }

        public List<CategorySalesDto> GetSalesByCategories(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.ToUniversalTime();
            endDate = endDate.ToUniversalTime();

            var orderDetails = _context.OrderDetails
                .Include(od => od.Product)
                    .ThenInclude(p => p.Category)
                .Include(od => od.Order)
                .Where(od => od.Order.OrderDate >= startDate && od.Order.OrderDate <= endDate)
                .ToList();

            var salesByCategory = orderDetails
                .GroupBy(od => od.Product.Category.Name)
                .Select(g => new CategorySalesDto
                {
                    CategoryName = g.Key,
                    TotalSold = g.Sum(od => od.Quantity),
                    TotalAmount = g.Sum(od => od.Quantity * od.Product.Price)
                })
                .ToList();

            return salesByCategory;
        }


        public List<(string ProductName, int TotalSold, decimal TotalAmount)> GetTopSellingProducts(DateTime startDate, DateTime endDate, int topCount)
        {
            startDate = startDate.ToUniversalTime();
            endDate = endDate.ToUniversalTime();

            var topProducts = _context.OrderDetails
                .Include(od => od.Product)
                .Include(od => od.Order)
                .Where(od => od.Order.OrderDate >= startDate && od.Order.OrderDate <= endDate)
                .GroupBy(od => od.Product.Name)
                .Select(g => new
                {
                    ProductName = g.Key,
                    TotalSold = g.Sum(od => od.Quantity),
                    TotalAmount = g.Sum(od => od.Quantity * od.Product.Price)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(topCount)
                .ToList();

            return topProducts.Select(tp => (tp.ProductName, tp.TotalSold, tp.TotalAmount)).ToList();
        }


        public List<OrderStatusDto> GetOrderStatuses()
        {
            var statuses = _context.OrderStatuses.ToList();
            return statuses.Select(s => new OrderStatusDto(s)).ToList();
        }

        public List<CategoryDto> GetCategories()
        {
            var categories = _context.Categories.ToList();
            return categories.Select(c => new CategoryDto(c)).ToList();
        }
    }
}
