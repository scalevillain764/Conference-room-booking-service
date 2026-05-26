using _data;
using _enums;
using _interfaces;
using _result;
using _room;
using _roomDto;
using Microsoft.EntityFrameworkCore;
namespace _roomService
{
    public class RoomService : IRoomService
    {
        private readonly AppDbContext _context;
        public RoomService(AppDbContext context) => _context = context;
        private async Task<Result<RoomDTO>> UpdateRoomPropertyAsync(int Id, Action<Room> updateAction)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(x=> x.Id == Id);
            
            if (room == null)
                return Result<RoomDTO>.Error("Ошибка: комната не найдена");

            updateAction(room);

            try
            {
                await _context.SaveChangesAsync();
                return Result<RoomDTO>.Success(new RoomDTO(room));
            }
            catch
            {
                return Result<RoomDTO>.Error("Ошибка: БД не смогла сохранить изменения");
            }
        } 
        public async Task<Result<RoomDTO>> AddRoomAsync(RoomCreationDTO RoomCreationDTO) 
        { 
            var room = new Room(RoomCreationDTO.Name, RoomCreationDTO.Capacity, RoomCreationDTO.Floor, RoomCreationDTO.Price, RoomCreationDTO.Status);
            _context.Rooms.Add(room);

            try
            {
                await _context.SaveChangesAsync();
                return Result<RoomDTO>.Success(new RoomDTO(room));
            }
            catch
            {
                return Result<RoomDTO>.Error("Ошибка: БД не смогла сохранить изменения");
            }
        }
        public async Task<Result<RoomDTO>> RemoveRoomAsync(int Id)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (room == null) 
                return Result<RoomDTO>.Error("Ошибка: комната не найдена");

            _context.Remove(room);
            
            try
            {
                await _context.SaveChangesAsync();
                return Result<RoomDTO>.Success(null);
            }
            catch
            {
                return Result<RoomDTO>.Error("Ошибка: БД не смогла сохранить изменения");
            }
        }
        public async Task<Result<RoomDTO>> GetRoomAsync(int Id)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(x => x.Id == Id);
            return room != null ? Result<RoomDTO>.Success(new RoomDTO(room)) : Result<RoomDTO>.Error("Ошибка: комната не найдена");
        }
        public async Task<Result<List<RoomDTO>>> GetAllRoomsAsync() {
            bool RoomExists = await _context.Rooms.AnyAsync();

            if (!RoomExists) 
                return Result<List<RoomDTO>>.Error("Ошибка: комнат нет");

            return Result<List<RoomDTO>>.Success(_context.Rooms.Select(x => new RoomDTO(x)).ToList());
        }
        public async Task<Result<RoomDTO>> ChangeNameAsync(int Id, string Name) => await UpdateRoomPropertyAsync(Id, room => room.Name = Name);
        public async Task<Result<RoomDTO>> ChangeCapacityAsync(int Id, double Capacity) => await UpdateRoomPropertyAsync(Id, room => room.Capacity = Capacity);
        public async Task<Result<RoomDTO>> ChangeFloorAsync(int Id, int Floor) => await UpdateRoomPropertyAsync(Id, room => room.Floor = Floor);
        public async Task<Result<RoomDTO>> ChangePriceAsync(int Id, decimal Price) => await UpdateRoomPropertyAsync(Id, room => room.Price = Price);
        public async Task<Result<RoomDTO>> ChangeStatusAsync(int Id, RoomStatus status) => await UpdateRoomPropertyAsync(Id, room => room.Status = status);
    }
}