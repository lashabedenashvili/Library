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

        [HttpPost("UserUpdate")]
        public async Task<ActionResult<ApiResponse<UserUpdateDto>>> UserUpdate(UserUpdateDto request)
        {
            int id = 4;
            return ResponseResult(await _userService.Update(id,request));
        }

        [HttpPost("PasswordChange")]
        public async Task<ActionResult<ApiResponse<string>>> PasswordChange(ChangePasswordDto request)
        {
            int id = 4;
            return ResponseResult(await _userService.UpdatePassword(request, id));
        }

        [HttpPost("LogIn")]
        public async Task<ActionResult<ApiResponse<string>>> LogIn(UserLogInDto request)
        {           
            return ResponseResult(await _userService.LogIn(request));
        }
    }
}
