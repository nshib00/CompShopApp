using BLL.DTO;
using BLL.Services.Interfaces;
using ComputerShop.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace CompShop.ViewModels
{
    public class EditCategoryVM : INotifyPropertyChanged
    {
        private readonly ICategoryService _categoryService;

        public event Action OnCategoryUpdated;
        public event PropertyChangedEventHandler PropertyChanged;
        public Action CloseAction { get; set; }

        public EditCategoryVM(ICategoryService categoryService, int categoryId)
        {
            _categoryService = categoryService;
            CategoryId = categoryId;

            SaveCommand = new RelayCommand(_ => SaveChanges());
            CancelCommand = new RelayCommand(_ => Cancel());

            LoadCategoryData();
        }

        public int CategoryId { get; }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private int? _parentCategoryId;
        public int? ParentCategoryId
        {
            get => _parentCategoryId;
            set
            {
                _parentCategoryId = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CategoryShortDto> Categories { get; } = new();

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public void SetCategories(ObservableCollection<CategoryShortDto> categories)
        {
            Categories.Clear();
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
        }

        private void LoadCategoryData()
        {
            var category = _categoryService.GetCategoryWithHierarchy(CategoryId);
            if (category != null)
            {
                Name = category.Name;
                ParentCategoryId = category.ParentCategory?.Id;
            }
        }

        private void SaveChanges()
        {
            try
            {
                var dto = new CategoryUpdateDto
                {
                    Id = CategoryId,
                    Name = Name,
                    ParentCategoryId = ParentCategoryId
                };

                _categoryService.UpdateCategory(dto);
                OnCategoryUpdated?.Invoke();
                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении категории: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            OnCategoryUpdated?.Invoke();
            CloseAction?.Invoke();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
