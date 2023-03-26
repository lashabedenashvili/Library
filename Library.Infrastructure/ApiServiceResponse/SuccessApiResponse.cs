using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.ApiServiceResponse
{
    public class SuccessApiResponse<T>:ApiResponse<T>
    {
        public SuccessApiResponse(T data) : base(data, HttpStatusCode.OK)
        {

        }
        public SuccessApiResponse(T data, string message) : base(data, message, HttpStatusCode.OK)
        {

        }
    }
}
