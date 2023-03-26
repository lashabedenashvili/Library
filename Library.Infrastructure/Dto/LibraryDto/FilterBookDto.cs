using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Dto.LibraryDto
{
    public class FilterBookDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? RatingFrom { get; set; }
        public decimal? RatingTo { get; set; }
        public bool? InLibrary { get; set; }
        public int PageSize { get; set; } = 20;
        public int PageNumb { get; set; } = 1;
    }
}
