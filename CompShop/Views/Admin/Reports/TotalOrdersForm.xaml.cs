using System.Windows;
using BLL.DTO;
using CompShop.ViewModels;

namespace CompShop.Views.Admin.Reports
{
    public partial class TotalOrdersForm : Window
    {
        public TotalOrdersDto? ReportParameters { get; private set; }

        public TotalOrdersForm()
        {
            ReportParameters = null;
            InitializeComponent();
            DataContext = new TotalOrdersFormVM();
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            ReportParameters = new TotalOrdersDto
            {
                StartDate = startDatePicker.SelectedDate ?? DateTime.MinValue,
                EndDate = endDatePicker.SelectedDate ?? DateTime.MaxValue,
                SelectedStatus = statusComboBox.SelectedItem as string
            };

            DialogResult = true;
            Close();
        }
    }
}
