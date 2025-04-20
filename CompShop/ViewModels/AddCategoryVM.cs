using BLL.DTO;
using ComputerShop.Commands;
using ComputerShop.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;

public class AddCategoryVM : INotifyPropertyChanged
{
    private readonly CategoryModel _categoryModel;
    private string _name;
    private int? _parentCategoryId;

    public event Action OnCategoryAdded;
    public event PropertyChangedEventHandler PropertyChanged;

    public AddCategoryVM(CategoryModel categoryModel)
    {
        _categoryModel = categoryModel;
        SaveCommand = new RelayCommand(_ => SaveCategory());
        CancelCommand = new RelayCommand(_ => Cancel());
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    public int? ParentCategoryId
    {
        get => _parentCategoryId;
        set
        {
            _parentCategoryId = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<CategoryShortDto> Categories { get; } = new ObservableCollection<CategoryShortDto>();
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    private void SaveCategory()
    {
        try
        {
            var dto = new CategoryCreateDto
            {
                Name = Name,
                ParentCategoryId = ParentCategoryId
            };

            _categoryModel.CreateCategory(dto);
            OnCategoryAdded?.Invoke();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при создании категории: {ex.Message}",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Cancel()
    {
        OnCategoryAdded?.Invoke();
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}