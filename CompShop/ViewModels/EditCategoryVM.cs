using BLL.DTO;
using ComputerShop.Commands;
using ComputerShop.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace CompShop.ViewModels
{
    public class EditCategoryVM : INotifyPropertyChanged
    {
        private readonly CategoryModel _categoryModel;
        private string _name;
        private int? _parentCategoryId;

        public event Action OnCategoryUpdated;
        public event PropertyChangedEventHandler PropertyChanged;

        public EditCategoryVM(CategoryModel categoryModel, int categoryId)
        {
            _categoryModel = categoryModel;
            CategoryId = categoryId;
            LoadCategoryData();

            SaveCommand = new RelayCommand(_ => SaveChanges());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        public int CategoryId { get; }

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

        private void LoadCategoryData()
        {
            var category = _categoryModel.GetCategoryWithHierarchy(CategoryId);
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

                _categoryModel.UpdateCategory(dto);
                OnCategoryUpdated?.Invoke();
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
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
