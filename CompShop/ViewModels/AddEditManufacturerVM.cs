using BLL.DTO;
using BLL.Services.Interfaces;
using ComputerShop.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace CompShop.ViewModels
{
    public class AddEditManufacturerVM : INotifyPropertyChanged
    {
        private readonly Window _window;
        private readonly IManufacturerService _manufacturerService;

        private string _name;
        private string _description;
        private string _country;
        private int? _manufacturerId;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public string Country
        {
            get => _country;
            set { _country = value; OnPropertyChanged(); }
        }

        public ICommand SaveManufacturerCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action OnManufacturerSaved;


        public AddEditManufacturerVM(Window window, IManufacturerService manufacturerService)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
            _manufacturerService = manufacturerService ?? throw new ArgumentNullException(nameof(manufacturerService));

            SaveManufacturerCommand = new RelayCommand(_ => SaveManufacturer());
            CancelCommand = new RelayCommand(_ => _window.Close());
        }

        public void SetManufacturer(ManufacturerDto manufacturer)
        {
            if (manufacturer == null) return;

            _manufacturerId = manufacturer.Id;
            Name = manufacturer.Name;
            Description = manufacturer.Description;
            Country = manufacturer.Country;
        }

        private void SaveManufacturer()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    MessageBox.Show("Название не может быть пустым", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var manufacturer = new ManufacturerDto
                {
                    Id = _manufacturerId ?? 0, // Если редактируем — есть Id, если добавляем — 0
                    Name = Name,
                    Description = Description,
                    Country = Country
                };

                if (_manufacturerId.HasValue)
                {
                    _manufacturerService.UpdateManufacturer(manufacturer);
                }
                else
                {
                    _manufacturerService.CreateManufacturer(manufacturer);
                }

                OnManufacturerSaved?.Invoke();
                _window.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
