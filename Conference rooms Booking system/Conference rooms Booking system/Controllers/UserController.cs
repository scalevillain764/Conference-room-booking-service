using _interfaces;
using _userDto;
using _userRequest;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
namespace _userController
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController (IUserService service) => _service = service;

        [HttpPost]     
        public async Task<IActionResult> AddUser ([FromBody] UserCreationRequest request)
        {
            var dto = UserCreationDTO.Create(request.Name, request.Email, request.Role);
            if (!dto.IsSuccess) return BadRequest(dto.ErrorMSG);

            var rez = await _service.AddUserAsync(dto.Data);
            return rez.IsSuccess ? Ok(rez.Data) : BadRequest(dto.ErrorMSG);
        }

        [HttpDelete]
        [Route("{UserId:int:min(1)}")]
        public async Task<IActionResult> RemoveUser(int UserId)
        {
           var rez = await _service.RemoveUserAsync(UserId);
           return rez.IsSuccess ? Ok(rez.Data) : BadRequest(rez.ErrorMSG);
        }

        [HttpGet]
        [Route("{UserId:int:min(1)}")]
        public async Task<IActionResult> GetUser(int UserId)
        {
            var rez = await _service.GetUserAsync(UserId);
            return rez.IsSuccess ? Ok(rez.Data) : NotFound(rez.ErrorMSG);
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            var rez = await _service.GetAllUsersAsync();
            return rez.IsSuccess ? Ok(rez.Data) : NotFound(rez.ErrorMSG);
        }

        [HttpPut]
        [Route("change-name/{UserId:int:min(1)}")]
        public async Task<IActionResult> ChangeUserName(int UserId, [FromQuery] string Name)
        {
            if (string.IsNullOrWhiteSpace(Name)) return BadRequest("Ошибка: неверное имя");
            var rez = await _service.ChangeNameAsync(UserId, Name);
            return rez.IsSuccess ? Ok(rez.Data) : BadRequest(rez.ErrorMSG);
        }

        [HttpPut]
        [Route("change-email/{UserId:int:min(1)}")]
        public async Task<IActionResult> ChangeUserEmail(int UserId, [FromQuery] string Email)
        {
            if (string.IsNullOrWhiteSpace(Email)) return BadRequest("Ошибка: неверный email");
            var rez = await _service.ChangeEmailAsync(UserId, Email);
            return rez.IsSuccess ? Ok(rez.Data) : BadRequest(rez.ErrorMSG);
        }

        [HttpPut]
        [Route("replenish/{UserId:int:min(1)}")]
        public async Task<IActionResult> Replenish(int UserId, [FromQuery] decimal Amount)
        {
            if (Amount <= 0) return BadRequest("Ошибка: неверная сумма");
            var rez = await _service.ReplenishBalanceAsync(UserId, Amount);
            return rez.IsSuccess ? Ok(rez.Data) : BadRequest(rez.ErrorMSG);
        }

        [HttpGet]
        [Route("active/{UserId:int:min(1)}")]
        public async Task<IActionResult> GetActiveBookings(int UserId)
        {
            var rez = await _service.GetAllActiveBookingsAsync(UserId);
            return rez.IsSuccess ? Ok(rez.Data) : NotFound(rez.ErrorMSG);
        }

        [HttpGet]
        [Route("history/{UserId:int:min(1)}")]
        public async Task<IActionResult> GetHistoryBookings(int UserId)
        {
            var rez = await _service.GetHistoryBookingsAsync(UserId);
            return rez.IsSuccess ? Ok(rez.Data) : NotFound(rez.ErrorMSG);
        }
    }
}
