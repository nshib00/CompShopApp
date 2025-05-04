using CompShop;
using ComputerShop.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Navigation;

namespace ComputerShop.Views
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
            var viewModel = ActivatorUtilities.CreateInstance<RegistrationVM>(
                App.ServiceProvider,
                txtPassword,
                txtConfirmPassword
            );
            DataContext = viewModel;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
            e.Handled = true;
        }
    }
}
