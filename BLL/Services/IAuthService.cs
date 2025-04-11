using System.Security;

namespace BLL.Services.Interfaces
{
    public interface IAuthService
    {
        bool Login(string email, SecureString password, bool rememberMe);
    }
}