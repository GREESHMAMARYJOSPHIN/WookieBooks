using WookieBooks.DataContext;
using WookieBooks.Models;

namespace WookieBooks.Test
{
    public class DummyDataInitializer
    {
        public void Seed(BooksDBContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Books.AddRange(
                 new Book
                 {
                     Id = 1001,
                     Title = "TestSample1",
                     Description = "Description1",
                     Author = "Author1",
                     CoverImage = "/images/sample1.jpg",
                     Price = 10.00M
                 },
                new Book
                {
                    Id = 1002,
                    Title = "TestSample2",
                    Description = "Description2",
                    Author = "Author2",
                    CoverImage = "/images/sample2.jpg",
                    Price = 20.00M
                },
                new Book
                {
                    Id = 1003,
                    Title = "TestSample3",
                    Description = "Description3",
                    Author = "Author3",
                    CoverImage = "/images/sample3.jpg",
                    Price = 30.00M
                },
                new Book
                {
                    Id = 1004,
                    Title = "TestSample4",
                    Description = "Description4",
                    Author = "Author4",
                    CoverImage = "/images/sample4.jpg",
                    Price = 40.00M
                }

                );
            context.SaveChanges();
        }
    }
}
