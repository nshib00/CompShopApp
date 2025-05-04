using BLL.DTO;
using BLL.Services;
using BLL.Services.Interfaces;
using ComputerShop.Commands;
using ComputerShop.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ComputerShop.ViewModels
{
    public class RegistrationVM : INotifyPropertyChanged
    {
        private readonly PasswordBox _passwordBox;
        private readonly PasswordBox _confirmPasswordBox;
        private readonly IUserService _userService;

        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phone;
        private bool _isBusy;

        public event PropertyChangedEventHandler PropertyChanged;

        public RegistrationVM(PasswordBox passwordBox, PasswordBox confirmPasswordBox, IUserService userService)
        {
            _passwordBox = passwordBox;
            _confirmPasswordBox = confirmPasswordBox;
            _userService = userService;

            RegisterCommand = new RelayCommand(Register, CanRegister);
        }

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public ICommand RegisterCommand { get; }

        private bool CanRegister(object parameter) => !IsBusy;

        private void Register(object parameter)
        {
            var password = _passwordBox.Password;
            var confirmPassword = _confirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(Phone))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                IsBusy = true;

                var newUser = new UserCreateDto
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    Password = password,
                    Phone = Phone
                };

                _userService.CreateUser(newUser);

                MessageBox.Show("Регистрация прошла успешно! Войдите в систему.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                var loginWindow = new MainWindow();
                loginWindow.Show();
                Application.Current.Windows[0]?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
