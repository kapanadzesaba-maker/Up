namespace BookCatalog.Application.Interfaces;

using BookCatalog.Application.Contracts;

public interface IBookService
{
    Task<IReadOnlyList<BookDto>> GetAllAsync(int? publicationYear = null, string? sortBy = null, int? page = null, int? pageSize = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BookDto>> GetByAuthorAsync(int authorId, CancellationToken cancellationToken = default);
    Task<BookDto> CreateAsync(CreateBookRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, UpdateBookRequest request, CancellationToken cancellationToken = default);
}
