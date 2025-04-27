using System.Windows;
using BLL.DTO;
using CompShop.ViewModels;

namespace CompShop.Views.Admin.Reports
{
    public partial class TopProductsForm : Window
    {
        public TopProductsDto ReportParameters { get; private set; }

        public TopProductsForm()
        {
            InitializeComponent();
            DataContext = new TopProductsFormVM();
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            ReportParameters = new TopProductsDto
            {
                StartDate = startDatePicker.SelectedDate ?? DateTime.MinValue,
                EndDate = endDatePicker.SelectedDate ?? DateTime.MaxValue,
                TopCount = int.TryParse(topCountTextBox.Text, out var count) ? count : 10
            };

            DialogResult = true;
            Close();
        }
    }
}
