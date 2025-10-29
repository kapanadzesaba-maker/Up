namespace BookCatalog.Infrastructure.InMemory.Repositories;

using BookCatalog.Application.Interfaces;
using BookCatalog.Domain;
using BookCatalog.Infrastructure.InMemory.Seed;

public class BookRepositoryInMemory : IBookRepository
{
    public Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        var nextId = InMemoryData.Books.Count == 0 ? 1 : InMemoryData.Books.Max(b => b.Id) + 1;
        book.Id = nextId;
        InMemoryData.Books.Add(book);
        return Task.FromResult(book);
    }

    public Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var book = InMemoryData.Books.FirstOrDefault(b => b.Id == id);
        return Task.FromResult(book);
    }

    public Task<IReadOnlyList<Book>> ListAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult((IReadOnlyList<Book>)InMemoryData.Books);
    }

    public Task<IReadOnlyList<Book>> ListByAuthorAsync(int authorId, CancellationToken cancellationToken = default)
    {
        var books = InMemoryData.Books.Where(b => b.AuthorId == authorId).ToList();
        return Task.FromResult((IReadOnlyList<Book>)books);
    }
}
