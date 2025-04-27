using BLL.DTO;
using CompShop.Models;
using CompShop.Utils;
using CompShop.Views.Admin.Reports;
using ComputerShop.Commands;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CompShop.ViewModels
{
    public class ReportVM : INotifyPropertyChanged
    {
        private readonly ReportModel _reportModel = new ReportModel();

        private ObservableCollection<object> _reportData;
        public ObservableCollection<object> ReportData
        {
            get => _reportData;
            set
            {
                _reportData = value;
                OnPropertyChanged(nameof(ReportData));
            }
        }

        public ICommand GenerateTotalOrdersReportCommand { get; }
        public ICommand GenerateCategoryOrdersReportCommand { get; }
        public ICommand GenerateTopProductsReportCommand { get; }
        public ICommand ExportReportToPdfCommand { get; }

        public ReportVM()
        {
            GenerateTotalOrdersReportCommand = new RelayCommand(
                execute: (param) => OpenTotalOrdersForm(),
                canExecute: (param) => true
            );

            GenerateCategoryOrdersReportCommand = new RelayCommand(
                execute: (param) => OpenCategoryOrdersForm(),
                canExecute: (param) => true
            );

            GenerateTopProductsReportCommand = new RelayCommand(
                execute: (param) => OpenTopProductsForm(),
                canExecute: (param) => true
            );

            ExportReportToPdfCommand = new RelayCommand(
                execute: (param) => ExportToPdf(),
                canExecute: (param) => ReportData != null && ReportData.Any()
            );

            ReportData = new ObservableCollection<object>();
        }

        private void OpenTotalOrdersForm()
        {
            var window = new TotalOrdersForm();
            if (window.ShowDialog() == true)
            {
                LoadTotalSalesReport(window.ReportParameters);
            }
        }

        private void OpenCategoryOrdersForm()
        {
            var window = new CategoryOrdersForm();
            if (window.ShowDialog() == true)
            {
                LoadCategorySalesReport(window.ReportParameters);
            }
        }

        private void OpenTopProductsForm()
        {
            var window = new TopProductsForm();
            if (window.ShowDialog() == true)
            {
                LoadTopSalesReport(window.ReportParameters);
            }
        }

        private void LoadTotalSalesReport(TotalOrdersDto parameters)
        {
            List<OrderDto> orders;

            if (string.IsNullOrEmpty(parameters.SelectedStatus))
            {
                orders = _reportModel.GetOrdersByDateRange(parameters.StartDate, parameters.EndDate);
            }
            else
            {
                orders = _reportModel.GetOrdersByDateRangeAndStatus(parameters.StartDate, parameters.EndDate, parameters.SelectedStatus);
            }

            var report = orders.Select(o => new
            {
                OrderNumber = o.Id,
                OrderDate = o.OrderDate.ToShortDateString(),
                Status = o.StatusName,
                OrderAmount = o.OrderDetails.Sum(d => d.TotalPrice) + o.DeliveryCost
            }).ToList();

            ReportData = new ObservableCollection<object>(report);
        }


        private void LoadCategorySalesReport(CategoryOrdersDto parameters)
        {
            // Получаем данные о продажах по категориям за указанный период
            var sales = _reportModel.GetSalesByCategories(parameters.StartDate, parameters.EndDate);

            // Формируем отчет, группируя данные по категориям и вычисляя нужные агрегаты
            var categoryReport = sales.Select(s => new
            {
                Category = s.CategoryName,
                SoldQuantity = s.TotalSold,
                TotalAmount = s.TotalAmount
            }).ToList();

            // Сохраняем отчет в коллекцию данных для отображения
            ReportData = new ObservableCollection<object>(categoryReport);
        }



        private void LoadTopSalesReport(TopProductsDto parameters)
        {
            var topProducts = _reportModel.GetTopSellingProducts(parameters.StartDate, parameters.EndDate, parameters.TopCount);

            var topReport = topProducts.Select(tp => new
            {
                Product = tp.ProductName,
                SoldQuantity = tp.TotalSold
            }).ToList();

            ReportData = new ObservableCollection<object>(topReport);
        }


        private void ExportToPdf()
        {
            if (ReportData == null || !ReportData.Any())
            {
                MessageBox.Show("Нет данных для экспорта.", "Экспорт в PDF", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "PDF файлы (*.pdf)|*.pdf",
                FileName = "Отчет.pdf"
            };

            if (saveDialog.ShowDialog() == true)
            {

                PdfExporter.Export(ReportData.ToList(), saveDialog.FileName);
                MessageBox.Show("Экспорт завершен.", "Экспорт в PDF", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
