using BLL.DTO;
using CompShop.ViewModels;
using ComputerShop.Models;
using ComputerShop.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace CompShop.Views.Pages
{
    public partial class ManufacturerPage : Page
    {
        private readonly ManufacturerModel _manufacturerModel = new();

        public ManufacturerPage()
        {
            InitializeComponent();
            DataContext = new ManufacturerVM();
        }
    }
}
