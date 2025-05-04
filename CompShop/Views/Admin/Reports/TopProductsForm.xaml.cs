using System.Windows;
using BLL.DTO;
using CompShop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CompShop.Views.Admin.Reports
{
    public partial class TopProductsForm : Window
    {
        public TopProductsDto ReportParameters { get; private set; }

        public TopProductsForm()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<TopProductsFormVM>();
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as TopProductsFormVM;

            ReportParameters = new TopProductsDto
            {
                StartDate = vm?.TopSalesStartDate ?? DateTime.MinValue,
                EndDate = vm?.TopSalesEndDate ?? DateTime.MaxValue,
                TopCount = vm?.TopCount ?? 10
            };

            DialogResult = true;
            Close();
        }
    }
}
