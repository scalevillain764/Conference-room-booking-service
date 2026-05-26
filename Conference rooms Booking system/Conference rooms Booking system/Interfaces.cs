using _room;
using _booking;
using _user;
using _result;
using _enums;
using _bookingDto;
using _userDto;
using _roomDto;
namespace _interfaces
{
    public interface IEntity { public int Id { get; set; } }
    public interface IUserService
    {     
        Task<Result<UserDTO>> AddUserAsync(UserCreationDTO UserCreationDTO);
        Task<Result<UserDTO>> RemoveUserAsync(int Id);
        Task<Result<UserDTO>> GetUserAsync(int Id);
        Task<Result<List<UserDTO>>> GetAllUsersAsync();
        Task<Result<UserDTO>> ChangeNameAsync(int Id, string Name);
        Task<Result<UserDTO>> ChangeEmailAsync(int Id, string Email);
        Task<Result<UserDTO>> ReplenishBalanceAsync(int Id, decimal Amount);
        Task<Result<List<BookingDTO>>> GetAllActiveBookingsAsync(int Id);
        Task<Result<List<BookingDTO>>> GetHistoryBookingsAsync(int Id);
    }
    public interface IRoomService
    {
        Task<Result<RoomDTO>> AddRoomAsync(RoomCreationDTO RoomCreationDTO);
        Task<Result<RoomDTO>> RemoveRoomAsync(int Id);
        Task<Result<RoomDTO>> GetRoomAsync(int Id);
        Task<Result<List<RoomDTO>>> GetAllRoomsAsync();     
        Task<Result<RoomDTO>> ChangeNameAsync(int Id, string Name);
        Task<Result<RoomDTO>> ChangeCapacityAsync(int Id, double Capacity);
        Task<Result<RoomDTO>> ChangeFloorAsync(int Id, int Floor);
        Task<Result<RoomDTO>> ChangePriceAsync(int Id, decimal Price);
        Task<Result<RoomDTO>> ChangeStatusAsync(int Id, RoomStatus status);
    }
    public interface IBookingService
    {
        Task<Result<BookingDTO>> AddBookingAsync(BookingCreationDTO BookingCreationDTO);
        Task<Result<BookingDTO>> GetBookingByIdAsync(int Id);
        Task<Result<List<BookingDTO>>> GetBookingByUserAsync(int UserId);
        Task<Result<List<BookingDTO>>> GetBookingByRoomAsync(int RoomId);
        Task<Result<BookingDTO>> ChangeStatusAsync(int Id, BookingStatus status);
    }
}