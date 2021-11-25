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
                _logger.LogInfo("START : GetBooks() - Get list of all books");

                //Call GetBooks() method in service layer
                var books = await _booksServices.GetBooks();

                _logger.LogInfo("GetBooks() - SUCCESS");
                //return the fetched list of books
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogInfo("GetBooks() - Error Occurred - Exception Info : " + ex.Message);
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
                _logger.LogInfo("START - GetBook(id) - GetBook by Id : " + id);

                //If id in the request is null, return BadRequest
                if (id == null) 
                {
                    _logger.LogInfo("GetBook(id) : - BAD REQUEST(Id cannot be null)");
                    return BadRequest(new ErrorResponseModel { Message = "Please recheck the request. Id cannot be null" });
                }

                //Call GetBookById() method in service layer
                var book = await _booksServices.GetBookById(id);

                //If no records fetched, return NotFound
                if (book == null)
                {
                    _logger.LogInfo("GetBook("+ id + ") - NOT FOUND");
                    return NotFound(new ErrorResponseModel { Message = "Could not find the requested record" });
                }
                else
                {
                    _logger.LogInfo("GetBook(" + id + ") - SUCCESS");
                    //return the fetched record
                    return Ok(book);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInfo("GetBook(" + id + ") - Error Occurred - Exception Info : " + ex.Message);
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
                _logger.LogInfo("START - AddBook() - Add new Book");

                //Validate the request
                if (InputValidation.validateRequest(book))
                {
                    //Call AddBook() method in service layer
                    var result = await _booksServices.AddBook(book);

                    _logger.LogInfo("AddBook() - SUCCESS");
                    //return the added book record
                    return Ok(result);                    
                }
                else
                {
                    //If validation fails, return BadRequest
                    _logger.LogInfo("AddBook() - BAD REQUEST");
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
                _logger.LogInfo("START - EditBook() - Edit existing Book");

                //Validate the request
                if (InputValidation.validateRequest(book))
                {
                    //call EditBook() method in service layer
                    var result = await _booksServices.EditBook(book);

                    //If result returned from service layer is null, unable to find the requested record
                    if (result == null)
                    {
                        _logger.LogInfo("EditBook() - request record NOT FOUND");
                        return NotFound(new ErrorResponseModel { Message = "Could not find the requested record" });
                    }
                    else
                    {
                        _logger.LogInfo("EditBook()- SUCCESS");
                        //return the updated record
                        return Ok(result);
                    }
                }
                else
                {
                    //if validation fails, return BadRequest
                    _logger.LogInfo("EditBook() - BAD REQUEST");
                    return BadRequest(new ErrorResponseModel { Message = "Please recheck the request" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogInfo("EditBook() - Error Occurred - Exception Info : " + ex.Message);
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
                _logger.LogInfo("START - DeleteBook() - Delete Book");

                //if id in request is null, return BadRequest
                if (id == null)
                {
                    _logger.LogInfo("DeleteBook() - BAD REQUEST");
                    return BadRequest(new ErrorResponseModel { Message = "Please recheck the request. Id cannot be null" });
                }

                //call DeleteBook() method in service layer
                var result = await _booksServices.DeleteBook(id);

                //if result returned from service layer is null, requested record is not found
                if (result == null)
                {
                    _logger.LogInfo("DeleteBook() - requested record NOT FOUND");
                    return NotFound(new ErrorResponseModel { Message = "Could not find the requested record" });
                }
                else
                {
                    _logger.LogInfo("DeleteBook() -SUCCESS");
                    //return the deleted record
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInfo("DeleteBook() - Error Occurred - Exception Info : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseModel { Message = "Error occurred while performing delete action", ErrorDescription = ex.Message });
            }            
        }
    }
}
