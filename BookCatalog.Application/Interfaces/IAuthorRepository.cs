namespace BookCatalog.Application.Interfaces;

using BookCatalog.Domain;

public interface IAuthorRepository
{
    Task<IReadOnlyList<Author>> ListAsync(CancellationToken cancellationToken = default);
    Task<Author?> GetByIdAsync(int authorId, CancellationToken cancellationToken = default);
}
