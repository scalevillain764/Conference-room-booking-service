namespace _roomRequest
{
    public class RoomCreationRequest
    {
        public string Name { get; set; }
        public double Capacity { get; set; } 
        public int Floor { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
    }
}