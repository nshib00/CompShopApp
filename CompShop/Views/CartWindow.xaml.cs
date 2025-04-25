using System.Windows;
using ComputerShop.Commands;
using ComputerShop.ViewModels;
using DAL.Entities;

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
