using CompShop.ViewModels;
using System.Windows;

namespace ComputerShop.Views
{
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
            DataContext = new AddEditProductVM(this);
        }
    }
}
