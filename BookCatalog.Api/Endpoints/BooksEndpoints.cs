namespace BookCatalog.Api.Endpoints;

using BookCatalog.Application.Contracts;
using BookCatalog.Application.Interfaces;

public static class BooksEndpoints
{
    public static IEndpointRouteBuilder MapBooksEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/books");

        group.MapGet("/", async (IBookService svc, CancellationToken ct) =>
        {
            var items = await svc.GetAllAsync(ct);
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

        return app;
    }
}
