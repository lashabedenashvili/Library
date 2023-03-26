using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Domein.Data
{
    public class Book:IGlobald
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public decimal? Rating { get; set; }
        public bool InLibrary { get; set; }
        public ICollection<BooksAuthors> BooksAuthors { get; set; }
    }
}
