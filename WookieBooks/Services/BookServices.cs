using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WookieBooks.DataContext;
using WookieBooks.IServices;
using WookieBooks.Models;

namespace WookieBooks.Services
{
    public class BookServices : IBookServices
    {
        private readonly BooksDBContext _bookDbContext;

        public BookServices(BooksDBContext booksDBContext)
        {
            _bookDbContext = booksDBContext;
        }

        public async Task<Book> AddBook(Book book)
        {
            if(book != null)
            {
                _bookDbContext.Books.Add(book);
                await _bookDbContext.SaveChangesAsync();
                return book;
            }
            return null;
        }

        public async Task<Book> DeleteBook(int? id)
        {
            var book = _bookDbContext.Books.FirstOrDefault(x => x.Id == id);
            if(book != null)
            {
                _bookDbContext.Entry(book).State = EntityState.Deleted;
                await _bookDbContext.SaveChangesAsync();
                return book;
            }
            return null;
        }

        public async Task<Book> EditBook(Book book)
        {
            var existingBook = await _bookDbContext.Books.FirstOrDefaultAsync(Book => Book.Id == book.Id);
            if (existingBook != null)
            {
                existingBook.Title = book.Title == null ? existingBook.Title : book.Title ;
                existingBook.Description = book.Description == null ? existingBook.Description : book.Description;
                existingBook.CoverImage = book.CoverImage == null? existingBook.CoverImage : book.CoverImage;
                existingBook.Author = book.Author == null ? existingBook.Author : book.Author;
                existingBook.Price = book.Price == 0 ? existingBook.Price : book.Price;
                await _bookDbContext.SaveChangesAsync();
                return existingBook;
            }
            return null;
        }

        public async Task<Book> GetBookById(int? id)
        {
            var book = await _bookDbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
            return book;
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            var books = await _bookDbContext.Books.ToListAsync();
            return books;
        }
    }
}
