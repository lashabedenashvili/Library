using Library.Application.UserServ;
using Library.Infrastructure.ApiServiceResponse;
using Library.Infrastructure.Dto.UserDto;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class UserController:BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("UserRegistration")]
        public async Task<ActionResult<ApiResponse<string>>> UserRegistration(UserRegistrationDto request)
        {
            return ResponseResult(await _userService.Registration(request));
        }

        [HttpDelete("UserDelete")]
        public async Task<ActionResult<ApiResponse<string>>> UserDelete(int id)
        {
            return ResponseResult(await _userService.Delete(id));
        }
    }
}
