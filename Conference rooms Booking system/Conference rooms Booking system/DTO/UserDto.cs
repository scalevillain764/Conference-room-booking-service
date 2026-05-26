using _result;
using _enums;
using _user;
using _bookingDto;
namespace _userDto
{
    public class UserCreationDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public UserCreationDTO(string Name, string Email, Role Role)
        {
            this.Name = Name;
            this.Email = Email;
            this.Role = Role;
        }
        public static Result<UserCreationDTO> Create(string Name, string Email, string Role_str)
        {
            if (string.IsNullOrWhiteSpace(Name)) return Result<UserCreationDTO>.Error("Ошибка: неверное имя");
            if (string.IsNullOrEmpty(Email) || !Email.Contains('@')) return Result<UserCreationDTO>.Error("Ошибка: неверный email");

            Role t_role;
            if (string.IsNullOrEmpty(Role_str) || !Enum.TryParse(Role_str, out t_role)) return Result<UserCreationDTO>.Error("Неверная роль");
            return Result<UserCreationDTO>.Success(new UserCreationDTO(Name, Email, t_role));
        }
    }
    public class UserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public decimal Balance { get; set; }
        public List <BookingDTO> Bookings { get; set; } = new List<BookingDTO>();
        public UserDTO(User user)
        {
            Name = user.Name;
            Email = user.Email;
            Role = user.Role.ToString();
            Balance = user.Balance;
            Bookings = user.Bookings.Select(x => new BookingDTO(x)).ToList();
        }
    }
}