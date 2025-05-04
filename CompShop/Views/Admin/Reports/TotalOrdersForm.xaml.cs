using System.Windows;
using BLL.DTO;
using CompShop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CompShop.Views.Admin.Reports
{
    public partial class TotalOrdersForm : Window
    {
        public TotalOrdersDto? ReportParameters { get; private set; }

        public TotalOrdersForm()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<TotalOrdersFormVM>();
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as TotalOrdersFormVM;

            ReportParameters = new TotalOrdersDto
            {
                StartDate = vm?.TotalOrdersStartDate ?? DateTime.MinValue,
                EndDate = vm?.TotalOrdersEndDate ?? DateTime.MaxValue,
                SelectedStatus = vm?.SelectedStatus
            };

            DialogResult = true;
            Close();
        }
    }
}
