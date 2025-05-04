using BLL.DTO;
using BLL.Services.Interfaces;
using ComputerShop.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows;
using System.Windows.Input;

namespace ComputerShop.ViewModels
{
    public class LoginVM : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;

        private string _email;
        private SecureString _password;

        public event Action<UserDto> LoginSuccess;
        public event Action LoginFailed;

        public LoginVM(IAuthService authService)
        {
            _authService = authService;
            LoginCommand = new RelayCommand(ExecuteLogin, CanLogin);
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public SecureString Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        private bool CanLogin(object parameter)
        {
            // Можно добавить логику для проверки, можно ли выполнить команду
            return !string.IsNullOrWhiteSpace(Email) && Password?.Length > 0;
        }

        private void ExecuteLogin(object parameter)
        {
            if (Password == null || Password.Length == 0)
            {
                MessageBox.Show("Пароль не был введен", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var userDto = _authService.Login(Email, Password);
                if (userDto != null)
                {
                    LoginSuccess?.Invoke(userDto);
                }
                else
                {
                    LoginFailed?.Invoke();
                    MessageBox.Show("Неверный email или пароль", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при входе: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
