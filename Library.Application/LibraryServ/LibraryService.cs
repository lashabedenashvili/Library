using Library.Data.Domein.Data;
using Library.DataBase.GeneralRepository;
using Library.Infrastructure.ApiServiceResponse;
using Library.Infrastructure.Dto.LibraryDto;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.LibraryServ
{
    public class LibraryService: ILibraryService
    {
        private readonly IGeneralRepository<Author> _authorRepo;
        private readonly IGeneralRepository<Book> _bookRepo;
        private readonly IGeneralRepository<BooksAuthors> _booksAuthorsRepo;

        public LibraryService
            (
            IGeneralRepository<Author>authorRepo,
            IGeneralRepository<Book> bookRepo,
            IGeneralRepository<BooksAuthors> BooksAuthorsRepo
            )
        {
            _authorRepo = authorRepo;
            _bookRepo = bookRepo;
            _booksAuthorsRepo = BooksAuthorsRepo;
        }
        private async Task<ApiResponse<byte[]>>ImageValidator(string image)
        {
            var bytedata = Convert.FromBase64String(image);
            using var stream = new MemoryStream(bytedata);
            var imgD = await Image.LoadAsync(stream);
            var format = Image.DetectFormat(bytedata);
            if (format.Name!= "PNG") return new BadApiResponse<byte[]>("Not Allowed Format");
            return new SuccessApiResponse<byte[]>(bytedata);
        }
        public async Task<ApiResponse<string>> AddBook(AddBookDto request)
        {
            var validateIamge = await ImageValidator(request.BookDto.Image);
            if (!validateIamge.Succes) return new BadApiResponse<string>(validateIamge.Message, validateIamge.ErrorCode);

            var bookAuthor = new List<BooksAuthors>();
            var bookDB= await _bookRepo.AnyAsync(x=>x.Title==request.BookDto.Title);
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
        public async Task<ApiResponse<UpdateBookDto>>UpdateBook(UpdateBookDto request, string bookName)
        {
            var bookDb = await _bookRepo.Where(x => x.Title == bookName).FirstOrDefaultAsync();
            if (bookDb != null)
            {
                bookDb.Title=request.Title==null? bookDb.Title:request.Title;
                bookDb.Description=request.Description==null? bookDb.Description:request.Description;
                bookDb.Rating=request.Rating==null?bookDb.Rating:request.Rating;    
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
    }
}
