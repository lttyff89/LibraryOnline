using LibraryApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Services
{
    public class BookService
    {
        private readonly HttpClient _httpClient;

        // Inyectamos el HttpClient para hacer solicitudes HTTP a la API.
        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Obtener todos los libros
        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            var response = await _httpClient.GetAsync("/api/books");
            response.EnsureSuccessStatusCode();  // Lanza excepción si el código de respuesta no es exitoso


            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error al obtener los libros. Código de estado: {response.StatusCode}");
            }

            // Leer el contenido de la respuesta como texto para depuración
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Contenido de la respuesta: " + responseContent);

            // Intentar deserializar la respuesta como JSON
            var books = await response.Content.ReadFromJsonAsync<IEnumerable<Book>>();

            if (books == null)
            {
                throw new Exception("La respuesta de la API es nula o no se pudo deserializar.");
            }

            return books;

        }

        // Agregar un libro
        //public async Task AddBookAsync(Book book)
        //{
        //    var response = await _httpClient.PostAsJsonAsync("/api/books", book);
        //    response.EnsureSuccessStatusCode();
        //}

        public async Task<ActionResult> AddBookAsync(Book book, string username, string password)
        {
            try
            {
                // Crear el encabezado de autenticación básica
                var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                var authHeader = Convert.ToBase64String(byteArray);

                // Configurar los encabezados para la solicitud HTTP
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);

                // Enviar el libro a la API
                var response = await _httpClient.PostAsJsonAsync("/api/books", book);

                // Si la respuesta es exitosa, retornar el código de respuesta adecuado
                response.EnsureSuccessStatusCode(); // Esto lanzará una excepción si el código de estado no es exitoso

                // Si todo salió bien, devolver un 200 OK con el libro creado y un mensaje
                var successMessage = "El libro se ha agregado correctamente.";

                // Devolver el libro con un mensaje adicional usando JsonResult
                return new JsonResult(new { message = successMessage, book = book })
                {
                    StatusCode = 200 // Esto asegura que el código de estado HTTP es 200 (OK)
                };
            }
            catch (HttpRequestException ex)
            {
                // Captura errores relacionados con la solicitud HTTP
                if (ex.Message.Contains("401"))
                {
                    return new JsonResult(new { success = false, message = "No autorizado. Verifica tus credenciales." })
                    {
                        StatusCode = 401 // Retorna un 401 para 'Unauthorized'
                    };
                }
                else
                {
                    // Si es otro tipo de error HTTP
                    return new JsonResult(new { success = false, message = $"Error al agregar el libro: {ex.Message}" })
                    {
                        StatusCode = 400 // Bad Request
                    };
                }
            }
            catch (Exception ex)
            {
                // Captura errores generales y devuelve un mensaje de error inesperado
                return new JsonResult(new { success = false, message = "Ocurrió un error inesperado." })
                {
                    StatusCode = 500 // Internal Server Error
                };
            }
        }





        // Eliminar un libro
        public async Task<ActionResult> DeleteBookAsync(int id, string username, string password)
        {

            try
            {
                // Crear el encabezado de autenticación básica
                var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                var authHeader = Convert.ToBase64String(byteArray);

                // Configurar los encabezados para la solicitud HTTP
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);

                var response = await _httpClient.DeleteAsync($"/api/books/{id}");
                response.EnsureSuccessStatusCode();

                // Si todo salió bien, devolver un 200 OK con el libro creado y un mensaje
                var successMessage = "El libro se ha eliminado correctamente.";

                // Devolver el libro con un mensaje adicional usando JsonResult
                return new JsonResult(new { message = successMessage})
                {
                    StatusCode = 200 // Esto asegura que el código de estado HTTP es 200 (OK)
                };
            }
            catch (HttpRequestException ex)
            {
                // Captura errores relacionados con la solicitud HTTP
                if (ex.Message.Contains("401"))
                {
                    return new JsonResult(new { success = false, message = "No autorizado. Verifica tus credenciales." })
                    {
                        StatusCode = 401 // Retorna un 401 para 'Unauthorized'
                    };
                }
                else
                {
                    // Si es otro tipo de error HTTP
                    return new JsonResult(new { success = false, message = $"Error al eliminar el libro: {ex.Message}" })
                    {
                        StatusCode = 400 // Bad Request
                    };
                }
            }
            catch (Exception ex)
            {
                // Captura errores generales y devuelve un mensaje de error inesperado
                return new JsonResult(new { success = false, message = "Ocurrió un error inesperado." })
                {
                    StatusCode = 500 // Internal Server Error
                };
            }

      
        }
    }
}
