using CompShop;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ComputerShop.Views
{
    public partial class OrderWindow : Window
    {
        public OrderWindow(int userId)
        {
            InitializeComponent();
            var vm = App.ServiceProvider.GetRequiredService<OrderVM>();
            vm.CloseWindowRequested += Close;
            DataContext = vm;
        }
    }
}
