using BLL.DTO;
using BLL.Services.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CompShop.ViewModels
{
    public class CategoryOrdersFormVM : INotifyPropertyChanged
    {
        private readonly IReportService _reportService;

        public CategoryOrdersFormVM(IReportService reportService)
        {
            _reportService = reportService;
        }

        private DateTime? _startDate;
        public DateTime? CategoryOrdersStartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(CategoryOrdersStartDate));
            }
        }

        private DateTime? _endDate;
        public DateTime? CategoryOrdersEndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(CategoryOrdersEndDate));
            }
        }

        public ObservableCollection<CategoryReportDto> CategoryReports { get; } = new();

        public void GenerateCategoryReport()
        {
            if (CategoryOrdersStartDate == null || CategoryOrdersEndDate == null)
                return;

            var sales = _reportService.GetSalesByCategories(CategoryOrdersStartDate.Value, CategoryOrdersEndDate.Value);

            var categoryReportData = sales.Select(s => new CategoryReportDto
            {
                CategoryName = s.CategoryName,
                TotalAmount = s.TotalAmount,
                SoldQuantity = s.TotalSold
            }).OrderByDescending(s => s.TotalAmount)
            .ToList();

            CategoryReports.Clear();
            foreach (var report in categoryReportData)
            {
                CategoryReports.Add(report);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
