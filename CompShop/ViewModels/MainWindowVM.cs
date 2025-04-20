using BLL.DTO;
using BLL.Services;
using BLL.Services.Interfaces;
using ComputerShop.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows;
using System.Windows.Input;

namespace ComputerShop.ViewModels
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;
        private string _email;

        public event Action<UserDto> LoginSuccess;
        public event Action NavigateToRegistrationRequested;
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowVM() : this(new AuthService()) { }

        public MainWindowVM(IAuthService authService)
        {
            _authService = authService;
            LoginCommand = new RelayCommand(Login, CanLogin);
            NavigateToRegistrationCommand = new RelayCommand(NavigateToRegistration);
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                    (LoginCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand NavigateToRegistrationCommand { get; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool CanLogin(object parameter)
        {
            return true;
            //return !string.IsNullOrWhiteSpace(Email) &&
            //       parameter is SecureString securePassword &&
            //       securePassword.Length > 0;
        }

        private void Login(object parameter)
        {
            if (!(parameter is SecureString securePassword))
            {
                MessageBox.Show("Пароль не был введен", "Ошибка входа",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var userDto = _authService.Login(Email, securePassword);
                if (userDto != null)
                {
                    LoginSuccess?.Invoke(userDto);
                }
                else
                {
                    MessageBox.Show("Неверный email или пароль", "Ошибка входа",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при входе: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NavigateToRegistration(object parameter)
        {
            NavigateToRegistrationRequested?.Invoke();
        }
    }
}