using BLL.DTO;
using ComputerShop.Commands;
using ComputerShop.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace CompShop.ViewModels
{
    public class AddEditManufacturerVM : INotifyPropertyChanged
    {
        private readonly ManufacturerModel _manufacturerModel = new();
        private readonly Window _window;

        public event Action OnManufacturerSaved;

        private int _id;
        private string _name;
        private string _description;
        private string _country;

        public bool IsEditMode { get; set; }

        public AddEditManufacturerVM(Window window)
        {
            _window = window;
            SaveManufacturerCommand = new RelayCommand(SaveManufacturer);
            CancelCommand = new RelayCommand(_ => _window.Close());
        }

        public void SetManufacturer(ManufacturerDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Description = dto.Description;
            Country = dto.Country;
            IsEditMode = true;
        }

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

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

        private void SaveManufacturer(object obj)
        {
            var dto = new ManufacturerDto
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Country = Country
            };

            try
            {
                if (IsEditMode)
                    _manufacturerModel.UpdateManufacturer(dto);
                else
                    _manufacturerModel.CreateManufacturer(dto);

                OnManufacturerSaved?.Invoke();
                _window.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении производителя: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
