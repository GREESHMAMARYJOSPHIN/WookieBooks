using System.Collections.Generic;
using System.Threading.Tasks;
using WookieBooks.Models;

namespace WookieBooks.IServices
{
    public interface IBookServices
    {
        Task<IEnumerable<Book>> GetBooks();
        Task<Book> GetBookById(int? id);
        Task<Book> AddBook(Book book);
        Task<Book> EditBook(Book book);
        Task<Book> DeleteBook(int? id);

    }
}
