using ComputerShop.ViewModels;
using System.Windows;
using System.Windows.Navigation;

namespace ComputerShop.Views
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
            DataContext = new RegistrationVM(txtPassword, txtConfirmPassword);
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
