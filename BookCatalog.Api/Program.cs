var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();

// DI wiring
builder.Services.AddSingleton<BookCatalog.Application.Interfaces.IAuthorRepository, BookCatalog.Infrastructure.InMemory.Repositories.AuthorRepositoryInMemory>();
builder.Services.AddSingleton<BookCatalog.Application.Interfaces.IBookRepository, BookCatalog.Infrastructure.InMemory.Repositories.BookRepositoryInMemory>();
builder.Services.AddSingleton<BookCatalog.Application.Interfaces.IBookMapper, BookCatalog.Application.Mapping.BookMapper>();
builder.Services.AddSingleton<BookCatalog.Application.Interfaces.IBookService, BookCatalog.Application.Services.BookService>();

var app = builder.Build();

app.UseExceptionHandler();

app.MapGet("/", () => Results.Ok(new { name = "BookCatalog API", status = "ok" }));

// Endpoint groups
BookCatalog.Api.Endpoints.BooksEndpoints.MapBooksEndpoints(app);
BookCatalog.Api.Endpoints.AuthorsEndpoints.MapAuthorsEndpoints(app);

app.Run();


