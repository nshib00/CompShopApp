using System.Windows;

namespace ComputerShop.Views
{
    public partial class OrderWindow : Window
    {
        public OrderWindow(int userId)
        {
            InitializeComponent();
            var vm = new OrderVM(userId);
            vm.CloseWindowRequested += Close;
            DataContext = vm;
        }
    }
}
