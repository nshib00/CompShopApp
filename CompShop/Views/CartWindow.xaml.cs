using ComputerShop.ViewModels;
using System.Windows;

namespace ComputerShop.Views
{
    public partial class CartWindow : Window
    {
        public CartWindow(CartVM viewModel)
        {
            InitializeComponent();
            viewModel.CloseWindowRequested += CloseWindow;
            DataContext = viewModel;
        }

        private void CloseWindow()
        {
            this.Close();
        }
    }
}
