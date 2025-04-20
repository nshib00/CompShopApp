using BLL.DTO;
using System.Security;

namespace BLL.Services.Interfaces
{
    public interface IAuthService
    {
        UserDto Login(string email, SecureString password);
    }
}