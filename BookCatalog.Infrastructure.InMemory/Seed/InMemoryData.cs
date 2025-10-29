namespace BookCatalog.Infrastructure.InMemory.Seed;

using BookCatalog.Domain;

public static class InMemoryData
{
    public static readonly List<Author> Authors = new()
    {
        new Author { Id = 1, Name = "Robert C. Martin" },
        new Author { Id = 2, Name = "Jeffrey Richter" }
    };

    public static readonly List<Book> Books = new()
    {
        new Book { Id = 1, Title = "Clean Code", AuthorId = 1, PublicationYear = 2008 },
        new Book { Id = 2, Title = "CLR via C#", AuthorId = 2, PublicationYear = 2012 },
        new Book { Id = 3, Title = "The Clean Coder", AuthorId = 1, PublicationYear = 2011 }
    };
}
