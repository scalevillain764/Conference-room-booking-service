using _enums;
using _interfaces;
using _result;
using _user;
using _userDto;
using _bookingDto;
using System.Xml.Linq;
using _data;
using Microsoft.EntityFrameworkCore;
namespace _userService
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext appDbContext) => _context = appDbContext;
        private async Task<Result<UserDTO>> UpdateUserPropertyAsync(int Id, Action<User> updateAction)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == Id);

            if(user == null) 
                return Result<UserDTO>.Error("Ошибка: пользователь не найден");
            
            updateAction(user);

            try
            {
                await _context.SaveChangesAsync();
                return Result<UserDTO>.Success(new UserDTO(user));
            }
            catch
            {
                return Result<UserDTO>.Error("Ошибка: БД не смогла сохранить изменения");
            }
        }
        public async Task<Result<UserDTO>> AddUserAsync(UserCreationDTO UserCreationDTO)
        {
            var user = new User(UserCreationDTO.Name, UserCreationDTO.Email, UserCreationDTO.Role);
            _context.Add(user);

            try
            {
                await _context.SaveChangesAsync();
                return Result<UserDTO>.Success(new UserDTO(user));
            }
            catch
            {
                return Result<UserDTO>.Error("Ошибка: БД не смогла сохранить изменения");
            }
        }
        public async Task<Result<UserDTO>> RemoveUserAsync(int Id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (user == null) 
                return Result<UserDTO>.Error("Ошибка: пользователь не найден");

            _context.Remove(user);
            try
            {
                await _context.SaveChangesAsync();
                return Result<UserDTO>.Success(null);
            }
            catch
            {
                return Result<UserDTO>.Error("Ошибка: БД не смогла сохранить изменения");
            }
        }
        public async Task<Result<UserDTO>> GetUserAsync(int Id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == Id);
            return user != null ? Result<UserDTO>.Success(new UserDTO(user)) : Result<UserDTO>.Error("Ошибка: пользователь не найден");
        }
        public async Task<Result<List<BookingDTO>>> GetAllActiveBookingsAsync(int Id)
        {
            bool UserExists = await _context.Users.AnyAsync(x => x.Id == Id);
            if (!UserExists)
                return Result<List<BookingDTO>>.Error("Ошибка: пользователь не найден");

            var bookings = _context.Bookings
                    .Where(x => x.Status == BookingStatus.Confirmed 
                    || x.Status == BookingStatus.Pending)
                        .Select(x => new BookingDTO(x))
                        .ToList();

            return bookings.Any()
                ? Result<List<BookingDTO>>.Success(bookings)
                : Result<List<BookingDTO>>.Error("Нет активных броней");
        }
        public async Task<Result<List<BookingDTO>>> GetHistoryBookingsAsync(int Id)
        {
            bool UserExists = await _context.Users.AnyAsync(x => x.Id == Id);

            if (!UserExists) 
                return Result<List<BookingDTO>>.Error("Ошибка: пользователь не найден");

            var bookings = _context.Bookings
                .Where(x => x.Id == Id && x.Status == BookingStatus.Completed)
                .Select(x => new BookingDTO(x))
                .ToList();

            return bookings.Any() 
                ? Result<List<BookingDTO>>.Success(bookings) 
                : Result<List<BookingDTO>>.Error("Нет броней");
        }
        public async Task<Result<List<UserDTO>>> GetAllUsersAsync()
        {
            bool UserExists = await _context.Users.AnyAsync();
            if (!UserExists)
                return Result<List<UserDTO>>.Error("Ошибка: пользователей нет");           
            return Result<List<UserDTO>>.Success(_context.Users.Select(x => new UserDTO(x)).ToList());
        }
        public async Task<Result<UserDTO>> ChangeNameAsync(int Id, string Name) => await UpdateUserPropertyAsync(Id, user => user.Name = Name);
        public async Task<Result<UserDTO>> ChangeEmailAsync(int Id, string Email) => await UpdateUserPropertyAsync(Id, user => user.Email = Email);
        public async Task<Result<UserDTO>> ReplenishBalanceAsync(int Id, decimal Amount) => await UpdateUserPropertyAsync (Id, user => user.Balance += Amount);
    }
}