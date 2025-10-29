namespace BookCatalog.Infrastructure.InMemory.Repositories;

using BookCatalog.Application.Interfaces;
using BookCatalog.Domain;
using BookCatalog.Infrastructure.InMemory.Seed;

public class AuthorRepositoryInMemory : IAuthorRepository
{
    public Task<Author?> GetByIdAsync(int authorId, CancellationToken cancellationToken = default)
    {
        InMemoryData.Authors.TryGetValue(authorId, out var author);
        return Task.FromResult(author);
    }

    public Task<IReadOnlyList<Author>> ListAsync(CancellationToken cancellationToken = default)
    {
        var list = InMemoryData.Authors.Values.ToList();
        return Task.FromResult((IReadOnlyList<Author>)list);
    }
}
