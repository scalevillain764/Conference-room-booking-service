using _room;
using _enums;
using _user;
using _interfaces;
namespace _booking
{
    public class Booking : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; } = null;
        public int RoomId { get; set; }
        public Room? Room { get; set; } = null;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public  BookingStatus Status { get; set; }
        public Booking(int UserId, int RoomId, DateTime StartTime, DateTime EndTime)
        {
            Id = 0;
            this.UserId = UserId;
            this.RoomId = RoomId;
            this.StartTime = StartTime;
            this.EndTime = EndTime;
            Status = BookingStatus.Pending;
        }
        public Booking() { }
    }
}