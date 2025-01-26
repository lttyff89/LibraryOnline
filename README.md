# Library API

This is a simple REST API for managing a catalog of books. It provides functionality to add, retrieve, search, and delete books. It uses basic authentication to ensure that only authorized users can add or delete books.

## Features

- **Get all books**: Retrieves all the books in the catalog.
- **Get a specific book by ID**: Retrieves a specific book based on its ID.
- **Search for books**: Searches for books based on title, author, or genre.
- **Add a new book**: Allows authorized users to add a new book to the catalog.
- **Delete a book by ID**: Allows authorized users to delete a book from the catalog by its ID.

## Technologies Used

- ASP.NET Core
- SQLite (database)
- Entity Framework Core
- Swagger (for API documentation and testing)
- Basic Authentication

## Prerequisites

- [.NET 6.0 SDK or higher](https://dotnet.microsoft.com/download/dotnet)
- A text editor or IDE (e.g., Visual Studio, Visual Studio Code)

## Setup and Installation

1. **Clone the repository**:

    ```bash
    git clone https://github.com/yourusername/library-api.git
    cd library-api
    ```

2. **Install dependencies**:

    The project uses `EntityFramework` and `SQLite`. Make sure all dependencies are restored:

    ```bash
    dotnet restore
    ```

3. **Set up the database**:

    Make sure the database is set up and created automatically. You can use SQLite for local development. The database will be created at the default location specified in `appsettings.json`.

4. **Run the application**:

    To run the application, use:

    ```bash
    dotnet run
    ```

    The API will be accessible at `http://localhost:5000` by default.

5. **Access Swagger UI**:

    The API documentation and interactive testing interface are available via Swagger. Once the application is running, you can access Swagger UI at `http://localhost:5000/swagger`.

## Authentication

This API uses Basic Authentication. You can use the credentials:

- **Username**: `admin`
- **Password**: `password123`

Make sure to add the `Authorization` header with the `Basic` scheme to any request that requires authentication (e.g., adding or deleting books).

## API Endpoints

### 1. **Get all books**

- **Endpoint**: `GET /api/books`
- **Response**: Returns a list of all books in the catalog.

### 2. **Get a specific book by ID**

- **Endpoint**: `GET /api/books/{id}`
- **Response**: Returns a specific book based on its ID.

### 3. **Search for books**

- **Endpoint**: `GET /api/books/search?query={query}`
- **Parameters**: `query` (search term for title, author, or genre)
- **Response**: Returns a list of books that match the search term.

### 4. **Add a new book**

- **Endpoint**: `POST /api/books`
- **Authorization**: Requires Basic Authentication.
- **Body**: A JSON object containing the details of the book to be added.

  Example:
  ```json
  {
    "title": "The Great Gatsby",
    "author": "F. Scott Fitzgerald",
    "isbn": "9780743273565",
    "genre": "Fiction",
    "publishedDate": "1925-04-10"
  }

