using CompShop.Views.Pages;
using System.Windows;

namespace ComputerShop.Views
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();

            ProductFrame.NavigationService.Navigate(new ProductPage());
            CategoryFrame.NavigationService.Navigate(new CategoryPage());
            //OrderFrame.NavigationService.Navigate(new OrderPage());
            ManufacturerFrame.NavigationService.Navigate(new ManufacturerPage());
        }
    }
}
