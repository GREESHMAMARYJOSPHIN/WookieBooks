using Microsoft.EntityFrameworkCore;
using WookieBooks.Models;

namespace WookieBooks.DataContext
{
    public class BooksDBContext : DbContext
    {
        public BooksDBContext(DbContextOptions<BooksDBContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}
