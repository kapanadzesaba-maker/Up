namespace BookCatalog.Application.Mapping;

using BookCatalog.Application.Contracts;
using BookCatalog.Application.Interfaces;
using BookCatalog.Domain;

public class BookMapper : IBookMapper
{
    public BookDto ToDto(Book book, Author author)
    {
        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            AuthorName = author.Name,
            PublicationYear = book.PublicationYear
        };
    }
}
