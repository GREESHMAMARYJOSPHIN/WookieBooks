using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using WookieBooks.DataContext;

namespace WookieBooks.Models
{
    public static class BooksDataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BooksDBContext(
                serviceProvider.GetRequiredService<DbContextOptions<BooksDBContext>>()))
            {
                if (!context.Books.Any())
                {
                    //Add sample books to in-memory database
                    context.Books.AddRange(
                        new Book
                        {
                            Id = 101,
                            Title = "BookSample1",
                            Description = "Description1",
                            Author = "Author1",
                            CoverImage = "/images/booksample1.jpg",
                            Price = 10.00M
                        },
                        new Book
                        {
                            Id = 102,
                            Title = "BookSample2",
                            Description = "Description2",
                            Author = "Author2",
                            CoverImage = "/images/booksample2.jpg",
                            Price = 20.00M
                        }
                        );
                    context.SaveChanges();
                }
            }
        }
    }
}
