using BusStationApp.Views;
using ComputerShop.ViewModels;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Input;

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

           // Loaded += (s, e) => txtEmail.Focus();
        }

        private void OnLoginSuccess()
        {
            // Создаем и показываем главное окно приложения
            //var mainAppWindow = new ShopMainWindow();
            //mainAppWindow.Show();

            // Закрываем текущее окно входа
            //this.Close();
        }

        private void OnNavigateToRegistrationRequested()
        {
            // Создаем и показываем окно регистрации
            var registrationWindow = new RegistrationWindow();
            registrationWindow.Owner = this;
            registrationWindow.ShowDialog();
        }

        // Обработчик нажатия Enter в поле пароля
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && DataContext is MainWindowVM vm)
            {
                vm.LoginCommand.Execute(txtPassword.SecurePassword);
            }
        }

        // Обработчик изменения видимости окна
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            // Сброс пароля при повторном открытии окна
            if (DataContext is MainWindowVM vm && !vm.RememberMe)
            {
                txtPassword.Clear();
            }
        }
    }
}