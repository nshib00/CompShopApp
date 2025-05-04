using BLL.Services;
using BLL.Services.Interfaces;
using CompShop.Services;
using CompShop.Services.Interfaces;
using CompShop.ViewModels;
using CompShop.Views.Admin.Reports;
using CompShop.Views.Pages;
using ComputerShop.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace CompShop
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<ICartService, CartService>();
            services.AddSingleton<ICategoryService, CategoryService>();
            services.AddSingleton<IManufacturerService, ManufacturerService>();
            services.AddSingleton<IOrderService, OrderService>();
            services.AddSingleton<IOrderStatusService, OrderStatusService>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IReportService, ReportService>();
            services.AddSingleton<IUserService, UserService>();

            services.AddSingleton<IDialogService, DialogService>();

            services.AddTransient<MainWindowVM>();
            services.AddTransient<ManufacturerVM>();
            services.AddTransient<AddCategoryVM>();
            services.AddTransient<AddEditManufacturerVM>();
            services.AddTransient<AddEditProductVM>();
            services.AddTransient<CartItemVM>();
            services.AddTransient<CartVM>();
            services.AddTransient<CategoryDetailsVM>();
            services.AddTransient<CategoryManagementVM>();
            services.AddTransient<CategoryOrdersFormVM>();
            services.AddTransient<CustomerVM>();
            services.AddTransient<EditCategoryVM>();
            services.AddTransient<OrderVM>();
            services.AddTransient<ProductVM>();
            services.AddTransient<RegistrationVM>();
            services.AddTransient<ReportVM>();
            services.AddTransient<TopProductsFormVM>();
            services.AddTransient<TotalOrdersFormVM>();
            services.AddTransient<LoginVM>();

            services.AddTransient<ManufacturerPage>();
            services.AddTransient<CategoryOrdersForm>();

            services.AddTransient<Func<int, EditCategoryVM>>(sp => id => ActivatorUtilities.CreateInstance<EditCategoryVM>(sp, id));
            services.AddTransient<Func<int, CategoryDetailsVM>>(sp => id => ActivatorUtilities.CreateInstance<CategoryDetailsVM>(sp, id));
            services.AddTransient<Func<IDialogService, OrderVM>>(sp => dialogService =>
                new OrderVM(
                    sp.GetRequiredService<IOrderService>(),
                    sp.GetRequiredService<ICartService>(),
                    dialogService
                ));
            services.AddTransient<Func<IDialogService, CartVM>>(sp => dialogService =>
                new CartVM(
                    sp.GetRequiredService<ICartService>(),
                    dialogService
                ));
            services.AddTransient<Func<CartVM>>(sp => () =>
                new CartVM(
                    sp.GetRequiredService<ICartService>(),
                    new VoidDialogService()
                ));
        }
    }
}
