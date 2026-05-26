namespace _bookingRequest
{
    public class BookingCreationRequest
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}