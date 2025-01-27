using ApiLibrary.Repositories.Interface;
using LibraryApi.Data;
using LibraryApi.Models;
using ApiLibrary.Repositories;
using ApiLibrary.Repositories.Implementation;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Endpoints
{
    public static class BooksEndpoints
    {
        public static void MapBooksEnspoints(this WebApplication app)
        {
            app.MapGet("/api/books", async (IBookRepository repository) =>
            {
                return Results.Ok(await repository.GetBooksAsync());
            });

            app.MapGet("/api/book/{id}", async (int id, IBookRepository repository) =>
            {
                var book = await repository.GetBookByIdAsync(id);
                return book is not null ? Results.Ok(book) : Results.NotFound();
            });

            app.MapGet("/api/books/search", async (string query, IBookRepository repository) =>
            {
                var book = await repository.SearchBooksAsync(query);
                return book is not null ? Results.Ok(book) : Results.NotFound();
            });

            app.MapPost("/api/books", async (Book book, IBookRepository repository) =>
            {
                await repository.AddBookAsync(book);
                return Results.Created($"/api/books/{book.Id}", book);
            });

            app.MapDelete("/api/books/{id}", async (int id, IBookRepository repository) =>
            {
                var book = await repository.GetBookByIdAsync(id);
                if (book is null) return Results.NotFound();

                await repository.DeleteBookAsync(id);
                return Results.NoContent();
            });


        }
    }
}
