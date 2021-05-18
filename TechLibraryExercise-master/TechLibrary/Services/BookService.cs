using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechLibrary.Data;
using TechLibrary.Domain;
using TechLibrary.Models;

namespace TechLibrary.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(int bookid);

        Task<Book> UpadteBookByIdAsync(Book book);
        Task<Book> CreateBookAsync(Book book);
    }
    public class BookService : IBookService
    {
        private readonly DataContext _dataContext;

        public BookService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            var queryable = _dataContext.Books.AsQueryable();

            return await queryable.ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int bookid)
        {
            return await _dataContext.Books.SingleOrDefaultAsync(x => x.BookId == bookid);
        }

        public async Task<Book> UpadteBookByIdAsync(Book book)
        {
            Book oldBook = await _dataContext.Books.FindAsync(book.BookId);

            oldBook.Title = book.Title;
            oldBook.ISBN = book.ISBN;
            oldBook.PublishedDate = book.PublishedDate;
            oldBook.ThumbnailUrl = book.ThumbnailUrl;
            oldBook.ShortDescr = book.ShortDescr;
            await _dataContext.SaveChangesAsync();
            return book;
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            _dataContext.Books.Add(book);
            await _dataContext.SaveChangesAsync();
            return book;
        }
    }
}
