# Course Manager API

A production-minded ASP.NET Core REST API for managing courses, built as a compact example of clean backend engineering.

## Highlights

- ASP.NET Core / C# on .NET 9
- Entity Framework Core with SQL Server
- Explicit request/response contracts instead of exposing persistence entities
- Input validation with clear HTTP semantics
- Async database access with cancellation support
- Read-only query optimization with `AsNoTracking`
- OpenAPI / Swagger documentation
- Database migrations with EF Core
- GitHub Actions CI

## Architecture

The API keeps its HTTP contract separate from its persistence model. Controllers accept validated request DTOs and return explicit response DTOs, which prevents accidental database-model changes from becoming breaking API changes.

For a project of this size, the architecture is deliberately lightweight: the controller coordinates HTTP behavior while EF Core provides persistence. Additional service/repository layers would add ceremony without meaningful isolation yet. The structure is designed so those layers can be introduced when business rules justify them.

```text
Client
  |
  v
CoursesController
  |-- request validation
  |-- HTTP semantics
  |-- response mapping
  v
AppDbContext / EF Core
  |
  v
SQL Server
```

## API

| Method | Endpoint | Purpose |
| --- | --- | --- |
| GET | `/api/courses` | List courses |
| GET | `/api/courses/{id}` | Get a course |
| POST | `/api/courses` | Create a course |
| PUT | `/api/courses/{id}` | Update a course |
| DELETE | `/api/courses/{id}` | Delete a course |

## Design decisions

### Explicit API contracts

`CreateCourseRequest`, `UpdateCourseRequest`, and `CourseResponse` keep transport concerns independent from the EF entity and make validation visible at the API boundary.

### Safe update semantics

Updates load the persisted entity first instead of marking an arbitrary client-supplied object as modified. This avoids over-posting and makes the fields that may change explicit.

### Efficient reads

Read endpoints use `AsNoTracking()` because returned entities are not modified, reducing unnecessary EF Core tracking overhead.

### Cancellation-aware I/O

Database operations accept the request cancellation token so abandoned HTTP requests do not needlessly keep database work alive.

## Run locally

Requirements:

- .NET 9 SDK
- SQL Server / SQL Server Express / LocalDB

Configure `ConnectionStrings:DefaultConnection`, then run:

```bash
dotnet restore
dotnet ef database update
dotnet run
```

In Development, Swagger UI is enabled for interactive API exploration.

## What I would add next

- Integration tests using `WebApplicationFactory`
- Pagination, filtering, and search
- Optimistic concurrency for competing updates
- Authentication and authorization for write operations
- Structured observability for production deployments

## Why this repository exists

This repository is intentionally small enough to review quickly while demonstrating the decisions I care about in production code: clear contracts, predictable behavior, maintainability, data safety, and avoiding unnecessary complexity.
