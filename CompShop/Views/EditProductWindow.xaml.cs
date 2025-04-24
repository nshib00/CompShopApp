using BLL.DTO;
using System.Windows;

namespace CompShop.Views
{
    public partial class EditProductWindow : Window
    {
        public EditProductWindow(ProductDto product)
        {
            InitializeComponent();
            DataContext = product;
        }
    }
}
