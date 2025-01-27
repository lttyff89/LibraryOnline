using ApiLibrary.Repositories.Implementation;
using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LibraryAPI.Tests
{
    public class BookRepositoryTests : IAsyncLifetime
    {
        private LibraryContext _context;
        private BookRepository _repository;

        // Set up in-memory SQLite database for testing
        public async Task InitializeAsync()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseSqlite("DataSource=:memory:")  // Use an in-memory SQLite database
                .Options;

            _context = new LibraryContext(options);
            await _context.Database.OpenConnectionAsync();
            await _context.Database.EnsureCreatedAsync();  // Create the database schema

            _repository = new BookRepository(_context);
        }

        // Cleanup the database after tests
        public async Task DisposeAsync()
        {
            await _context.Database.CloseConnectionAsync();
            _context.Dispose();
        }

        [Fact]
        public async Task AddBookAsync_ShouldAddBook()
        {
            // Arrange
            var book = new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "123456789",
                PunlishedDate = DateTime.Now.ToString(),
                Genre = "Fiction"
            };

            // Act
            await _repository.AddBookAsync(book);

            // Assert
            var addedBook = await _context.Books.FindAsync(book.Id);
            Assert.NotNull(addedBook);
            Assert.Equal("Test Book", addedBook.Title);
            Assert.Equal("Test Author", addedBook.Author);
        }

        [Fact]
        public async Task GetBooksAsync_ShouldReturnAllBooks()
        {
            // Arrange
            var book1 = new Book
            {
                Title = "Book One",
                Author = "Author One",
                ISBN = "111111",
                PunlishedDate = DateTime.Now.ToString(),
                Genre = "Sci-Fi"
            };

            var book2 = new Book
            {
                Title = "Book Two",
                Author = "Author Two",
                ISBN = "222222",
                PunlishedDate = DateTime.Now.ToString(),
                Genre = "Fantasy"
            };

            await _repository.AddBookAsync(book1);
            await _repository.AddBookAsync(book2);

            // Act
            var books = await _repository.GetBooksAsync();

            // Assert
            Assert.Equal(2, books.Count());
        }

        [Fact]
        public async Task SearchBooksAsync_ShouldReturnCorrectBooks()
        {
            // Arrange
            var book1 = new Book
            {
                Title = "The Great Adventure",
                Author = "Adventure Author",
                ISBN = "111111",
                PunlishedDate = DateTime.Now.ToString(),
                Genre = "Adventure"
            };

            var book2 = new Book
            {
                Title = "Mystery Night",
                Author = "Mystery Author",
                ISBN = "222222",
                PunlishedDate = DateTime.Now.ToString(),
                Genre = "Mystery"
            };

            await _repository.AddBookAsync(book1);
            await _repository.AddBookAsync(book2);

            // Act
            var searchResult = await _repository.SearchBooksAsync("Mystery");

            // Assert
            Assert.Single(searchResult);
            Assert.Equal("Mystery Night", searchResult.First().Title);
        }

        [Fact]
        public async Task DeleteBookAsync_ShouldDeleteBook()
        {
            // Arrange
            var book = new Book
            {
                Title = "Book to Delete",
                Author = "Delete Author",
                ISBN = "999999",
                PunlishedDate = DateTime.Now.ToString(),
                Genre = "Thriller"
            };

            await _repository.AddBookAsync(book);

            // Act
            await _repository.DeleteBookAsync(book.Id);

            // Assert
            var deletedBook = await _context.Books.FindAsync(book.Id);
            Assert.Null(deletedBook);  // Should be null because it was deleted
        }
    }
}
