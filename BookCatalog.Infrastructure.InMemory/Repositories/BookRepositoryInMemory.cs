namespace BookCatalog.Infrastructure.InMemory.Repositories;

using BookCatalog.Application.Interfaces;
using BookCatalog.Domain;
using BookCatalog.Infrastructure.InMemory.Seed;

public class BookRepositoryInMemory : IBookRepository
{
    public Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        var nextId = InMemoryData.Books.Keys.DefaultIfEmpty(0).Max() + 1;
        book.Id = nextId;
        InMemoryData.Books[book.Id] = book;
        return Task.FromResult(book);
    }

    public Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        InMemoryData.Books.TryGetValue(id, out var book);
        return Task.FromResult(book);
    }

    public Task<IReadOnlyList<Book>> ListAsync(CancellationToken cancellationToken = default)
    {
        var list = InMemoryData.Books.Values.ToList();
        return Task.FromResult((IReadOnlyList<Book>)list);
    }

    public Task<IReadOnlyList<Book>> ListByAuthorAsync(int authorId, CancellationToken cancellationToken = default)
    {
        var list = InMemoryData.Books.Values.Where(b => b.AuthorId == authorId).ToList();
        return Task.FromResult((IReadOnlyList<Book>)list);
    }

    public Task UpdateAsync(Book book, CancellationToken cancellationToken = default)
    {
        InMemoryData.Books[book.Id] = book;
        return Task.CompletedTask;
    }
}
