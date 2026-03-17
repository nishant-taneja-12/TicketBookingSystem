# BookingHub

BookingHub is a minimal, realistic example of a booking system implemented with Clean Architecture and Domain-Driven Design (DDD) principles. The solution demonstrates how to model aggregates and value objects, implement application use-cases, and persist data using Entity Framework Core with SQLite. The project is intentionally small but structured to serve as a reference for pragmatic DDD and layered architecture in .NET 8.

## What this project does

- Provides a simple domain model for bookings and flights.
- Supports creating bookings for a given flight while enforcing domain invariants (seat counts, departure date constraints).
- Persists domain entities using EF Core and SQLite, with migrations and seeded initial flight data.
- Exposes a small Web API to create bookings and query bookings within a date range.
- Includes unit tests for domain and application behavior.

## Design principles and architecture

This repository follows Clean Architecture and DDD patterns:

- Domain Layer (`BookingHub.Domain`)
  - Contains entities (aggregates), value objects, domain events, and repository interfaces.
  - Business rules live inside aggregates and value objects. No persistence or framework concerns leak into domain code.
  - Examples: `Booking`, `Flight`, `BookingId`, `FlightId`, `BookingDate`, `SeatCount`.

- Application Layer (`BookingHub.Application`)
  - Hosts use-case handlers (MediatR), DTOs, and request/response models.
  - Orchestrates domain operations and repository interactions, but it does not contain business rules.
  - Example: `CreateBookingHandler` fetches `Flight` from repository, validates seat availability, updates the flight, and creates the booking aggregate.

- Infrastructure Layer (`BookingHub.Infrastructure`)
  - EF Core `DbContext`, repository implementations, migrations, and integration code.
  - Maps domain model to persistence (value conversions for value objects, owned types for small value objects).
  - Seeds initial data (two flights) and implements `IFlightRepository` and `IBookingRepository`.

- API Layer (`BookingHub.API`)
  - Thin ASP.NET Core Web API controllers that translate HTTP to application requests and return DTOs.
  - Handles input validation (for example, `startDate` <= `endDate` for range queries) and error mapping (domain exceptions → HTTP 400).

Other practices:
- Explicit Value Objects: strongly-typed IDs (`BookingId`, `FlightId`) and small VOs (`BookingDate`, `SeatCount`) encapsulate invariants and improve intention-revealing code.
- Aggregate roots: `Booking` and `Flight` are aggregates; repositories work with aggregates and persist changes via EF Core.
- No repositories inside domain: domain defines repository contracts in `BookingHub.Domain.Interfaces`; implementations are in infrastructure.
- Tests: unit tests live in `BookingHub.Tests` and target domain logic and application handlers.

## Persistence and EF Core

- EF Core maps value objects using primitive backing properties (e.g., `IdValue`) and `ValueConverter` for complex VOs (e.g., `BookingDate`).
- Database provider: SQLite (used in `BookingHub.API` via connection string `DefaultConnection`), and in-memory provider used for tests.
- Migrations are located in `BookingHub.Infrastructure/Migrations` and include an `AddFlightEntity` migration to create the `Flights` table and seed two flights.
- Seeded flights (example IDs and data):
  - `11111111-1111-1111-1111-111111111111` — Delhi → Mumbai, 2027-01-01T10:00Z, 100 seats
  - `22222222-2222-2222-2222-222222222222` — Bangalore → Hyderabad, 2027-02-01T15:30Z, 50 seats

NOTE: The project uses an explicit approach to prevent EF from attempting to map domain value objects directly by exposing primitive backing properties (e.g., `IdValue`) and ignoring the VO properties in the model mapping. This keeps domain code clean while enabling persistence.

## API Endpoints

- POST `/api/bookings` — Create booking
  - Request: `CreateBookingRequest` includes `BookingDate`, `SeatCount`, optional `Destination`, and `FlightId`.
  - Behavior: Validates flight existence and seat availability, reduces flight seats, creates booking, returns `BookingDto`.

- GET `/api/bookings/bydaterange?startDate={start}&endDate={end}` — Get bookings in a date range
  - Validates `startDate <= endDate`. Returns bookings whose `BookingDate` falls within the inclusive date range. Time component is normalized to dates on queries.

- GET `/api/bookings/{id}` — Get booking by id

## How to run (developer machine)

Prerequisites:
- .NET 8 SDK

Run locally:
1. Restore packages and build solution:
   - `dotnet build`
2. Apply EF Core migrations and create SQLite database (from solution root):
   - `dotnet ef database update -p BookingHub.Infrastructure -s BookingHub.API`
   - This will create or update `bookings.db` (configured via `DefaultConnection` in `BookingHub.API`).
3. Run the API:
   - `cd BookingHub.API` then `dotnet run`
4. Use Swagger UI at `https://localhost:<port>/swagger` to exercise endpoints.

Running tests:
- From solution root: `dotnet test` — unit tests use the in-memory provider and run quickly.

Notes:
- I did not include UI or payment features; the focus is domain modeling and persistence.
- The project contains sample seed data (flights) that are added during migration or initialization.

## Coding standards & contribution notes

- The project uses explicit value objects and favors immutability inside domain layer.
- Repositories are defined as domain contracts and implemented in infrastructure.
- Keep controllers thin: validation and mapping only, delegating work to application handlers.
- When contributing, follow existing patterns: place domain rules inside aggregates/value objects, not in application or infrastructure layers.

## Troubleshooting

- If EF migration commands fail due to design-time DbContext issues, ensure the startup project (`BookingHub.API`) builds and DI registrations do not create design-time-only constraints. Use the same startup assembly when running `dotnet ef` commands (the `-s` option).
- If you need to reseed flights or modify migration history during development, review `BookingHub.Infrastructure/Migrations` and use `dotnet ef migrations remove` or `add` as required.

## Contact / Maintainers

- Repository: `https://github.com/nishant-taneja-12/TicketBookingSystem` (origin)

---

This README provides an overview to get started and explains the architectural decisions. If you want, I can produce a short mermaid diagram of the architecture or add a `CONTRIBUTING.md` with coding standards and PR guidelines.
