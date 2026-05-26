using _roomService;
using _roomRequest;
using _enums;
using _interfaces;
using _roomDto;
using _result;
using Microsoft.AspNetCore.Mvc;
namespace _roomController
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _service;
        public RoomController(IRoomService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> AddRoom([FromBody] RoomCreationRequest request)
        {
            var rez_dto = RoomCreationDTO.Create(request.Name, request.Capacity, request.Floor, request.Price, request.Status);
            if (!rez_dto.IsSuccess) return BadRequest(rez_dto.ErrorMSG);

            var rez = await _service.AddRoomAsync(rez_dto.Data);
            return rez.IsSuccess ? Ok(rez.Data) : BadRequest(rez.ErrorMSG);
        }

        [HttpGet]
        [Route("{RoomId:int:min(1)}")]
        public async Task<IActionResult> GetRoomAsync(int RoomId)
        {
            var rez = await _service.GetRoomAsync(RoomId);
            return rez.IsSuccess ? Ok(rez.Data) : NotFound(rez.ErrorMSG);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRooms()
        {
            var rez = await _service.GetAllRoomsAsync();
            return rez.IsSuccess ? Ok(rez) : NotFound(rez.ErrorMSG);
        }

        [HttpPut]
        [Route("change-name/{RoomId:int:min(1)}")]
        public async Task<IActionResult> ChangeName(int RoomId, [FromQuery] string Name)
        {
            if (string.IsNullOrWhiteSpace(Name)) return BadRequest("Ошибка: неверное название");
            var rez = await _service.ChangeNameAsync(RoomId, Name);
            return rez.IsSuccess ? Ok(rez.Data) : BadRequest(rez.ErrorMSG);
        }

        [HttpPut]
        [Route("change-capacity/{RoomId:int:min(1)}")]
        public async Task<IActionResult> ChangeCapacity(int RoomId, [FromQuery] double Capacity)
        {
            if (Capacity <= 0) return BadRequest("Ошибка: неверная площадь");
            var rez = await _service.ChangeCapacityAsync(RoomId, Capacity);
            return rez.IsSuccess ? Ok(rez.Data) : BadRequest(rez.ErrorMSG);
        }

        [HttpPut]
        [Route("change-floor/{RoomId:int:min(1)}")]
        public async Task<IActionResult> ChangeFloor(int Id, [FromQuery] int Floor)
        {
            if (Floor <= 0) return BadRequest("Ошибка: неверный этаж");
            var rez = await _service.ChangeFloorAsync(Id, Floor);
            return rez.IsSuccess ? Ok(rez.Data) : BadRequest(rez.ErrorMSG);
        }

        [HttpPut]
        [Route("change-price/{RoomId:int:min(1)}")] 
        public async Task<IActionResult> ChangePrice(int Id, [FromQuery] decimal Price)
        {
            if (Price <= 0) return BadRequest("Ошибка: неверная цена");
            var rez = await _service.ChangePriceAsync(Id, Price);
            return rez.IsSuccess ? Ok(rez.Data) : BadRequest(rez.ErrorMSG);
        }

        [HttpPut]
        [Route("change-status/{RoomId:int:min(1)}")]
        public async Task<IActionResult> ChangeStatus (int RoomId, [FromQuery] string Status)
        {
            if (!Enum.TryParse<RoomStatus>(Status, out var st)) return BadRequest("Ошибка: неверный статус");
            var rez = await _service.ChangeStatusAsync(RoomId, st);
            return rez.IsSuccess ? Ok(rez.Data) : BadRequest(rez.ErrorMSG);
        }
    }
}  