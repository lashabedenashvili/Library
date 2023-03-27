using Library.Infrastructure.ApiServiceResponse;
using Library.Infrastructure.Dto.LibraryDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.LibraryServ
{
    public interface ILibraryService
    {
        Task<ApiResponse<string>> AddBook(AddBookDto request);
        Task<ApiResponse<UpdateBookDto>> UpdateBook(UpdateBookDto request, int bookId);
        Task<ApiResponse<List<GetBooksDto>>> GetBooksByFiltering(FilterBookDto request);
        Task<ApiResponse<string>> BookStatusChange(int bookId, bool inLibrrary);
        Task<ApiResponse<List<GetBookByAutorDto>>> GetBookByAutor(AuthorDto request);
        Task<ApiResponse<string>> DeleteBook(int bookId);
    }
}
