using CompShop.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace CompShop.Views.Pages
{
    public partial class ReportPage : Page
    {
        public ReportPage()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<ReportVM>();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "OrderNumber":
                    e.Column.Header = "Номер заказа";
                    break;
                case "OrderDate":
                    e.Column.Header = "Дата заказа";
                    break;
                case "Status":
                    e.Column.Header = "Статус";
                    break;
                case "OrderAmount":
                    e.Column.Header = "Сумма заказа";
                    break;
                case "Category":
                    e.Column.Header = "Категория";
                    break;
                case "SoldQuantity":
                    e.Column.Header = "Количество проданных товаров";
                    break;
                case "TotalAmount":
                    e.Column.Header = "Общая сумма";
                    break;
                case "Product":
                    e.Column.Header = "Продукт";
                    break;
                default:
                    break;
            }
        }
    }
}
