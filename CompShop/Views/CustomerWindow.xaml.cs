using System.Windows;
using System.Windows.Controls;
using BLL.DTO;
using CompShop.ViewModels;

namespace ComputerShop.Views
{
    public partial class CustomerWindow : Window
    {
        private readonly CustomerVM _viewModel;

        public CustomerWindow()
        {
            InitializeComponent();
            _viewModel = new CustomerVM();
            this.DataContext = _viewModel;

            // Подключение обработчиков событий
            this.Loaded += CustomerWindow_Loaded;
        }

        private void CustomerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Загрузка категорий и товаров
            _viewModel.LoadCategories();
            _viewModel.LoadProducts();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Обновляем поисковый текст в ViewModel
            _viewModel.SearchText = SearchTextBox.Text;
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Обновляем выбранную категорию в ViewModel
            _viewModel.SelectedCategory = (CategoryDto)CategoryComboBox.SelectedItem;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика для выхода из системы
            _viewModel.Logout(sender);
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
        }
    }
}
