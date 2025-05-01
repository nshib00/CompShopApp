using BLL.DTO;
using ComputerShop.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using BLL.Services.Interfaces;

public class AddCategoryVM : INotifyPropertyChanged
{
    private readonly ICategoryService _categoryService;
    private string _name;
    private int? _parentCategoryId;
    private readonly ObservableCollection<CategoryShortDto> _categories = new ObservableCollection<CategoryShortDto>();

    public event Action OnCategoryAdded;
    public event PropertyChangedEventHandler PropertyChanged;

    public Action CloseAction { get; set; }

    public AddCategoryVM(ICategoryService categoryService)
    {
        _categoryService = categoryService;
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

    public ObservableCollection<CategoryShortDto> Categories => _categories;

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public void SetCategories(ObservableCollection<CategoryShortDto> categories)
    {
        _categories.Clear();
        foreach (var category in categories)
        {
            _categories.Add(category);
        }
    }

    private void SaveCategory()
    {
        try
        {
            var dto = new CategoryCreateDto
            {
                Name = Name,
                ParentCategoryId = ParentCategoryId
            };

            _categoryService.CreateCategory(dto);
            OnCategoryAdded?.Invoke();
            CloseAction?.Invoke();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при создании категории: {ex.Message}",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Cancel()
    {

        CloseAction?.Invoke();
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}