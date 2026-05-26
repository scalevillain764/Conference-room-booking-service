using _booking;
using _bookingDto;
using _bookingRequest;
using _enums;
using _interfaces;
using _user;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
namespace _bookingController
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingConroller : ControllerBase
    {
        private readonly IBookingService _service;
        public BookingConroller(IBookingService service) => _service = service;

        [HttpPost]
        public async Task<ActionResult> AddBooking([FromBody] BookingCreationRequest request)
        {
            var rez_dto = BookingCreationDTO.Create(request.UserId, request.RoomId, request.StartTime, request.EndTime);
            if (!rez_dto.IsSuccess) return BadRequest(rez_dto.ErrorMSG);

            var rez = await _service.AddBookingAsync(rez_dto.Data);
            return rez.IsSuccess ? Ok(rez.Data) : BadRequest(rez_dto.ErrorMSG);
        }

        [HttpGet]
        [Route("{BookingId:int:min(1)}")]
        public async Task<IActionResult> GetBookingId (int BookingId)
        {
            var rez = await _service.GetBookingByIdAsync(BookingId);
            return rez.IsSuccess ? Ok(rez.Data) : NotFound(rez.ErrorMSG);
        }

        [HttpGet]
        [Route("user/{UserId:int:min(1)}")]
        public async Task<IActionResult> GetBookingByUser (int UserId)
        {
            var rez = await _service.GetBookingByUserAsync(UserId);
            return rez.IsSuccess ? Ok(rez) : NotFound(rez.ErrorMSG);
        }

        [HttpGet]
        [Route("room/{RoomId:int:min(1)}")]
        public async Task<IActionResult> GetBookingByRoom(int RoomId)
        {
            var rez = await _service.GetBookingByRoomAsync(RoomId);
            return rez.IsSuccess ? Ok(rez) : NotFound(rez.ErrorMSG);
        }

        [HttpPut]
        [Route("change-status/{BookingId:int:min(1)}")]
        public async Task<IActionResult> ChangeStatus(int BookingId, [FromQuery] string status)
        {
            if (string.IsNullOrWhiteSpace(status) || !Enum.TryParse<BookingStatus>(status, out var st)) return BadRequest("Ошибка: неверный статус");
            var rez = await _service.ChangeStatusAsync(BookingId, st);
            return rez.IsSuccess ? Ok(rez.Data) : BadRequest(rez.ErrorMSG);
        }
    }
}