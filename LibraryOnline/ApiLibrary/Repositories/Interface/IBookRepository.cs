using LibraryApi.Models;

namespace ApiLibrary.Repositories.Interface
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetBooksAsync();

        Task<Book?> GetBookByIdAsync(int id);
        Task<IEnumerable<Book>> SearchBooksAsync(string query);
        Task AddBookAsync(Book book);
        Task DeleteBookAsync(int id);
    }
}
