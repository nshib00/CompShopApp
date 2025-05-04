using CompShop;
using CompShop.Services.Interfaces;
using CompShop.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ComputerShop.Views
{
    public partial class OrderWindow : Window
    {
        public OrderWindow(int userId)
        {
            InitializeComponent();
            var dialogService = new DialogService(this);
            var vmFactory = App.ServiceProvider.GetRequiredService<Func<IDialogService, OrderVM>>();
            var viewModel = vmFactory(dialogService);
            viewModel.Initialize(userId);
            DataContext = viewModel;
        }
    }
}
