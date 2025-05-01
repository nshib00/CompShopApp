using BLL.DTO;

namespace BLL.Services.Interfaces
{
    public interface IUserService
    {
        public int CreateUser(UserCreateDto dto);
        public void UpdateUser(int id, UserUpdateDto dto);
        public void DeleteUser(int id);
        public List<UserDto> GetAllUsers();
        public UserDto? GetUserById(int id);
    }
}
