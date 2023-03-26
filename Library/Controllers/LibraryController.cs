using Library.Application.LibraryServ;
using Library.Infrastructure.ApiServiceResponse;
using Library.Infrastructure.Dto.LibraryDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class LibraryController:BaseController
    {
        private readonly ILibraryService _libraryService;

        public LibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }
        [HttpPost("AddBook")]
        public async Task<ActionResult<ApiResponse<string>>> AddBook(AddBookDto request)
        {
            return ResponseResult(await _libraryService.AddBook(request));
        }

        [HttpPost("UpdateBook")]
        public async Task<ActionResult<ApiResponse<UpdateBookDto>>> UpdateBook(UpdateBookDto request,string bookName)
        {
            return ResponseResult(await _libraryService.UpdateBook(request,bookName));
        }

    }
}
