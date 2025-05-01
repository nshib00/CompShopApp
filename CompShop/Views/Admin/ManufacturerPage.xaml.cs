using BLL.Services;
using BLL.Services.Interfaces;
using ComputerShop.ViewModels;
using System.Windows.Controls;

namespace CompShop.Views.Pages
{
    public partial class ManufacturerPage : Page
    {
        private readonly IManufacturerService _manufacturerModel = new ManufacturerService();

        public ManufacturerPage()
        {
            InitializeComponent();
            DataContext = new ManufacturerVM();
        }
    }
}
