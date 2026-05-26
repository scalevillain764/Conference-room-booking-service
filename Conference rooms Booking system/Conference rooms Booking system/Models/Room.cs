using _interfaces;
using _booking;
using _enums;
namespace _room
{
    public class Room : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Capacity { get; set; }
        public int Floor { get; set; }
        public decimal Price { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public RoomStatus Status { get; set;}
        public Room (string Name, double Capacity, int Floor, decimal Price, RoomStatus Status)
        {
            this.Name = Name;
            this.Capacity = Capacity;
            this.Floor = Floor;
            this.Price = Price;
            this.Status = Status;           
        }
        public Room() { }
    }
}