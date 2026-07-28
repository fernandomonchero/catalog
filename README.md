# Catalog Consolidation

This project implements a catalog consolidation service for a marketplace scenario, where products from multiple sellers are imported into a centralized catalog.

The application receives a JSON file containing products from different sellers and consolidates them into an existing SQLite database, preventing duplicated products while preserving seller relationships.

## Features

- Import products from a JSON file
- Prevent duplicated products using **Brand + Name** matching
- Associate multiple sellers to the same product
- Validate products and sellers before importing
- Normalize product data before persistence
- Return an import summary containing processed records and validation errors
- REST API built with ASP.NET Core 8
- SQLite database
- Docker support
- Unit and Integration Tests

## Architecture

The solution follows **Clean Architecture** principles, separating responsibilities into independent layers.

```
src
├── Catalog.Api
├── Catalog.Application
├── Catalog.Domain
└── Catalog.Infrastructure

tests
├── Catalog.UnitTests
└── Catalog.IntegrationTests

data
└── SQLite database
```

### Responsibilities

| Project | Responsibility |
|----------|----------------|
| Catalog.Api | HTTP endpoints and dependency injection |
| Catalog.Application | Use cases, DTOs and application orchestration |
| Catalog.Domain | Business rules, entities and validations |
| Catalog.Infrastructure | Entity Framework Core, repositories and persistence |

## Product Consolidation Strategy

A product is considered duplicated when another product with the same:

- Brand
- Name

already exists in the catalog.

If a duplicated product is found:

- A new Product is **not** created.
- A new SellerProduct relationship is created only if it does not already exist.

## Technologies

- .NET 8
- ASP.NET Core
- Entity Framework Core
- SQLite
- FluentValidation
- Docker
- NUnit

## Running locally

### Prerequisites

- .NET 8 SDK

Clone the repository:

```bash
git clone <repository-url>
```

Restore packages:

```bash
dotnet restore
```

Run the API:

```bash
dotnet run --project src/Catalog.Api
```

Swagger:

```
http://localhost:5000/swagger
```

(or the port displayed in the console)

---

## Running with Docker

Build and start the application:

```bash
docker compose up --build
```

Swagger:

```
http://localhost:8080/swagger
```

---

## Running Tests

Unit Tests

```bash
dotnet test tests/Catalog.UnitTests
```

Integration Tests

```bash
dotnet test tests/Catalog.IntegrationTests
```

Or execute all tests:

```bash
dotnet test
```

---

## API Endpoints

### Import Catalog

```
POST /catalog/import
```

Imports a list of products into the catalog.

### Get Products

```
GET /catalog
```

Returns all products.

### Get Product Details

```
GET /catalog/product/{id}
```

Returns a product by its identifier.

---

## Import Response

```json
{
  "success": true,
  "data": {
    "totalRecords": 10,
    "totalErrors": 2,
    "errors": [
      "Product name is required."
    ]
  }
}
```

---

## Testing Strategy

The solution contains two testing layers.

### Unit Tests

Focused on:

- Domain business rules
- Application use cases
- Validation logic

### Integration Tests

Focused on:

- REST API endpoints
- Database persistence
- End-to-end import workflow

---

## Design Decisions

Some intentional design decisions were made during the implementation:

- Clean Architecture to improve maintainability and separation of concerns.
- Domain Service responsible for the catalog consolidation logic.
- Repository Pattern for data access abstraction.
- SQLite used to match the challenge requirements.
- Docker support to simplify application setup.

---

## Future Improvements

Possible improvements for a production-ready solution:

- Background processing for large imports
- Pagination and filtering endpoints
- Structured logging
- Metrics and health checks
- Authentication and authorization
- Asynchronous import processing
- OpenTelemetry support
