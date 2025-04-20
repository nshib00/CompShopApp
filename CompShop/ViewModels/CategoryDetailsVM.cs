using BLL.DTO;
using ComputerShop.Commands;
using ComputerShop.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CompShop.ViewModels
{
    public class CategoryDetailsVM : INotifyPropertyChanged
    {
        private readonly CategoryModel _categoryModel;
        private CategoryWithHierarchyDto _category;

        public event Action OnBackRequested;
        public event PropertyChangedEventHandler PropertyChanged;

        public CategoryDetailsVM(CategoryModel categoryModel, int categoryId)
        {
            _categoryModel = categoryModel;
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
            Category = _categoryModel.GetCategoryWithHierarchy(CategoryId);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
