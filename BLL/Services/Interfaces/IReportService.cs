using BLL.DTO;

namespace BLL.Services.Interfaces
{
    public interface IReportService
    {
        List<OrderDto> GetOrdersByDateRange(DateTime startDate, DateTime endDate);

        List<OrderDto> GetOrdersByDateRangeAndStatus(DateTime startDate, DateTime endDate, string statusName);

        List<CategorySalesDto> GetSalesByCategories(DateTime startDate, DateTime endDate);

        List<(string ProductName, int TotalSold, decimal TotalAmount)> GetTopSellingProducts(
            DateTime startDate,
            DateTime endDate,
            int topCount);
    }
}
