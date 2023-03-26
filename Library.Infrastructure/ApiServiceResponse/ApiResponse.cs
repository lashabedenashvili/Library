using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.ApiServiceResponse
{
    public class ApiResponse<T>
    {
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public T Data { get; set; }
        public bool Succes { get; set; }
        public HttpStatusCode statusCode { get; set; }
        //private List<string> message1 { get; set; }
        public ApiResponse(string message)
        {
            Message = message;
        }
        public ApiResponse(T data)
        {

        }
        //public ApiResponse(List<string>message1)
        //{
        //    this.message1 = message1;
        //}
        public ApiResponse(T data, HttpStatusCode httpStatusCode)
        {
            Data = data;
            statusCode = httpStatusCode;
            Succes = (int)statusCode >= 200 && (int)statusCode < 300;
        }
        public ApiResponse(string message, HttpStatusCode httpStatusCode)
        {
            this.Message = message;
            statusCode = httpStatusCode;
            Succes = (int)statusCode >= 200 && (int)statusCode < 300;
        }
        public ApiResponse(string Message, string ErrorCode, HttpStatusCode httpStatusCode)
        {
            this.Message = Message;
            this.ErrorCode = ErrorCode;
            statusCode = httpStatusCode;
            Succes = (int)statusCode >= 200 && (int)statusCode < 300;
        }
        public ApiResponse(T data, string Message, HttpStatusCode httpStatusCode)
        {
            Data = data;
            this.Message = Message;
            statusCode = httpStatusCode;
            Succes = (int)statusCode >= 200 && (int)statusCode < 300;
        }

        public ApiResponse(T data, string Message, string ErrorCode, HttpStatusCode httpStatusCode)
        {
            Data = data;
            this.Message = Message;
            this.ErrorCode = ErrorCode;
            statusCode = httpStatusCode;
            Succes = (int)statusCode >= 200 && (int)statusCode < 300;
        }

    }
}
