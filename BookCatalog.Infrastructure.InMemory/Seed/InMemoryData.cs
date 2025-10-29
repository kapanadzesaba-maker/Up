namespace BookCatalog.Infrastructure.InMemory.Seed;

using BookCatalog.Domain;
using System.Collections.Concurrent;

public static class InMemoryData
{
    public static readonly ConcurrentDictionary<int, Author> Authors = new(new[]
    {
        new KeyValuePair<int, Author>(1, new Author { Id = 1, Name = "Robert C. Martin" }),
        new KeyValuePair<int, Author>(2, new Author { Id = 2, Name = "Jeffrey Richter" })
    });

    public static readonly ConcurrentDictionary<int, Book> Books = new(new[]
    {
        new KeyValuePair<int, Book>(1, new Book { Id = 1, Title = "Clean Code", AuthorId = 1, PublicationYear = 2008 }),
        new KeyValuePair<int, Book>(2, new Book { Id = 2, Title = "CLR via C#", AuthorId = 2, PublicationYear = 2012 }),
        new KeyValuePair<int, Book>(3, new Book { Id = 3, Title = "The Clean Coder", AuthorId = 1, PublicationYear = 2011 })
    });
}
