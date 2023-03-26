using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.ApiServiceResponse
{
    public class BadApiResponse<T>:ApiResponse<T>
    {
        public BadApiResponse(string message) : base(message, HttpStatusCode.BadRequest)
        {

        }
        public BadApiResponse(string message, string errorCode) : base(message, errorCode, HttpStatusCode.BadRequest)
        {

        }
    }
}
