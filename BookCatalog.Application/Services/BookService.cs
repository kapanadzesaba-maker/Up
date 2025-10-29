namespace BookCatalog.Application.Services;

using BookCatalog.Application.Contracts;
using BookCatalog.Application.Interfaces;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookMapper _bookMapper;

    public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IBookMapper bookMapper)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _bookMapper = bookMapper;
    }

    public async Task<IReadOnlyList<BookDto>> GetAllAsync(int? publicationYear = null, string? sortBy = null, int? page = null, int? pageSize = null, CancellationToken cancellationToken = default)
    {
        var books = await _bookRepository.ListAsync(cancellationToken);
        var authors = await _authorRepository.ListAsync(cancellationToken);
        var authorLookup = authors.ToDictionary(a => a.Id);
        var query = books
            .Where(b => authorLookup.ContainsKey(b.AuthorId))
            .AsEnumerable();

        if (publicationYear.HasValue)
        {
            query = query.Where(b => b.PublicationYear == publicationYear.Value);
        }

        IEnumerable<BookDto> result = query.Select(b => _bookMapper.ToDto(b, authorLookup[b.AuthorId]));

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            result = sortBy.Equals("title", StringComparison.OrdinalIgnoreCase)
                ? result.OrderBy(d => d.Title)
                : result.OrderBy(d => d.Id);
        }

        if (page.HasValue && pageSize.HasValue && page.Value > 0 && pageSize.Value > 0)
        {
            result = result.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return result.ToList();
    }

    public async Task<IReadOnlyList<BookDto>> GetByAuthorAsync(int authorId, CancellationToken cancellationToken = default)
    {
        var author = await _authorRepository.GetByIdAsync(authorId, cancellationToken);
        if (author is null)
        {
            throw new KeyNotFoundException($"Author {authorId} not found");
        }

        var books = await _bookRepository.ListByAuthorAsync(authorId, cancellationToken);
        return books.Select(b => _bookMapper.ToDto(b, author)).ToList();
    }

    public async Task<BookDto> CreateAsync(CreateBookRequest request, CancellationToken cancellationToken = default)
    {
        Validate(request);
        var author = await _authorRepository.GetByIdAsync(request.AuthorId, cancellationToken);
        if (author is null)
        {
            throw new KeyNotFoundException($"Author {request.AuthorId} not found");
        }

        var book = new Domain.Book
        {
            Title = request.Title.Trim(),
            AuthorId = request.AuthorId,
            PublicationYear = request.PublicationYear
        };
        var created = await _bookRepository.AddAsync(book, cancellationToken);
        return _bookMapper.ToDto(created, author);
    }

    private static void Validate(CreateBookRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ArgumentException("Title cannot be empty", nameof(request.Title));
        }
        var currentYear = DateTime.UtcNow.Year;
        if (request.PublicationYear > currentYear)
        {
            throw new ArgumentException("PublicationYear cannot be in the future", nameof(request.PublicationYear));
        }
        if (request.AuthorId <= 0)
        {
            throw new ArgumentException("AuthorId must be positive", nameof(request.AuthorId));
        }
    }

    private static void Validate(UpdateBookRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ArgumentException("Title cannot be empty", nameof(request.Title));
        }
        var currentYear = DateTime.UtcNow.Year;
        if (request.PublicationYear > currentYear)
        {
            throw new ArgumentException("PublicationYear cannot be in the future", nameof(request.PublicationYear));
        }
        if (request.AuthorId <= 0)
        {
            throw new ArgumentException("AuthorId must be positive", nameof(request.AuthorId));
        }
    }

    public async Task UpdateAsync(int id, UpdateBookRequest request, CancellationToken cancellationToken = default)
    {
        Validate(request);
        var existing = await _bookRepository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            throw new KeyNotFoundException($"Book {id} not found");
        }
        var author = await _authorRepository.GetByIdAsync(request.AuthorId, cancellationToken);
        if (author is null)
        {
            throw new KeyNotFoundException($"Author {request.AuthorId} not found");
        }
        existing.Title = request.Title.Trim();
        existing.AuthorId = request.AuthorId;
        existing.PublicationYear = request.PublicationYear;
        await _bookRepository.UpdateAsync(existing, cancellationToken);
    }
}
