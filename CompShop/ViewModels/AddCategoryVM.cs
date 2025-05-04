using BLL.DTO;
using BLL.Services.Interfaces;
using ComputerShop.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

public class AddCategoryVM : INotifyPropertyChanged
{
    private readonly ICategoryService _categoryService;
    private string _name;
    private int? _parentCategoryId;

    public event Action OnCategoryAdded;
    public event PropertyChangedEventHandler PropertyChanged;

    public Action CloseAction { get; set; }

    public AddCategoryVM(ICategoryService categoryService)
    {
        _categoryService = categoryService;

        Categories = new ObservableCollection<CategoryShortDto>();

        SaveCommand = new RelayCommand(_ => SaveCategory(), _ => CanSaveCategory());
        CancelCommand = new RelayCommand(_ => Cancel());
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
            CommandManager.InvalidateRequerySuggested();
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

    public ObservableCollection<CategoryShortDto> Categories { get; }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public void SetCategories(IEnumerable<CategoryShortDto> categories)
    {
        Categories.Clear();
        foreach (var category in categories)
        {
            Categories.Add(category);
        }
    }

    private bool CanSaveCategory()
    {
        return !string.IsNullOrWhiteSpace(Name);
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
