# Book Catalog API

Minimal API with SOLID layering, in-memory repositories, and SQL scripts.

## Projects

- BookCatalog.Domain: entities
- BookCatalog.Application: contracts, interfaces, services, mapping
- BookCatalog.Infrastructure.InMemory: in-memory repositories with seed data
- BookCatalog.Api: minimal API with DI and endpoints

## Endpoints

- GET /api/books
- GET /api/authors/{id}/books
- POST /api/books

## SQL

See `sql/schema.sql` for CREATE, INSERT, UPDATE, DELETE, and SELECT.
