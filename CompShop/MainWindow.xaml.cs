using BLL.DTO;
using ComputerShop.ViewModels;
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

            var viewModel = new MainWindowVM();
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
                var vm = new CustomerVM(user);
                nextWindow = new CustomerWindow(vm);
            }

            nextWindow.Show();
            this.Close();
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
