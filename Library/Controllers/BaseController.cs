using Library.Infrastructure.ApiServiceResponse;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController: ControllerBase
    {
        protected ActionResult<ApiResponse<T>> ResponseResult<T>(ApiResponse<T> apiResponse)
        {
            switch (apiResponse.statusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return Ok(apiResponse);
                case System.Net.HttpStatusCode.BadRequest:
                    return BadRequest(apiResponse);
                default:
                    return Ok(apiResponse);
            }
        }
        protected string GetEmail()
        {
            return User.FindFirstValue(ClaimTypes.Email);
        }

        protected int GetId()
        {
            return Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}
