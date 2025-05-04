using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace CompShop.Views.Pages
{
    public partial class ManufacturerPage : Page
    {
        public ManufacturerPage()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<ManufacturerVM>();
        }
    }
}
