using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WookieBooks.IServices;
using WookieBooks.Logger;
using WookieBooks.Models;
using WookieBooks.Utlilies;

namespace WookieBooks.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookServices _booksServices;
        private readonly ILoggerManager _logger;

        public BooksController(IBookServices booksServices, ILoggerManager logger)
        {
            _booksServices = booksServices;
            _logger = logger;
        }

        /// <summary>
        /// Get all the books
        /// </summary>
        /// <response code = "200">success - returns list of all available books</response>
        /// <response code = "500">internal server error - an error occurred at server side and books couldn't be fetched</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Book>), 200)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                _logger.LogInfo("Get  all Books");
                var books = await _booksServices.GetBooks();
                _logger.LogInfo("Get all Books - SUCCESS");
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogInfo("Get all Books - Error Occurred - Exception Info : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseModel { Message = "Error occurred while performing get action", ErrorDescription = ex.Message });
            }

        }

        /// <summary>
        /// Get book by id
        /// </summary>
        /// <param name="id"></param>
        /// <response code = "200">success - returns book filtered by requested id</response>
        /// <response code = "400">bad request - error in the payload sent in request</response>
        /// <response code = "404">not found - requested book not found</response>
        /// <response code = "500">internal server error - an error occurred at server side and book couldn't be fetched</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Book), 200)]
        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 404)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        public async Task<IActionResult> GetBook(int? id)
        {
            try
            {
                _logger.LogInfo("Get Book by Id : " + id);
                if (id == null) 
                {
                    _logger.LogInfo("Get Book by Id : - BAD REQUEST(Id cannot be null)");
                    return BadRequest(new ErrorResponseModel { Message = "Please recheck the request. Id cannot be null" });
                }
                var book = await _booksServices.GetBookById(id);
                if (book == null)
                {
                    _logger.LogInfo("Get Book by Id : "+ id + "- NOT FOUND");
                    return NotFound(new ErrorResponseModel { Message = "Could not find the requested record" });
                }
                else
                {
                    _logger.LogInfo("Get Book by Id : " + id + "- SUCCESS");
                    return Ok(book);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInfo("Get Book by Id : "+ id +" - Error Occurred - Exception Info : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseModel { Message = "Error occurred while performing get action", ErrorDescription = ex.Message });
            }

        }

        /// <summary>
        /// Add a new book to the collection
        /// </summary>
        /// <param name="book"></param> json payload containing details of the book to be added
        /// <response code = "200">success - new book added to collection</response>
        /// <response code = "400">bad request - error in the payload sent in request</response>
        /// <response code = "500">internal server error - an error occurred at server side and book couldn't be added</response>
        [HttpPost]
        [ProducesResponseType(typeof(Book), 200)]
        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        public async Task<IActionResult> AddBook(Book book)
        {
            try
            {
                _logger.LogInfo("Add new Book");
                if (InputValidation.validateRequest(book))
                {
                    var result = await _booksServices.AddBook(book);
                    _logger.LogInfo("Add new Book - SUCCESS");
                    return Ok(result);                    
                }
                else
                {
                    _logger.LogInfo("Add new Book - BAD REQUEST");
                    return BadRequest(new ErrorResponseModel { Message = "Please recheck the request" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogInfo("Add new Book - Error Occurred - Exception Info : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseModel { Message = "Error occurred while performing add action", ErrorDescription = ex.Message });
            }
        }

        /// <summary>
        /// Edit the details of an existing book in the collection
        /// </summary>
        /// <param name="book"></param> json payload containing details of the book to be edited
        /// <response code = "200">success - book edited</response>
        /// <response code = "400">bad request - error in the payload sent in request</response>
        /// <response code = "404">not found - requested book couldn't be found</response>
        /// <response code = "500">internal server error - an error occurred at server side and book couldn't be edited</response>
        [HttpPut]
        [ProducesResponseType(typeof(Book), 200)]
        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 404)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        public async Task<IActionResult> EditBook(Book book)
        {
            try
            {
                _logger.LogInfo("Edit Book");
                if (InputValidation.validateRequest(book))
                {
                    var result = await _booksServices.EditBook(book);
                    if (result == null)
                    {
                        _logger.LogInfo("Edit Book - request record NOT FOUND");
                        return NotFound(new ErrorResponseModel { Message = "Could not find the requested record" });
                    }
                    else
                    {
                        _logger.LogInfo("Edit Book - SUCCESS");
                        return Ok(result);
                    }
                }
                else
                {
                    _logger.LogInfo("Edit Book - BAD REQUEST");
                    return BadRequest(new ErrorResponseModel { Message = "Please recheck the request" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogInfo("Edit Book - Error Occurred - Exception Info : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseModel { Message = "Error occurred while performing edit action", ErrorDescription = ex.Message });
            }
        }

        /// <summary>
        /// Delete a book record from the collection
        /// </summary>
        /// <param name="id"></param>
        /// <response code = "200">success - book edited</response>
        /// <response code = "400">bad request - error in the payload sent in request</response>
        /// <response code = "404">not found - requested book couldn't be found</response>
        /// <response code = "500">internal server error - an error occurred at server side and book couldn't be edited</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Book),200)]
        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel),404)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        public async Task<IActionResult> DeleteBook(int? id)
        {
            try
            {
                _logger.LogInfo("Delete Book");
                if (id == null)
                {
                    _logger.LogInfo("Delete Book - BAD REQUEST");
                    return BadRequest(new ErrorResponseModel { Message = "Please recheck the request. Id cannot be null" });
                }
                var result = await _booksServices.DeleteBook(id);
                if (result == null)
                {
                    _logger.LogInfo("Delete Book - requested record NOT FOUND");
                    return NotFound(new ErrorResponseModel { Message = "Could not find the requested record" });
                }
                else
                {
                    _logger.LogInfo("Delete Book -SUCCESS");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInfo("Edit Book - Error Occurred - Exception Info : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseModel { Message = "Error occurred while performing delete action", ErrorDescription = ex.Message });
            }            
        }
    }
}
