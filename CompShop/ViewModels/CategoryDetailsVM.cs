using BLL.DTO;
using BLL.Services.Interfaces;
using ComputerShop.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CompShop.ViewModels
{
    public class CategoryDetailsVM : INotifyPropertyChanged
    {
        private readonly ICategoryService _categoryService;
        private CategoryWithHierarchyDto _category;

        public event Action OnBackRequested;
        public event PropertyChangedEventHandler PropertyChanged;

        public CategoryDetailsVM(ICategoryService categoryService, int categoryId)
        {
            _categoryService = categoryService;
            CategoryId = categoryId;
            LoadCategoryDetails();

            BackCommand = new RelayCommand(_ => OnBackRequested?.Invoke());
        }

        public int CategoryId { get; }

        public CategoryWithHierarchyDto Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }

        public ICommand BackCommand { get; }

        private void LoadCategoryDetails()
        {
            Category = _categoryService.GetCategoryWithHierarchy(CategoryId);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
