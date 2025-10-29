namespace BookCatalog.Api.Endpoints;

using BookCatalog.Application.Contracts;
using BookCatalog.Application.Interfaces;

public static class BooksEndpoints
{
    public static IEndpointRouteBuilder MapBooksEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/books");

        group.MapGet("/", async (IBookService svc, int? publicationYear, string? sortBy, int? page, int? pageSize, CancellationToken ct) =>
        {
            var items = await svc.GetAllAsync(publicationYear, sortBy, page, pageSize, ct);
            return Results.Ok(items);
        });

        group.MapPost("/", async (IBookService svc, CreateBookRequest request, CancellationToken ct) =>
        {
            try
            {
                var created = await svc.CreateAsync(request, ct);
                return Results.Created($"/api/books/{created.Id}", created);
            }
            catch (ArgumentException ex)
            {
                return Results.Problem(title: "Validation failed", detail: ex.Message, statusCode: StatusCodes.Status400BadRequest);
            }
            catch (KeyNotFoundException ex)
            {
                return Results.Problem(title: "Author not found", detail: ex.Message, statusCode: StatusCodes.Status400BadRequest);
            }
        });

        group.MapPut("/{id:int}", async (int id, IBookService svc, UpdateBookRequest request, CancellationToken ct) =>
        {
            try
            {
                await svc.UpdateAsync(id, request, ct);
                return Results.NoContent();
            }
            catch (ArgumentException ex)
            {
                return Results.Problem(title: "Validation failed", detail: ex.Message, statusCode: StatusCodes.Status400BadRequest);
            }
            catch (KeyNotFoundException ex)
            {
                var isBook = ex.Message.StartsWith("Book ");
                return Results.Problem(title: isBook ? "Book not found" : "Author not found", detail: ex.Message, statusCode: isBook ? StatusCodes.Status404NotFound : StatusCodes.Status400BadRequest);
            }
        });

        return app;
    }
}
