namespace BookCatalog.Application.Interfaces;

using BookCatalog.Application.Contracts;
using BookCatalog.Domain;

public interface IBookMapper
{
    BookDto ToDto(Book book, Author author);
}
