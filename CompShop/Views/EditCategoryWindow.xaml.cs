using System.Windows;

namespace CompShop.Views
{
    public partial class EditCategoryWindow : Window
    {
        public EditCategoryWindow()
        {
            InitializeComponent();
        }

        public Action CloseAction { get; internal set; }
    }
}
