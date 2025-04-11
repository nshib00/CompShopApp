using BLL.Services;
using BLL.Services.Interfaces;
using ComputerShop.Commands;
using System;
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
        private bool _rememberMe;
        private bool _isBusy;

        public event Action LoginSuccess;
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

        public bool RememberMe
        {
            get => _rememberMe;
            set
            {
                if (_rememberMe != value)
                {
                    _rememberMe = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
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
            //       !IsBusy &&
            //       parameter is SecureString securePassword &&
            //       securePassword.Length > 0;
        }

        private void Login(object parameter)
        {
            try
            {
                var securePassword = parameter as SecureString;
                var success = _authService.Login(Email, securePassword, RememberMe);

                if (success)
                {
                    LoginSuccess?.Invoke();
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
            finally
            {
                IsBusy = false;
            }
        }

        private void NavigateToRegistration(object parameter)
        {
            NavigateToRegistrationRequested?.Invoke();
        }
    }
}
