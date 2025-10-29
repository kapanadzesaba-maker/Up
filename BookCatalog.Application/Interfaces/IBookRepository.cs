namespace BookCatalog.Application.Interfaces;

using BookCatalog.Domain;

public interface IBookRepository
{
    Task<IReadOnlyList<Book>> ListAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Book>> ListByAuthorAsync(int authorId, CancellationToken cancellationToken = default);
    Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default);
    Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Book book, CancellationToken cancellationToken = default);
}
