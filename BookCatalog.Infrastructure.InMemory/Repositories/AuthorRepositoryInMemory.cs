namespace BookCatalog.Infrastructure.InMemory.Repositories;

using BookCatalog.Application.Interfaces;
using BookCatalog.Domain;
using BookCatalog.Infrastructure.InMemory.Seed;

public class AuthorRepositoryInMemory : IAuthorRepository
{
    public Task<Author?> GetByIdAsync(int authorId, CancellationToken cancellationToken = default)
    {
        var author = InMemoryData.Authors.FirstOrDefault(a => a.Id == authorId);
        return Task.FromResult(author);
    }

    public Task<IReadOnlyList<Author>> ListAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult((IReadOnlyList<Author>)InMemoryData.Authors);
    }
}
