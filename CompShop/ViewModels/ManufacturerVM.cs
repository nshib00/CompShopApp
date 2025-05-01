using BLL.DTO;
using BLL.Services;
using BLL.Services.Interfaces;
using CompShop.ViewModels;
using CompShop.Views;
using ComputerShop.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ComputerShop.ViewModels
{
    public class ManufacturerVM : INotifyPropertyChanged
    {
        private readonly IManufacturerService _manufacturerService;
        private ManufacturerDto _selectedManufacturer;

        public ObservableCollection<ManufacturerDto> Manufacturers { get; } = new();

        public ManufacturerDto SelectedManufacturer
        {
            get => _selectedManufacturer;
            set
            {
                _selectedManufacturer = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand LoadManufacturersCommand { get; }
        public ICommand AddManufacturerCommand { get; }
        public ICommand EditManufacturerCommand { get; }
        public ICommand DeleteManufacturerCommand { get; }

        public ManufacturerVM()
        {
            _manufacturerService = new ManufacturerService();

            LoadManufacturersCommand = new RelayCommand(_ => LoadManufacturers());
            AddManufacturerCommand = new RelayCommand(_ => ShowAddManufacturerWindow());
            EditManufacturerCommand = new RelayCommand(_ => ShowEditManufacturerWindow(), _ => SelectedManufacturer != null);
            DeleteManufacturerCommand = new RelayCommand(_ => DeleteManufacturer(), _ => SelectedManufacturer != null);

            LoadManufacturers();
        }

        private void LoadManufacturers()
        {
            Manufacturers.Clear();
            var manufacturers = _manufacturerService.GetAllManufacturers();
            foreach (var m in manufacturers)
                Manufacturers.Add(m);
        }

        private void ShowAddManufacturerWindow()
        {
            var window = new AddManufacturerWindow();
            var vm = new AddEditManufacturerVM(window);

            vm.OnManufacturerSaved += () =>
            {
                LoadManufacturers();
                MessageBox.Show("Производитель успешно добавлен", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            };

            window.DataContext = vm;
            window.ShowDialog();
        }
        private void ShowEditManufacturerWindow()
        {
            if (SelectedManufacturer == null) return;

            var window = new AddManufacturerWindow { Title = "Редактировать производителя" };
            var vm = new AddEditManufacturerVM(window);
            vm.SetManufacturer(SelectedManufacturer);
            vm.OnManufacturerSaved += () =>
            {
                LoadManufacturers();
                MessageBox.Show("Производитель успешно обновлён", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            };

            window.DataContext = vm;
            window.ShowDialog();
        }
        private void DeleteManufacturer()
        {
            if (SelectedManufacturer == null) return;

            var result = MessageBox.Show(
                $"Удалить производителя '{SelectedManufacturer.Name}'?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _manufacturerService.DeleteManufacturer(SelectedManufacturer.Id);
                    LoadManufacturers();
                    MessageBox.Show("Производитель удалён", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
