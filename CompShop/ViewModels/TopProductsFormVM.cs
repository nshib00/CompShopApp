using BLL.Services;
using BLL.Services.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CompShop.ViewModels
{
    public class TopProductsFormVM : INotifyPropertyChanged
    {
        private DateTime? _startDate;
        public DateTime? TopSalesStartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(TopSalesStartDate));
            }
        }

        private DateTime? _endDate;
        public DateTime? TopSalesEndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(TopSalesEndDate));
            }
        }

        private int _topCount;
        public int TopCount
        {
            get => _topCount;
            set
            {
                _topCount = value;
                OnPropertyChanged(nameof(TopCount));
            }
        }
        public ObservableCollection<TopProductReport> TopProductReports { get; set; } = new ObservableCollection<TopProductReport>();

        private readonly IReportService _reportService = new ReportService();

        public TopProductsFormVM()
        {
            TopCount = 10;
        }

        public void GenerateTopProductReport()
        {
            if (TopSalesStartDate == null || TopSalesEndDate == null)
                return;

            var topProducts = _reportService.GetTopSellingProducts(TopSalesStartDate.Value, TopSalesEndDate.Value, TopCount);

            var topProductReportData = topProducts.Select(tp => new TopProductReport
            {
                ProductName = tp.ProductName,
                SoldQuantity = tp.TotalSold,
                TotalAmount = tp.TotalAmount
            }).ToList();

            TopProductReports.Clear();
            foreach (var report in topProductReportData)
            {
                TopProductReports.Add(report);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class TopProductReport
    {
        public string ProductName { get; set; }
        public int SoldQuantity { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
