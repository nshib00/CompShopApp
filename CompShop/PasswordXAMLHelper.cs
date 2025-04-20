using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace CompShop.Helpers
{
    public static class PasswordXAMLHelper
    {
        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(PasswordXAMLHelper), new PropertyMetadata(false, Attach));

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(SecureString), typeof(PasswordXAMLHelper));

        public static bool GetAttach(DependencyObject obj) => (bool)obj.GetValue(AttachProperty);
        public static void SetAttach(DependencyObject obj, bool value) => obj.SetValue(AttachProperty, value);

        public static SecureString GetPassword(DependencyObject obj) => (SecureString)obj.GetValue(PasswordProperty);
        public static void SetPassword(DependencyObject obj, SecureString value) => obj.SetValue(PasswordProperty, value);

        private static void Attach(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox && (bool)e.NewValue)
            {
                passwordBox.PasswordChanged += (s, args) =>
                {
                    SetPassword(passwordBox, passwordBox.SecurePassword);
                };
            }
        }
    }
}
