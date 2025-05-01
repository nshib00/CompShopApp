using CompShop.ViewModels;
using DAL.Context;
using DAL.Models;
using System.Windows;
using System.Windows.Controls;

namespace CompShop.Views.Pages
{
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();
            DataContext = new ProductVM();
        }
    }
}
