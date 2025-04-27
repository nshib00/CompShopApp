using CompShop.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CompShop.ViewModels
{
    public class CategoryOrdersFormVM : INotifyPropertyChanged
    {
        private readonly ReportModel _reportModel = new ReportModel();

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

        // Список для хранения данных отчета
        public ObservableCollection<CategoryReport> CategoryReports { get; set; } = new ObservableCollection<CategoryReport>();

        public CategoryOrdersFormVM()
        {
            // Инициализация, если нужно что-то подгрузить при старте
        }

        // Генерация отчета по категориям
        public void GenerateCategoryReport()
        {
            if (CategoryOrdersStartDate == null || CategoryOrdersEndDate == null)
                return;

            // Получаем данные по продажам для всех категорий за выбранный период
            var sales = _reportModel.GetSalesByCategories(CategoryOrdersStartDate.Value, CategoryOrdersEndDate.Value);

            // Группируем по категории и суммируем проданное количество и сумму
            var categoryReportData = sales.Select(s => new CategoryReport
            {
                CategoryName = s.CategoryName,
                TotalAmount = s.TotalAmount,
                SoldQuantity = s.TotalSold
            }).ToList();

            // Обновляем данные отчета
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

    // Класс для представления отчета по категориям
    public class CategoryReport
    {
        public string CategoryName { get; set; }
        public decimal TotalAmount { get; set; }
        public int SoldQuantity { get; set; }
    }
}
