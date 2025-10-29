# Book Catalog API

Minimal API with SOLID layering, in-memory repositories, and SQL scripts.

## Project structure

- BookCatalog.Domain: entities
- BookCatalog.Application: contracts, interfaces, services, mapping
- BookCatalog.Infrastructure.InMemory: in-memory repositories with seed data
- BookCatalog.Api: minimal API with DI and endpoints
- sql/schema.sql: database schema and sample DML

## Prerequisites

- .NET SDK 8.x
- PowerShell or a terminal

## Run the API

1) From the repository root:

```bash
dotnet run --project BookCatalog.Api
```

2) Default URLs (as shown in console output):

- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:7000`

3) Swagger UI:

- `http://localhost:5000/swagger`
- `https://localhost:7000/swagger`

## Versioned base path

- All endpoints are under `/api/v1`.

## Endpoints and examples

### GET /api/v1/books

Query parameters:
- `publicationYear` (int, optional): filter by publication year
- `sortBy` (string, optional): `title` or omitted (defaults to by Id)
- `page` (int, optional, >=1) and `pageSize` (int, optional, >=1) for pagination

Examples:
```bash
curl "http://localhost:5000/api/v1/books"
curl "http://localhost:5000/api/v1/books?publicationYear=2011"
curl "http://localhost:5000/api/v1/books?sortBy=title"
curl "http://localhost:5000/api/v1/books?page=1&pageSize=2"
```

Response 200 OK (application/json):
```json
[
  { "id": 1, "title": "Clean Code", "authorName": "Robert C. Martin", "publicationYear": 2008 }
]
```

### GET /api/v1/authors/{id}/books

Examples:
```bash
curl "http://localhost:5000/api/v1/authors/1/books"
```

Response 200 OK:
```json
[
  { "id": 1, "title": "Clean Code", "authorName": "Robert C. Martin", "publicationYear": 2008 }
]
```

Author not found:
```bash
curl "http://localhost:5000/api/v1/authors/999/books"
```
Response 404 Not Found:
```json
{ "title": "Author not found", "detail": "Author 999 not found", "status": 404 }
```

### POST /api/v1/books

Body (application/json):
```json
{ "title": "Agile Principles", "authorId": 1, "publicationYear": 2009 }
```

Example:
```bash
curl -X POST "http://localhost:5000/api/v1/books" \
  -H "Content-Type: application/json" \
  -d "{\"title\":\"Agile Principles\",\"authorId\":1,\"publicationYear\":2009}"
```

Response 201 Created:
```json
{ "id": 4, "title": "Agile Principles", "authorName": "Robert C. Martin", "publicationYear": 2009 }
```

Validation failure (400):
```json
{ "title": "Validation failed", "detail": "Title cannot be empty", "status": 400 }
```

Unknown author (400):
```json
{ "title": "Author not found", "detail": "Author 999 not found", "status": 400 }
```

### PUT /api/v1/books/{id}

Body (application/json):
```json
{ "title": "Clean Code (Updated)", "authorId": 1, "publicationYear": 2008 }
```

Examples:
```bash
curl -X PUT "http://localhost:5000/api/v1/books/1" \
  -H "Content-Type: application/json" \
  -d "{\"title\":\"Clean Code (Updated)\",\"authorId\":1,\"publicationYear\":2008}"
```

Responses:
- 204 No Content on success
- 404 Not Found when the book does not exist
- 400 Bad Request for validation errors or unknown author

## HTTPS note

For self-signed dev certificates, use `-k` with curl, for example:
```bash
curl -k "https://localhost:7000/api/v1/books"
```

## Error format

Errors use RFC 7807 ProblemDetails shape:
```json
{ "title": "Validation failed", "detail": "<reason>", "status": 400 }
```

## SQL usage

The file `sql/schema.sql` contains:
- CREATE TABLE for `Authors` and `Books` with PK/FK
- IDENTITY on primary keys
- CHECK constraint for `PublicationYear <= current year`
- Index on `Books(AuthorId)`
- Sample INSERT, UPDATE, DELETE, and SELECT

You can run it in SSMS or `sqlcmd` against a local SQL Server instance.

## Notes

- This project follows SOLID via layering (Domain, Application, Infrastructure, Api) with DI.
- The in-memory repositories are thread-safe using `ConcurrentDictionary` and seeded data.
- Swagger UI is enabled for quick exploration.
