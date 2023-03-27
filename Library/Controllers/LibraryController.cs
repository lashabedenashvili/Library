using Library.Application.LibraryServ;
using Library.Infrastructure.ApiServiceResponse;
using Library.Infrastructure.Dto.LibraryDto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class LibraryController : BaseController
    {
        private readonly ILibraryService _libraryService;

        public LibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }


        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("AddBook")]        
        public async Task<ActionResult<ApiResponse<string>>> AddBook(AddBookDto request)
        {
            return ResponseResult(await _libraryService.AddBook(request));
        }


        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("UpdateBook")]
        public async Task<ActionResult<ApiResponse<UpdateBookDto>>> UpdateBook(UpdateBookDto request, string bookName)
        {
            return ResponseResult(await _libraryService.UpdateBook(request, bookName));
        }


        [Authorize(Roles="member",AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("GetBooks")]
        public async Task<ActionResult<ApiResponse<List<GetBooksDto>>>> FilterBook(FilterBookDto request)
        {
            return ResponseResult(await _libraryService.GetBooksByFiltering(request));
        }

        [Authorize(Roles = "member", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("BookStatusChange")]
        public async Task<ActionResult<ApiResponse<string>>> BookStatusChange(int bookId, bool inLibrary)
        {
            return ResponseResult(await _libraryService.BookStatusChange(bookId, inLibrary));
        }

        [Authorize(Roles = "member", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]        
        [HttpPost("GetBooksByAuthor")]
        public async Task<ActionResult<ApiResponse<List<GetBookByAutorDto>>>> GetBookByAutor(AuthorDto request)
        {
            return ResponseResult(await _libraryService.GetBookByAutor(request));
        }


        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("DeleteBook")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteBook(int bookId)
        {
            return ResponseResult(await _libraryService.DeleteBook(bookId));
        }
    }
}
