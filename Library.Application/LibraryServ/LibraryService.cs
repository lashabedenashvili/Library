using Library.Data.Domein.Data;
using Library.DataBase.GeneralRepository;
using Library.Infrastructure.ApiServiceResponse;
using Library.Infrastructure.Dto.LibraryDto;
using Microsoft.EntityFrameworkCore;
namespace Library.Application.LibraryServ
{
    public class LibraryService : ILibraryService
    {
        private readonly IGeneralRepository<Author> _authorRepo;
        private readonly IGeneralRepository<Book> _bookRepo;
        private readonly IGeneralRepository<BooksAuthors> _booksAuthorsRepo;

        public LibraryService
            (
            IGeneralRepository<Author> authorRepo,
            IGeneralRepository<Book> bookRepo,
            IGeneralRepository<BooksAuthors> booksAuthorsRepo
            )
        {
            _authorRepo = authorRepo;
            _bookRepo = bookRepo;
            _booksAuthorsRepo = booksAuthorsRepo;
        }
        public async Task<ApiResponse<string>> BookStatusChange(int bookId, bool inLibrary)
        {
            var bookStatusDb = await _bookRepo
                 .Where(x => x.Id == bookId)
                 .FirstOrDefaultAsync();

            if (bookStatusDb == null) return new BadApiResponse<string>("Book does not exist");
            if (bookStatusDb.InLibrary == inLibrary) return new BadApiResponse<string>("");

            bookStatusDb.InLibrary = inLibrary;
            await _bookRepo.Update(bookStatusDb);
            await _bookRepo.SaveChangesAsync();
            return new SuccessApiResponse<string>("Book status has been changed");
        }
        private async Task<ApiResponse<byte[]>> ImageValidator(string image)
        {
            var bytedata = Convert.FromBase64String(image);
            using var stream = new MemoryStream(bytedata);
            var imgD = await Image.LoadAsync(stream);
            var format = Image.DetectFormat(bytedata);
            if (format.Name != "PNG") return new BadApiResponse<byte[]>("Not Allowed Format");
            return new SuccessApiResponse<byte[]>(bytedata);
        }
        public async Task<ApiResponse<string>> AddBook(AddBookDto request)
        {
            var validateIamge = await ImageValidator(request.BookDto.Image);
            if (!validateIamge.Succes) return new BadApiResponse<string>(validateIamge.Message, validateIamge.ErrorCode);

            var bookAuthor = new List<BooksAuthors>();
            var bookDB = await _bookRepo.AnyAsync(x => x.Title == request.BookDto.Title);
            if (!bookDB)
            {
                var book = new Book
                {
                    Title = request.BookDto.Title,
                    Description = request.BookDto.Description,
                    Image = validateIamge.Data,
                    InLibrary = true,
                    Rating = request.BookDto.Rating
                };
                var _book = await _bookRepo.AddAsync(book);

                foreach (var author in request.AuthorDto)
                {
                    var _author = await _authorRepo.Where(e => e.Name.ToLower() == author.Name.ToLower() &&
                                                               e.Surname.ToLower() == author.Surname.ToLower() &&
                                                               e.BirthDate == author.BirthDate)
                                                               .FirstOrDefaultAsync();

                    if (_author == null)
                    {
                        var authors = new Author
                        {
                            Name = author.Name,
                            Surname = author.Surname,
                            BirthDate = author.BirthDate
                        };
                        var newAuthor = await _authorRepo.AddAsync(authors);
                        var AuthorBook = new BooksAuthors()
                        {
                            Author = newAuthor,
                            Book = book
                        };
                        bookAuthor.Add(AuthorBook);
                    }
                    else
                    {
                        var AuthorBook = new BooksAuthors()
                        {
                            AuthorId = _author.Id,
                            BookId = book.Id
                        };
                        bookAuthor.Add(AuthorBook);
                    }
                }
                book.BooksAuthors = bookAuthor;
                await _bookRepo.SaveChangesAsync();
                return new SuccessApiResponse<string>("Book has been added");
            }
            return new BadApiResponse<string>("Book is already exist");
        }
        public async Task<ApiResponse<UpdateBookDto>> UpdateBook(UpdateBookDto request, string bookName)
        {
            var bookDb = await _bookRepo.Where(x => x.Title == bookName).FirstOrDefaultAsync();
            if (bookDb != null)
            {
                bookDb.Title = request.Title == null ? bookDb.Title : request.Title;
                bookDb.Description = request.Description == null ? bookDb.Description : request.Description;
                bookDb.Rating = request.Rating == null ? bookDb.Rating : request.Rating;
                await _bookRepo.SaveChangesAsync();
                return new SuccessApiResponse<UpdateBookDto>(new UpdateBookDto
                {
                    Title = bookDb.Title,
                    Description = bookDb.Description,
                    Rating = bookDb.Rating,
                });
            }
            return new BadApiResponse<UpdateBookDto>("Book is not exist");
        }
        public async Task<ApiResponse<List<GetBooksDto>>> GetBooksByFiltering(FilterBookDto request)
        {

            var data = _bookRepo.AsQuareble()
                .Where(x => x.InLibrary == true)
                .Include(e => e.BooksAuthors)
                .ThenInclude(e => e.Author).AsQueryable();

            if (request.Id.HasValue)
                data = data.Where(x => x.Id == request.Id.Value);
            if (request.InLibrary.HasValue)
                data = data.Where(x => x.InLibrary == true);
            if (request.RatingFrom.HasValue)
                data = data.Where(x => x.Rating >= request.RatingFrom);
            if (request.RatingTo.HasValue)
                data = data.Where(x => x.Rating <= request.RatingTo);
            if (!string.IsNullOrEmpty(request.Title))
                data = data.Where(x => x.Title.Contains(request.Title));
            if (!string.IsNullOrEmpty(request.Description))
                data = data.Where(x => x.Description.Contains(request.Description));

            data = data.Skip((request.PageNumb - 1) * request.PageSize).Take(request.PageSize);

            var result = await data.ToListAsync();

            return new SuccessApiResponse<List<GetBooksDto>>(result.Select(e =>
           {
               return new GetBooksDto()
               {
                   Id = e.Id,
                   Title = e.Title,
                   Description = e.Description,
                   InLibrary = e.InLibrary,
                   Rating = e.Rating,
                   Image = Convert.ToBase64String(e.Image),
                   Author = e.BooksAuthors.SelectMany(e => new List<AuthorDto>() { new AuthorDto()
                   {
                       Name = e.Author.Name,
                       Surname = e.Author.Surname,
                       BirthDate = e.Author.BirthDate
                   } }).ToList()
               };
           }).ToList());

        }
        public async Task<ApiResponse<List<GetBookByAutorDto>>> GetBookByAutor(AuthorDto request)
        {
            var data = _authorRepo
                .AsQuareble()
                 .AsQueryable();
          
            if (!string.IsNullOrEmpty(request.Name))
                data = data.Where(x => x.Name.Contains(request.Name));
            if (!string.IsNullOrEmpty(request.Surname))
                data = data.Where(x => x.Surname.Contains(request.Surname));

            var result = await data
                .Include(x => x.BooksAuthors)
                .ThenInclude(x => x.Book)
                .SelectMany(x => x.BooksAuthors)
                .Select(x => x.Book).Where(x=>x.InLibrary==true)
                .ToListAsync();

            return new SuccessApiResponse<List<GetBookByAutorDto>>(result.Select(x =>
            {
                return new GetBookByAutorDto()
                {
                    Description = x.Description,
                    Id = x.Id,
                    Image = Convert.ToBase64String(x.Image),
                    InLibrary = x.InLibrary,
                    Rating = x.Rating,
                    Title = x.Title
                };
            }).ToList());


        }
        public async Task<ApiResponse<string>>DeleteBook(int bookId)
        {
            var bookDb = await _bookRepo.Where(x => x.Id == bookId).FirstOrDefaultAsync();
            if (bookDb == null) return new BadApiResponse<string>("Book does not exist");
            await _bookRepo.Delete(bookDb);
            await _bookRepo.SaveChangesAsync();
            return new SuccessApiResponse<string>("The book has been deleted succesfuly");
        }
    }
}
