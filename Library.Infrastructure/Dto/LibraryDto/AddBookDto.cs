using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Dto.LibraryDto
{
    public class AddBookDto
    {
        public BookDto BookDto { get; set; }
        public List<AuthorDto> AuthorDto { get; set; }
    }
}
