using _interfaces;
using _enums;
using _booking;
namespace _user
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public decimal Balance { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public User(string name, string email, Role role, decimal balance)
        {
            Id = 0;
            Name = name;
            Email = email;
            Role = role;
            Balance = balance;
        }
        public User (string name, string email, Role role)
        {
            Id = 0;
            Name = name;
            Email = email;
            Role = role;
            Balance = 0;
        }
        public User() { }
    }
}