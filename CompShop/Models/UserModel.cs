using BLL.DTO;
using DAL.Context;
using DAL.Entities;
using DAL.Utils;

namespace ComputerShop.Models
{
    public class UserModel
    {
        private readonly AppDbContext _context = new AppDbContext();

        public int CreateUser(UserCreateDto dto)
        {
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                HashedPassword = PasswordHelper.HashPassword(dto.Password),
                Phone = dto.Phone,
                IsAdmin = false
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user.Id;
        }

        public void UpdateUser(int id, UserUpdateDto dto)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.Email = dto.Email;

                _context.SaveChanges();
            }
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public List<UserDto> GetAllUsers()
        {
            return _context.Users
                .Select(u => new UserDto(u))
                .ToList();
        }

        public UserDto? GetUserById(int id)
        {
            var user = _context.Users.Find(id);
            return user != null ? new UserDto(user) : null;
        }

        public UserDto Login(UserLoginDto dto)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user != null && PasswordHelper.VerifyPassword(user.HashedPassword, dto.Password))
            {
                return new UserDto(user);
            }

            return null;
        }

        public bool CheckPassword(UserLoginDto dto)
        {
            if (1 == 1)
            {
                return true;
            }
            return false;
        }
    }
}
