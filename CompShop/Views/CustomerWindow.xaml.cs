using System.Collections.Generic;
using System.Windows;
using BLL.DTO;

namespace ComputerShop.Views
{
    public partial class CustomerWindow : Window
    {
        public CustomerWindow()
        {
            InitializeComponent();
            LoadProducts();
        }

        // Загрузка списка продуктов (пример, можно заменить на реальную загрузку из БД)
        private void LoadProducts()
        {
            var products = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Товар 1", Description = "Описание товара 1", Price = 1000, CategoryName = "Категория 1" },
                new ProductDto { Id = 2, Name = "Товар 2", Description = "Описание товара 2", Price = 1500, CategoryName = "Категория 2" },
                new ProductDto { Id = 3, Name = "Товар 3", Description = "Описание товара 3", Price = 2000, CategoryName = "Категория 3" }
            };

            ProductsItemsControl.ItemsSource = products;
        }

        // Обработка кнопки "Войти/Выйти"
        private void LoginLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
