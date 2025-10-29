namespace BookCatalog.Api.Endpoints;

using BookCatalog.Application.Interfaces;

public static class AuthorsEndpoints
{
    public static IEndpointRouteBuilder MapAuthorsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/authors");

        group.MapGet("/{id:int}/books", async (int id, IBookService svc, CancellationToken ct) =>
        {
            try
            {
                var items = await svc.GetByAuthorAsync(id, ct);
                return Results.Ok(items);
            }
            catch (KeyNotFoundException ex)
            {
                return Results.Problem(title: "Author not found", detail: ex.Message, statusCode: StatusCodes.Status404NotFound);
            }
        });

        return app;
    }
}
