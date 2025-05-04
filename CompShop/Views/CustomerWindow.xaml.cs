using BLL.DTO;
using System.Windows;
using System.Windows.Controls;

namespace ComputerShop.Views
{
    public partial class CustomerWindow : Window
    {
        private readonly CustomerVM _viewModel;

        public CustomerWindow(CustomerVM viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            Loaded += CustomerWindow_Loaded;
        }

        private void CustomerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadCategories();
            _viewModel.LoadProducts();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.SearchText = SearchTextBox.Text;
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.SelectedCategory = (CategoryDto)CategoryComboBox.SelectedItem;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Logout(sender);
        }
    }
}
