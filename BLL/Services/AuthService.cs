using BLL.DTO;
using BLL.Services.Interfaces;
using DAL.Context;
using DAL.Utils;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService()
        {
            _context = new AppDbContext();
        }

        public UserDto? Login(string email, SecureString password)
        {
            var plainPassword = SecureStringHelper.ConvertToUnsecureString(password);

            // Проверка в базе данных
            var user = _context.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Email == email);

            if (user == null)
                return null;

            if (!PasswordHelper.VerifyPassword(user.HashedPassword, plainPassword))
                return null;

            return new UserDto(user);
        }

        public void Dispose()
        {
            _context?.Dispose();
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
