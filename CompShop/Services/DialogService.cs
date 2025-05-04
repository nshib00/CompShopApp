using CompShop.Services.Interfaces;
using System.Windows;

namespace CompShop.Services
{
    public class DialogService : IDialogService
    {
        private readonly Window _window;

        public DialogService(Window window)
        {
            _window = window;
        }

        public void CloseDialog()
        {
            _window.Close();
        }
    }

    public class VoidDialogService : IDialogService
    {
        public void CloseDialog()
        { }
    }
}
