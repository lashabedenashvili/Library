﻿using Library.Infrastructure.ApiServiceResponse;
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
        Task<ApiResponse<UpdateBookDto>> UpdateBook(UpdateBookDto request, string bookName);
    }
}