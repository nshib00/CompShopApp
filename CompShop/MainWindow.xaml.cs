using BLL.DTO;
using CompShop;
using ComputerShop.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ComputerShop.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = App.ServiceProvider.GetRequiredService<MainWindowVM>();
            DataContext = viewModel;

            viewModel.LoginSuccess += OnLoginSuccess;
            viewModel.NavigateToRegistrationRequested += OnNavigateToRegistrationRequested;
        }

        private void OnLoginSuccess(UserDto user)
        {
            Window nextWindow;

            if (user.IsAdmin)
                nextWindow = new AdminWindow();
            else
            {
                var customerVM = App.ServiceProvider.GetRequiredService<CustomerVM>();
                customerVM.Initialize(user);
                nextWindow = new CustomerWindow(customerVM);
            }

            nextWindow.Show();
            Close();
        }

        private void OnNavigateToRegistrationRequested()
        {
            var registrationWindow = new RegistrationWindow();
            registrationWindow.Owner = this;
            registrationWindow.ShowDialog();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && DataContext is MainWindowVM vm)
            {
                vm.LoginCommand.Execute(txtPassword.SecurePassword);
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (DataContext is MainWindowVM vm)
            {
                txtPassword.Clear();
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            var registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            e.Handled = true;
        }
    }
}
