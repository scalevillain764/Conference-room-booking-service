 using _enums;
using _result;
using _booking;
namespace _bookingDto
{
    public class BookingCreationDTO
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public BookingCreationDTO(int UserId, int RoomId, DateTime StartTime, DateTime EndTime)
        {
            this.UserId = UserId;
            this.RoomId = RoomId;
            this.StartTime = StartTime;
            this.EndTime = EndTime;
        }
        public static Result<BookingCreationDTO> Create(int UserId, int RoomId, string StartTime_Str, string EndTime_Str)
        {
            if (UserId <= 0) return Result<BookingCreationDTO>.Error("Ошибка: неверный номер пользователя");
            if (RoomId <= 0) return Result<BookingCreationDTO>.Error("Ошибка: неверный номер комнаты");
            DateTime _startTime, _endTime;
            if(!DateTime.TryParse(StartTime_Str, out _startTime) || !DateTime.TryParse(EndTime_Str, out _endTime) ||
                _startTime > _endTime || _startTime < DateTime.UtcNow) return Result<BookingCreationDTO>.Error("Ошибка: неверное время");
            return Result<BookingCreationDTO>.Success(new BookingCreationDTO(UserId, RoomId, _startTime, _endTime));
        }
    }

    public class BookingDTO
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public BookingDTO (Booking booking)
        {
            UserId = booking.UserId;
            RoomId = booking.RoomId;
            StartTime = booking.StartTime;
            EndTime = booking.EndTime;
            Status = booking.Status.ToString();
        }
    }
}