using BLL.DTO;
using CompShop.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace CompShop.Views.Admin.Reports
{
    public partial class CategoryOrdersForm : Window
    {
        public CategoryOrdersFormVM ViewModel => (CategoryOrdersFormVM)DataContext;

        public CategoryOrdersForm()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<CategoryOrdersFormVM>();
        }

        public CategoryOrdersDto ReportParameters { get; private set; }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CategoryOrdersStartDate == null || ViewModel.CategoryOrdersEndDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите оба диапазона дат.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ReportParameters = new CategoryOrdersDto
            {
                StartDate = ViewModel.CategoryOrdersStartDate.Value,
                EndDate = ViewModel.CategoryOrdersEndDate.Value
            };

            DialogResult = true;
            Close();
        }
    }
}
