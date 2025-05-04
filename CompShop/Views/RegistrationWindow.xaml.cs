using ComputerShop.ViewModels;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Extensions.DependencyInjection;
using CompShop;

namespace ComputerShop.Views
{
    public partial class RegistrationWindow : Window
    {
        private readonly RegistrationVM _viewModel;

        public RegistrationWindow()
        {
            InitializeComponent();
            _viewModel = App.ServiceProvider.GetRequiredService<RegistrationVM>();
            DataContext = _viewModel;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
            e.Handled = true;
        }
    }
}
