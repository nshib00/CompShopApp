using CompShop;
using CompShop.Services;
using CompShop.Services.Interfaces;
using ComputerShop.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ComputerShop.Views
{
    public partial class CartWindow : Window
    {
        public CartWindow(int userId)
        {
            InitializeComponent();

            var dialogService = new DialogService(this);
            var vmFactory = App.ServiceProvider.GetRequiredService<Func<IDialogService, CartVM>>();
            var viewModel = vmFactory(dialogService);
            viewModel.Initialize(userId);
            DataContext = viewModel;
        }
    }
}
