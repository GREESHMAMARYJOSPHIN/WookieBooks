using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WookieBooks.Controllers;
using WookieBooks.DataContext;
using WookieBooks.Logger;
using WookieBooks.Models;
using WookieBooks.Services;
using Xunit;

namespace WookieBooks.Test
{
    public class TestController
    {
        private readonly BookServices bookServices;
        private readonly BooksController controller;
        private readonly LoggerManager logger;
        public static DbContextOptions<BooksDBContext> dbContextOptions { get; }

        static TestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<BooksDBContext>()
                .UseInMemoryDatabase(databaseName: "TestBooks").Options;
        }

        public TestController()
        {
            var context = new BooksDBContext(dbContextOptions);
            DummyDataInitializer db = new DummyDataInitializer();
            db.Seed(context);

            bookServices = new BookServices(context);
            logger = new LoggerManager();
            controller = new BooksController(bookServices, logger);
        }

        #region Get Book by Id
        [Fact]
        public async Task Task_GetBookById_Return_OkResult()
        {
            //Arrange
            var bookId = 1001;

            //Act
            var data = await controller.GetBook(bookId);

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async Task Task_GetBookById_Return_NotFoundResult()
        {
            //Arrange
            var bookId = 2001;

            //Act
            var data = await controller.GetBook(bookId);

            //Assert
            Assert.IsType<NotFoundObjectResult>(data);
        }

        [Fact]
        public async Task Task_GetBookById_Return_BadRequestResult()
        {
            //Arrange
            int? bookId = null;

            //Act
            var data = await controller.GetBook(bookId);

            //Assert
            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public async Task Task_GetBookById_Return_MatchResult()
        {
            //Arrange
            int? bookId = 1002;

            //Act
            var data = await controller.GetBook(bookId);

            //Assert
            Assert.IsType<OkObjectResult>(data);
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var book = okResult.Value.Should().BeAssignableTo<Book>().Subject;

            Assert.Equal("TestSample2", book.Title);
            Assert.Equal("Description2", book.Description);
        }
        #endregion

        #region Get all books
        [Fact]
        public async Task Task_GetBooks_Return_OkResult()
        {
            //Arrange
            
            //Act
            var data = await controller.GetBooks();

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async Task Task_GetBooks_Return_MatchResult()
        {
            //Arrange
            
            //Act
            var data = await controller.GetBooks();

            //Assert
            Assert.IsType<OkObjectResult>(data);
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var book = okResult.Value.Should().BeAssignableTo<List<Book>>().Subject;

            Assert.Equal("TestSample1", book[0].Title);
            Assert.Equal("Description1", book[0].Description);

            Assert.Equal("TestSample2", book[1].Title);
            Assert.Equal("Description2", book[1].Description);
        }
        #endregion

        #region Add new book
        [Fact]
        public async Task Task_AddBook_ValidData_Return_OkResult()
        {
            //Arrange
            var book = new Book
            {
                Title = "NewSample1",
                Description = "NewDescription1",
                Author = "NewAuthor1",
                CoverImage = "/images/newsample1.jpg",
                Price = 10.00M
            };

            //Act
            var data = await controller.AddBook(book);

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async Task Task_AddBook_InvalidData_Return_BadRequestResult()
        {
            //Arrange
            var book = new Book
            {
                Title = "NewSample2",
                Description = "NewDescription2",
                Author = "NewAuthor2",
                CoverImage = "/images/newsample2.jpg",
                Price = -10.00M
            };

            //Act
            var data = await controller.AddBook(book);

            //Assert
            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public async Task Task_AddBook_ValidData_Return_MatchResult()
        {
            //Arrange
            var book = new Book
            {
                Title = "NewSample3",
                Description = "NewDescription3",
                Author = "NewAuthor3",
                CoverImage = "/images/newsample3.jpg",
                Price = 10.00M
            };

            //Act
            var data = await controller.AddBook(book);

            //Assert
            Assert.IsType<OkObjectResult>(data);
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;

            Assert.Equal(book, okResult.Value);
        }
        #endregion

        #region Edit book
        [Fact]
        public async Task Task_EditBook_ValidData_Return_OkResult()
        {
            //Arrange
            var book = new Book
            {
                Id = 1003,
                Title = "UpdateTestSample3",
                Description = "UpdateDescription3",
                Author = "UpdateAuthor3",
                CoverImage = "/images/updateimage3.jpg",
                Price = 10.00M
            };

            //Act         
            var updatedData = await controller.EditBook(book);

            //Assert
            Assert.IsType<OkObjectResult>(updatedData);
        }

        [Fact]
        public async Task Task_EditBook_InvalidData_Return_BadRequestResult()
        {
            //Arrange
            var book = new Book
            {
                Id = 1002,
                Title = "UpdateTestSample2",
                Description = "UpdateDescription2",
                Author = "UpdateAuthor2",
                CoverImage = "/images/updateimage2.jpg",
                Price = -10.00M
            };

            //Act
            var updatedData = await controller.EditBook(book);

            //Assert
            Assert.IsType<BadRequestObjectResult>(updatedData);
        }
        #endregion

        #region Delete book
        [Fact]
        public async Task Task_DeleteBook_Return_OkResult()
        {
            //Arrange
            var bookId = 1001;

            //Act         
            var updatedData = await controller.DeleteBook(bookId);

            //Assert
            Assert.IsType<OkObjectResult>(updatedData);
        }

        [Fact]
        public async Task Task_DeleteBook_Return_NotFoundResult()
        {
            //Arrange
            var bookId = 2002;

            //Act         
            var updatedData = await controller.DeleteBook(bookId);

            //Assert
            Assert.IsType<NotFoundObjectResult>(updatedData);
        }

        [Fact]
        public async Task Task_DeleteBook_Return_BadRequestResult()
        {
            //Arrange
            int? bookId = null;

            //Act         
            var updatedData = await controller.DeleteBook(bookId);

            //Assert
            Assert.IsType<BadRequestObjectResult>(updatedData);
        }
        #endregion

    }
}
