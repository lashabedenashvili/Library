using Library.Data.Domein.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Dto.LibraryDto
{
    public class GetBooksDto : BookDto
    {
        public int Id { get; set; }
        public bool InLibrary { get; set; }
        public List<AuthorDto> Author { get; set; }

    }

    
}
