using LibraryApp.Models;
using LibraryApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;


namespace LibraryApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        // Acción para ver todos los libros
        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetBooksAsync();

            if (books == null || !books.Any())
            {
                return View("Error");  // O cualquier otra vista si no hay libros
            }

            return View(books);
        }

        // Acción para agregar un nuevo libro
        public IActionResult Create()
        {
            return PartialView("_CreateBookPartial", new Book()); // Retorna un PartialView con un modelo vacío
        }

        [HttpPost]
        //public async Task<IActionResult> Create(Book book)
        //{
        //    if (ModelState.IsValid)
        //    {
        //       var reponse = await _bookService.AddBookAsync(book);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(book);
        //}

        public async Task<IActionResult> Create(Book book)
        {

            var username = Request.Form["Username"];
            var password = Request.Form["Password"];

            if (ModelState.IsValid)
            {               
       
                // Llamada a tu servicio para agregar el libro
                var response = await _bookService.AddBookAsync(book, username, password);

                return response;
            }

            // Si el modelo no es válido, devolver los errores de validación
            return Json(new { success = false, message = "The book data is invalid." });
        }


        // Acción para eliminar un libro
        public async Task<IActionResult> Delete(int id)
        {
            var username = Request.Form["Username"];
            var password = Request.Form["Password"];

           var response =  await _bookService.DeleteBookAsync(id, username, password);
            return response;
        }
    }
}
