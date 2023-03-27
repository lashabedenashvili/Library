using Library.Application.UserServ;
using Library.Infrastructure.ApiServiceResponse;
using Library.Infrastructure.Dto.UserDto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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


        [Authorize(Roles="admin",AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("UserDelete")]
        public async Task<ActionResult<ApiResponse<string>>> UserDelete()
        {
            var id = GetId();
            return ResponseResult(await _userService.Delete(id));
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("UserUpdate")]
        public async Task<ActionResult<ApiResponse<UserUpdateDto>>> UserUpdate(UserUpdateDto request)
        {
            var id = GetId();
            return ResponseResult(await _userService.Update(id,request));
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("PasswordChange")]
        public async Task<ActionResult<ApiResponse<string>>> PasswordChange(ChangePasswordDto request)
        {
            var id = GetId();
            return ResponseResult(await _userService.UpdatePassword(request, id));
        }


        [HttpPost("LogIn")]
       
        public async Task<ActionResult<ApiResponse<string>>> LogIn(UserLogInDto request)
        {            
            return ResponseResult(await _userService.LogIn(request));
        }
    }
}
