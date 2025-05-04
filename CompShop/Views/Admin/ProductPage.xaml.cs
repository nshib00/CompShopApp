using CompShop.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace CompShop.Views.Pages
{
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<ProductVM>();
        }
    }
}
