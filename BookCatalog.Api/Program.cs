var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();

// DI wiring
builder.Services.AddSingleton<BookCatalog.Application.Interfaces.IAuthorRepository, BookCatalog.Infrastructure.InMemory.Repositories.AuthorRepositoryInMemory>();
builder.Services.AddSingleton<BookCatalog.Application.Interfaces.IBookRepository, BookCatalog.Infrastructure.InMemory.Repositories.BookRepositoryInMemory>();
builder.Services.AddSingleton<BookCatalog.Application.Interfaces.IBookMapper, BookCatalog.Application.Mapping.BookMapper>();
builder.Services.AddSingleton<BookCatalog.Application.Interfaces.IBookService, BookCatalog.Application.Services.BookService>();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var feature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        var ex = feature?.Error;
        if (ex is ArgumentException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ProblemDetails { Title = "Validation failed", Detail = ex.Message, Status = StatusCodes.Status400BadRequest });
            return;
        }
        if (ex is KeyNotFoundException ke)
        {
            var isBook = ke.Message.StartsWith("Book ");
            context.Response.StatusCode = isBook ? StatusCodes.Status404NotFound : StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ProblemDetails { Title = isBook ? "Book not found" : "Author not found", Detail = ke.Message, Status = context.Response.StatusCode });
            return;
        }
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new ProblemDetails { Title = "Unexpected error" , Detail = ex?.Message, Status = StatusCodes.Status500InternalServerError });
    });
});

app.MapGet("/", () => Results.Ok(new { name = "BookCatalog API", status = "ok" }));

// API v1 route group
var v1 = app.MapGroup("/api/v1");
BookCatalog.Api.Endpoints.BooksEndpoints.MapBooksEndpoints(v1);
BookCatalog.Api.Endpoints.AuthorsEndpoints.MapAuthorsEndpoints(v1);

app.Run();


