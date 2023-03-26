using Library.Infrastructure.ApiServiceResponse;
using Library.Infrastructure.Dto.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UserServ
{
    public interface IUserService
    {
        Task<ApiResponse<string>> Registration(UserRegistrationDto request);
        Task<ApiResponse<string>> Delete(int id);
        Task<ApiResponse<UserUpdateDto>> Update(int id, UserUpdateDto request);
        Task<ApiResponse<string>> UpdatePassword(ChangePasswordDto request, int id);
        Task<ApiResponse<string>> LogIn(UserLogInDto request);
    }
}
