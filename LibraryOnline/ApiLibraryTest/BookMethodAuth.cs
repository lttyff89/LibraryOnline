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
    public class BookMethodAuth
    {
        private readonly LibraryContext _context;
        private readonly AuthService _authService;

        public BookMethodAuth(LibraryContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<bool> AddBookAsync(Book book, string username, string password)
        {
            if (!_authService.Authenticate(username, password))
            {
                return false;  // User is not authenticated
            }

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBookAsync(int bookId, string username, string password)
        {
            // Verifica si el usuario está autenticado
            if (!_authService.Authenticate(username, password))
            {
                return false; // Si no está autenticado, no eliminamos el libro
            }

            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return false; // Si el libro no existe, no hacemos nada
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true; // El libro fue eliminado
        }


    }

}
