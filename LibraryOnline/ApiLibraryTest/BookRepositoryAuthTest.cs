using ApiLibrary.Repositories.Implementation;
using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibraryTest
{
    public class BookRepositoryAuthTest : IAsyncLifetime
    {
        private LibraryContext _context;
        private BookMethodAuth _repository;
        private AuthService _authService;

        public async Task InitializeAsync()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseSqlite("DataSource=:memory:")
                .LogTo(Console.WriteLine)  // Muestra todas las consultas SQL ejecutadas
                .Options;

            _context = new LibraryContext(options);
            await _context.Database.OpenConnectionAsync();
            await _context.Database.EnsureCreatedAsync();

            _authService = new AuthService();
            _repository = new BookMethodAuth(_context, _authService);
        }


        public async Task DisposeAsync()
        {
            await _context.Database.CloseConnectionAsync();
            _context.Dispose();
        }

        [Fact]
        public async Task AddBookAsync_ShouldAddBook_WhenAuthenticated()
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

            string username = "user1";
            string password = "password123";  // Usuario correcto

            // Act
            var result = await _repository.AddBookAsync(book, username, password);

            // Assert
            Assert.True(result);  // Debería ser verdadero porque la autenticación es exitosa
            var addedBook = await _context.Books.FindAsync(book.Id);
            Assert.NotNull(addedBook);
            Assert.Equal("Test Book", addedBook.Title);
        }

        [Fact]
        public async Task AddBookAsync_ShouldFail_WhenNotAuthenticated()
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

            string username = "user1";
            string password = "wrongpassword";  // Contraseña incorrecta

            // Act
            var result = await _repository.AddBookAsync(book, username, password);

            // Assert
            Assert.False(result);  // Debería fallar porque la autenticación no es exitosa
            var addedBook = await _context.Books.FindAsync(book.Id);
            Assert.Null(addedBook);  // El libro no debe haberse añadido
        }

        [Fact]
        public async Task DeleteBookAsync_ShouldDeleteBook_WhenAuthenticated()
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

            string username = "user1";
            string password = "password123";  // Usuario correcto

            await _repository.AddBookAsync(book, username, password);
            // Act
            var result = await _repository.DeleteBookAsync(book.Id, username, password);

            // Assert
            Assert.True(result);  // Debería ser verdadero porque la autenticación es exitosa
            var deletedBook = await _context.Books.FindAsync(book.Id);
            Assert.Null(deletedBook);  // El libro debe ser nulo porque fue eliminado
        }

        [Fact]
        public async Task DeleteBookAsync_ShouldFail_WhenNotAuthenticated()
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

            // Asegúrate de que el libro se agrega correctamente
            string username = "user1";  // Usuario correcto
            string password = "password123";  // Contraseña correcta

            var addResult = await _repository.AddBookAsync(book, username, password);
            Assert.True(addResult, "El libro debería haberse agregado correctamente.");

            string user = "user1";
            string pass = "wrongpassword";  // Contraseña incorrecta

            // Act
            var result = await _repository.DeleteBookAsync(book.Id, user, pass);

            // Assert
            Assert.False(result, "La eliminación debería fallar con credenciales incorrectas.");
            var deletedBook = await _context.Books.FindAsync(book.Id);
            Assert.NotNull(deletedBook);  // El libro no debe haberse eliminado
        }
    }
}
