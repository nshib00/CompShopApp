using BLL.Services.Interfaces;
using System;
using System.Security;


namespace BLL.Services
{
    public class AuthService : IAuthService
    {
        public bool Login(string email, SecureString password, bool rememberMe)
        { 
            return email == "admin@example.com" &&
                   SecureStringHelper.ConvertToUnsecureString(password) == "admin123";
        }
    }

    public static class SecureStringHelper
    {
        public static string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
                return string.Empty;

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}