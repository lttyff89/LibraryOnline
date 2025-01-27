using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LibraryApi.Data;
using ApiLibrary.Repositories.Implementation;
using ApiLibrary.Repositories.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using LibraryApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure the connection string for the database
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories and services
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddControllers();

// Configure Basic Authentication
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentication", options => { });

builder.Services.AddEndpointsApiExplorer();

// Add Swagger for API documentation
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Library API",
        Version = "v1",
        Description = "API for managing a book catalog",
        Contact = new OpenApiContact
        {
            Name = "Arletty Fernandez Fustes",
            Email = "lttyff@gmail.com"
        }
    });

    // Include XML comments if available
    var xmlFile = $"{System.AppDomain.CurrentDomain.FriendlyName}.xml";
    var xmlPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    // Set up Basic Authentication for Swagger
    options.AddSecurityDefinition("BasicAuthentication", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        Description = "Use the credentials 'admin' and 'password123' to test Basic Authentication"
    });

    // Configure authentication in Swagger documentation
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "BasicAuthentication"
                }
            },
            new string[] {}
        }
    });
});

// Register the book repository service
builder.Services.AddScoped<IBookRepository, BookRepository>();

var app = builder.Build();

// Automatically create the database if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    context.Database.EnsureCreated();  // Automatically create the database if it doesn't exist
}

// Enable Swagger in the request pipeline for development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();  // Set up the Swagger UI
}

// Enable Swagger for other environments (could be removed if you don't need it outside development)
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
