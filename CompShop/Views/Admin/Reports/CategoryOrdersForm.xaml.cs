using System.Windows;
using BLL.DTO;
using CompShop.ViewModels;

namespace CompShop.Views.Admin.Reports
{
    public partial class CategoryOrdersForm : Window
    {
        public CategoryOrdersDto ReportParameters { get; private set; }

        public CategoryOrdersForm()
        {
            InitializeComponent();
            DataContext = new CategoryOrdersFormVM();
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            ReportParameters = new CategoryOrdersDto
            {
                StartDate = startDatePicker.SelectedDate ?? DateTime.MinValue,
                EndDate = endDatePicker.SelectedDate ?? DateTime.MaxValue,
            };

            DialogResult = true;
            Close();
        }
    }
}
