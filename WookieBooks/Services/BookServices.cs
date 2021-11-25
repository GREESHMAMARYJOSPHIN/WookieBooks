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

        /// <summary>
        /// Add book record to the existing book collection
        /// </summary>
        /// <param name="book"></param>
        /// <returns value="book">new book record added to book collection</returns>
        public async Task<Book> AddBook(Book book)
        {
            if(book != null)
            {    
                //Add the book object in request to Books collection
                _bookDbContext.Books.Add(book);
                
                await _bookDbContext.SaveChangesAsync();
                return book;
            }
            return null;
        }

        /// <summary>
        /// Delete the book record corresponding to the requested id
        /// </summary>
        /// <param name="id"></param>
        /// <returns value="book">Deleted book record</returns>
        public async Task<Book> DeleteBook(int? id)
        {
            //get the existing record corresponding to the id in request
            var book = _bookDbContext.Books.FirstOrDefault(x => x.Id == id);
            if(book != null) //check if the requested record exists
            {
                //set the entity state of the existing record to Deleted
                _bookDbContext.Entry(book).State = EntityState.Deleted;

                await _bookDbContext.SaveChangesAsync();
                return book;
            }
            return null;
        }

        /// <summary>
        /// Update the book record with the book in request
        /// </summary>
        /// <param name="book"></param>
        /// <returns value="book">Updated book record</returns>
        public async Task<Book> EditBook(Book book)
        {
            //get the existing record corresponding to the id in the request
            var existingBook = await _bookDbContext.Books.FirstOrDefaultAsync(Book => Book.Id == book.Id);
            if (existingBook != null) //check if record exists
            {
                //update existing book record with the values in the request
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

        /// <summary>
        /// Get book record corresponding to id in request
        /// </summary>
        /// <param name="id"></param>
        /// <returns value="book">Book record corresponding to id</returns>
        public async Task<Book> GetBookById(int? id)
        {
            //get the book record corresponding to the id in the request
            var book = await _bookDbContext.Books.FirstOrDefaultAsync(x => x.Id == id);

            return book;
        }

        /// <summary>
        /// Get all books in the collection
        /// </summary>
        /// <returns value = "List<Book>">List of all the books</returns>
        public async Task<IEnumerable<Book>> GetBooks()
        {
            //get the list of all books in the collection
            var books = await _bookDbContext.Books.ToListAsync();

            return books;
        }
    }
}
