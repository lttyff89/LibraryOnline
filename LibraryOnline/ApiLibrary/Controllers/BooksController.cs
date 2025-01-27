using ApiLibrary.Repositories.Implementation;
using ApiLibrary.Repositories.Interface;
using LibraryApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace LibraryApi.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _repository;

        public BooksController(IBookRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all books from the catalog.
        /// </summary>
        /// <returns>A list of books</returns>
        // GET /api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await _repository.GetBooksAsync();
            return Ok(books);
        }

        /// <summary>
        /// Retrieves a specific book by its ID.
        /// </summary>
        /// <param name="id">ID of the book</param>
        /// <returns>A book with the requested information</returns>
        // GET /api/books/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _repository.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound(); // Return 404 if the book is not found
            }
            return Ok(book); // Return 200 with the book data
        }

        /// <summary>
        /// Searches for books by title, author, or genre.
        /// </summary>
        /// <param name="query">Search term</param>
        /// <returns>A list of books matching the search term</returns>
        // GET /api/books/search?query=somequery
        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks([FromQuery] string query)
        {
            var books = await _repository.SearchBooksAsync(query);
            if (books == null || !books.Any())
            {
                return NotFound(); // Return 404 if no books match the search query
            }
            return Ok(books); // Return 200 with the list of found books
        }

        /// <summary>
        /// Adds a new book to the catalog.
        /// </summary>
        /// <param name="book">Book to add</param>
        /// <returns>The newly added book</returns>
        // POST /api/books
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // If the model is invalid, return BadRequest with details
            }

            try
            {
                // Add the book to the database
                await _repository.AddBookAsync(book);

                // Return the created book with a 201 status and its URI
                return CreatedAtAction(nameof(AddBook), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                // In case of an error, return a 500 status code with the error message
                return StatusCode(500, $"Error creating the book: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a book by its ID.
        /// </summary>
        /// <param name="id">ID of the book to delete</param>
        /// <returns>Status code 204 if the deletion was successful</returns>
        // DELETE /api/books/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var book = await _repository.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound(); // Return 404 if the book is not found
            }

            await _repository.DeleteBookAsync(id);
            return NoContent(); // Return 204 if the deletion was successful
        }
    }
}
