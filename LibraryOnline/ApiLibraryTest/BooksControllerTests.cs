// Tests/BooksControllerTests.cs
using ApiLibrary.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using LibraryApi.Controllers;
using LibraryApi.Models;

namespace LibraryAPI.Tests
{
    public class BooksControllerTests
    {
        private readonly Mock<IBookRepository> _mockRepository;
        private readonly BooksController _controller;

        public BooksControllerTests()
        {
            // Crear un mock del repositorio
            _mockRepository = new Mock<IBookRepository>();

            // Crear el controlador e inyectar el mock
            _controller = new BooksController(_mockRepository.Object);
        }
        #region Test para GetBooks
        [Fact]
        public async Task GetBooks_ShouldReturnBooks()
        {
            // Arrange: Crear una lista simulada de libros
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1", Author = "Author 1", ISBN = "123", PunlishedDate =  DateTime.Now.ToString(), Genre = "Fiction" },
                new Book { Id = 2, Title = "Book 2", Author = "Author 2", ISBN = "456", PunlishedDate =  DateTime.Now.ToString(), Genre = "Science" }
            };

            // Configurar el mock para devolver la lista de libros cuando se llame a GetAllBooksAsync
            _mockRepository.Setup(repo => repo.GetBooksAsync()).ReturnsAsync(books);

            // Act: Llamar al método GetBooks en el controlador
            var result = await _controller.GetBooks();

            // Assert: Verificar que el resultado sea un ActionResult
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Book>>>(result);

            // Verificar que el resultado es un OkObjectResult
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            // Assert: Asegurarse de que el contenido dentro de OkObjectResult sea de tipo IEnumerable<Book>
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);

            // Verificar la cantidad de libros
            Assert.Equal(2, returnValue.Count());
        }
        #endregion

        #region Test para GetBookById

        [Fact]
        public async Task GetBookById_ShouldReturnBook_WhenBookExists()
        {
            // Arrange: Crear un libro simulado
            var book = new Book { Id = 1, Title = "Book 1", Author = "Author 1", ISBN = "123", PunlishedDate = DateTime.Now.ToString(), Genre = "Fiction" };

            // Configurar el mock para devolver el libro cuando se busque por ID
            _mockRepository.Setup(repo => repo.GetBookByIdAsync(1)).ReturnsAsync(book);

            // Act: Llamar al método GetBookById en el controlador
            var result = await _controller.GetBook(1);

            // Assert: Verificar que el resultado sea un OkObjectResult
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Book>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task GetBookById_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange: No existe un libro con ID 999
            _mockRepository.Setup(repo => repo.GetBookByIdAsync(999)).ReturnsAsync((Book)null);

            // Act: Intentar obtener un libro con un ID inexistente
            var result = await _controller.GetBook(999);

            // Assert: Verificar que el resultado sea un NotFoundResult
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        #endregion

        #region Test para SearchBooks

        [Fact]
        public async Task SearchBooks_ShouldReturnBooks_WhenBooksFound()
        {
            // Arrange: Crear una lista de libros simulada
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1", Author = "Author 1", ISBN = "123", PunlishedDate =  DateTime.Now.ToString(), Genre = "Fiction" },
                new Book { Id = 2, Title = "Book 2", Author = "Author 2", ISBN = "456", PunlishedDate = DateTime.Now.ToString(), Genre = "Science" }
            };

            // Configurar el mock para devolver los libros cuando se busque por título o autor
            _mockRepository.Setup(repo => repo.SearchBooksAsync("Book")).ReturnsAsync(books);

            // Act: Llamar al método SearchBooks en el controlador con el query "Book"
            var result = await _controller.SearchBooks("Book");

            // Assert: Verificar que el resultado sea un OkObjectResult
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task SearchBooks_ShouldReturnNotFound_WhenNoBooksFound()
        {
            // Arrange: No existe un libro que coincida con el query "NonExistent"
            _mockRepository.Setup(repo => repo.SearchBooksAsync("NonExistent")).ReturnsAsync(new List<Book>());

            // Act: Intentar buscar libros con un query que no existe
            var result = await _controller.SearchBooks("NonExistent");

            // Assert: Verificar que el resultado sea un NotFoundResult
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        #endregion

        [Fact]
        public async Task DeleteBook_ShouldReturnNoContent_WhenBookExists()
        {
            // Arrange: Crear un libro simulado
            var book = new Book { Id = 1, Title = "Book 1", Author = "Author 1", ISBN = "123", PunlishedDate = DateTime.Now.ToString(), Genre = "Fiction" };

            // Configurar el mock para devolver el libro cuando se busque por ID
            _mockRepository.Setup(repo => repo.GetBookByIdAsync(1)).ReturnsAsync(book);
            _mockRepository.Setup(repo => repo.DeleteBookAsync(1)).Returns(Task.CompletedTask);

            // Act: Llamar al método DeleteBook en el controlador
            var result = await _controller.DeleteBook(1);

            // Assert: Verificar que el resultado sea un NoContent (HTTP 204)
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task DeleteBook_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange: No existe un libro con ID 999
            _mockRepository.Setup(repo => repo.GetBookByIdAsync(999)).ReturnsAsync((Book)null);

            // Act: Intentar eliminar un libro con un ID inexistente
            var result = await _controller.DeleteBook(999);

            // Assert: Verificar que el resultado sea un NotFound (HTTP 404)
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}

