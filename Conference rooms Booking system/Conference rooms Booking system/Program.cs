using _bookingService;
using _data;
using _interfaces;
using _roomService;
using _userService;
using Microsoft.EntityFrameworkCore;
namespace Conference_rooms_Booking_system
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(x => {
                x.UseSqlite("Data Source=BookingSystem.db");
            });

            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IBookingService, BookingService>();                   
            builder.Services.AddRazorPages();
            builder.Services.AddControllers();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.MapControllers();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
