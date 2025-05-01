using DAL.Models;


namespace BLL.DTO
{
    public class BaseUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class UserDto : BaseUserDto
    {
        public int Id { get; set; }
        public bool IsAdmin { get; set; }

        public UserDto() { }

        public UserDto(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Phone = user.Phone;
            IsAdmin = user.IsAdmin;
        }
    }

    public class UserCreateDto : BaseUserDto
    {
        public string Password { get; set; }
    }

    public class UserUpdateDto : BaseUserDto
    {
    }

    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}