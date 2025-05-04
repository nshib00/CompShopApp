using BLL.DTO;
using BLL.Services.Interfaces;
using CompShop.ViewModels;
using CompShop.Views;
using ComputerShop.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;

public class ManufacturerVM : INotifyPropertyChanged
{
    private readonly IManufacturerService _manufacturerService;

    public ManufacturerVM(IManufacturerService manufacturerService)
    {
        _manufacturerService = manufacturerService;

        LoadManufacturersCommand = new RelayCommand(_ => LoadManufacturers());
        AddManufacturerCommand = new RelayCommand(_ => ShowAddManufacturerWindow());
        EditManufacturerCommand = new RelayCommand(_ => ShowEditManufacturerWindow());
        DeleteManufacturerCommand = new RelayCommand(_ => DeleteManufacturer());

        LoadManufacturers();
    }

    private ObservableCollection<ManufacturerDto> _manufacturers;
    public ObservableCollection<ManufacturerDto> Manufacturers
    {
        get => _manufacturers;
        set
        {
            _manufacturers = value;
            OnPropertyChanged();
        }
    }

    private ManufacturerDto _selectedManufacturer;
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

    private void LoadManufacturers()
    {
        Manufacturers = new ObservableCollection<ManufacturerDto>(_manufacturerService.GetAllManufacturers());
    }

    private void ShowAddManufacturerWindow()
    {
        var window = new AddManufacturerWindow();
        var vm = new AddEditManufacturerVM(window, _manufacturerService);

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
        if (SelectedManufacturer == null)
        {
            MessageBox.Show("Пожалуйста, выберите производителя для редактирования",
                              "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var window = new AddManufacturerWindow { Title = "Редактировать производителя" };
        var vm = new AddEditManufacturerVM(window, _manufacturerService);

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
        if (SelectedManufacturer == null)
        {
            MessageBox.Show("Пожалуйста, выберите производителя для удаления",
                              "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

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
            catch (Exception ex)
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
