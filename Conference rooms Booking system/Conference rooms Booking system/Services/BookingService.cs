using _booking;
using _bookingDto;
using _data;
using _enums;
using _interfaces;
using _result;
using _room;
using _user;
using Microsoft.EntityFrameworkCore;
namespace _bookingService
{
    public class BookingService : IBookingService
    {
        private readonly AppDbContext _context;
        public BookingService(AppDbContext context) => _context = context;

        public async Task<Result<BookingDTO>> AddBookingAsync(BookingCreationDTO BookingCreationDTO)
        {
            var UserGet = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == BookingCreationDTO.UserId);
            
            if (UserGet == null) 
                return Result<BookingDTO>.Error("Ошибка: пользователь не найден");

            var RoomGet = await _context.Rooms
                .FirstOrDefaultAsync(x => x.Id == BookingCreationDTO.RoomId);

            if (RoomGet == null) 
                return Result<BookingDTO>.Error("Ошибка: комната не найдена");
 
            // l2 validation
            if (RoomGet.Status == _enums.RoomStatus.Booked) 
                return Result<BookingDTO>.Error("Ошибка: комната уже забронировавна");
            if (UserGet.Balance < RoomGet.Price) 
                return Result<BookingDTO>.Error("Ошибка: недостаточно средств");

            var booking = new Booking(BookingCreationDTO.UserId, BookingCreationDTO.RoomId, BookingCreationDTO.StartTime, BookingCreationDTO.EndTime);
             _context.Add(booking);

            try
            {
                UserGet.Balance -= RoomGet.Price;
                RoomGet.Status = RoomStatus.Booked;
                booking.Status = BookingStatus.Confirmed;

                await _context.SaveChangesAsync();
                return Result<BookingDTO>.Success(new BookingDTO(booking));
            }
            catch
            {
                return Result<BookingDTO>.Error("Ошибка: БД не смогла сохранить изменения");
            }
        }
        public async Task<Result<BookingDTO>> GetBookingByIdAsync(int Id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(x => x.Id == Id);

            return booking != null ? Result<BookingDTO>.Success(new BookingDTO(booking)) : Result<BookingDTO>.Error("Ошибка: бронь не найдена");
        }
        public async Task<Result<List<BookingDTO>>> GetBookingByUserAsync(int UserId)
        {
            bool UserExists = await _context.Users.AnyAsync(x => x.Id == UserId);

            if (!UserExists)
                return Result<List<BookingDTO>>.Error("Ошибка: пользователь не найден");

            var bookings = _context.Bookings
                .Where(x => x.UserId == UserId)
                .Select(x => new BookingDTO(x))
                .ToList();

            return bookings.Any() ? Result<List<BookingDTO>>.Success(bookings) : Result<List<BookingDTO>>.Error("Ошибка: список заявок пуст"); 
        }
        public async Task<Result<List<BookingDTO>>> GetBookingByRoomAsync(int RoomId)
        {
            bool RoomExists = await _context.Rooms.AnyAsync(x => x.Id == RoomId);

            if (!RoomExists) 
                return Result<List<BookingDTO>>.Error("Ошибка: комната не найдена");

            var bookings = _context.Bookings
                .Where(x => x.RoomId == RoomId)
                .Select(x => new BookingDTO(x))
                .ToList();

            return bookings.Any() ? Result<List<BookingDTO>>.Success(bookings) : Result<List<BookingDTO>>.Error("Ошибка: список заявок пуст");
        }
        public async Task<Result<BookingDTO>> ChangeStatusAsync(int Id, BookingStatus status)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(x => x.Id == Id);
            
            if (booking == null) 
                return Result<BookingDTO>.Error("Ошибка: комната не найдена");

            booking.Status = status;
            try
            {
                await _context.SaveChangesAsync();
                return Result<BookingDTO>.Success(new BookingDTO(booking));
            }
            catch 
            {
                return Result<BookingDTO>.Error("Ошибка: БД не смогла сохранить изменения");
            }
        }
    }
}