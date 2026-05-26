using _enums;
using _room;
using _result;
using _bookingDto;
namespace _roomDto
{
    public class RoomCreationDTO
    {
        public string Name { get; set; }
        public double Capacity { get; set; }
        public int Floor { get; set; }
        public decimal Price { get; set; } 
        public RoomStatus Status { get; set; }
        public RoomCreationDTO(string name, double capacity, int floor, decimal price, RoomStatus status)
        {
            Name = name;
            Capacity = capacity;
            Floor = floor;
            Price = price;
            Status = status;
        }
        public static Result<RoomCreationDTO> Create(string Name, double Capacity, int Floor, decimal Price, string RoomStatus_str)
        {
            if (string.IsNullOrWhiteSpace(Name)) return Result<RoomCreationDTO>.Error("Ошибка: неверноре название комнаты");
            if (Capacity <= 0) return Result<RoomCreationDTO>.Error("Ошибка: неверная площадь");
            if (Floor <= 0) return Result<RoomCreationDTO>.Error("Ошибка: неверный этаж");
            if (Price <= 0) return Result<RoomCreationDTO>.Error("Ошибка: неверная цена");
            RoomStatus t_status;
            if(!Enum.TryParse<RoomStatus>(RoomStatus_str, out t_status)) return Result<RoomCreationDTO>.Error("Ошибка: неверный статус");
            return Result<RoomCreationDTO>.Success(new RoomCreationDTO(Name, Capacity, Floor, Price, t_status));
        }
    }

    public class RoomDTO
    {
        public string Name { get; set; }
        public double Capacity { get; set; }
        public int Floor { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public List<BookingDTO> Bookings { get; set; }
        public RoomDTO(Room room)
        {
            Name = room.Name;
            Capacity = room.Capacity;
            Floor = room.Floor;
            Price = room.Price;
            Status = room.Status.ToString();
            Bookings = room.Bookings.Select(x => new BookingDTO(x)).ToList();
        }
    }
}