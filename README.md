# 🎟️ BookingHub

A minimal, high-performance airline-style booking system built with **.NET 8**, focusing on **Domain-Driven Design (DDD)** and **Clean Architecture** principles.

---

## 🚀 Overview
BookingHub demonstrates how to build a scalable, maintainable backend by isolating business logic from infrastructure. Key features include:
* **Domain Modeling:** Robust entities and value objects for Bookings and Flights.
* **Business Invariants:** Strict enforcement of rules (e.g., seat availability, date validation) within the Domain layer.
* **Persistence:** EF Core with SQLite provider.
* **Date Queries:** Efficient range-based filtering for booking records.

## 🏗️ Architecture
The project follows a layered approach where dependencies point inwards:
**API** → **Application** → **Domain** ← **Infrastructure**

| Layer | Responsibility |
| :--- | :--- |
| **Domain** | Entities, Value Objects, Domain Events, and Core Interfaces. |
| **Application** | Use cases (MediatR handlers), DTOs, and Request Mapping. |
| **Infrastructure** | EF Core DbContext, Repository implementations, and Migrations. |
| **API** | Minimal Controllers, Swagger integration, and Request Validation. |

### Architecture Diagram

```mermaid
graph TB
    subgraph API["BookingHub.API — Presentation Layer"]
        Controller["BookingsController"]
        Swagger["Swagger / OpenAPI"]
        Program["Program.cs<br/>(DI · Middleware · Seeding)"]
    end

    subgraph Application["BookingHub.Application — Use Case Layer"]
        direction TB
        subgraph Handlers["MediatR Handlers"]
            CreateHandler["CreateBookingHandler"]
            GetByIdHandler["GetBookingByIdHandler"]
            GetByDateHandler["GetBookingsByDateHandler"]
        end
        subgraph DTOs["DTOs & Requests"]
            CreateReq["CreateBookingRequest"]
            BookingDto["BookingDto"]
            Queries["GetBookingByIdQuery<br/>GetBookingsByDateRangeQuery"]
        end
    end

    subgraph Domain["BookingHub.Domain — Core Domain Layer"]
        direction TB
        subgraph Entities["Aggregate Roots"]
            Booking["Booking"]
            Flight["Flight"]
        end
        subgraph VOs["Value Objects"]
            BookingId["BookingId"]
            FlightId["FlightId"]
            SeatCount["SeatCount"]
            BookingDate["BookingDate"]
            BookingDest["BookingDestination"]
        end
        subgraph Events["Domain Events"]
            DomainEvent["DomainEvent"]
            BookingCreated["BookingCreatedEvent"]
        end
        subgraph Interfaces["Repository Contracts"]
            IBookingRepo["IBookingRepository"]
            IFlightRepo["IFlightRepository"]
        end
    end

    subgraph Infrastructure["BookingHub.Infrastructure — Persistence Layer"]
        direction TB
        DbContext["BookingDbContext<br/>(EF Core)"]
        BookingRepo["BookingRepository"]
        FlightRepo["FlightRepository"]
        Migrations["Migrations"]
        SwaggerEx["Swagger Examples"]
        SQLite[("SQLite Database")]
    end

    subgraph Tests["BookingHub.Tests"]
        xUnit["xUnit Tests<br/>(Domain · Application)"]
    end

    %% Request flow
    Controller -->|"sends MediatR request"| Handlers
    Handlers -->|"uses"| DTOs
    Handlers -->|"calls"| Interfaces
    Handlers -->|"creates / validates"| Entities

    %% Domain internals
    Entities -->|"composed of"| VOs
    Booking -->|"raises"| BookingCreated
    BookingCreated -.->|"extends"| DomainEvent

    %% Infrastructure implements Domain
    BookingRepo -.->|"implements"| IBookingRepo
    FlightRepo -.->|"implements"| IFlightRepo
    BookingRepo --> DbContext
    FlightRepo --> DbContext
    DbContext --> SQLite
    DbContext --> Migrations

    %% DI wiring
    Program -->|"registers services"| Controller
    Program -->|"configures"| DbContext
    Program -->|"registers"| Swagger

    %% Tests
    xUnit -.->|"tests"| Handlers
    xUnit -.->|"tests"| Entities

    %% Styling
    classDef apiStyle fill:#4A90D9,stroke:#2E6DA4,color:#fff
    classDef appStyle fill:#F5A623,stroke:#D4891A,color:#fff
    classDef domainStyle fill:#7ED321,stroke:#5DA018,color:#fff
    classDef infraStyle fill:#9B59B6,stroke:#7D3C98,color:#fff
    classDef testStyle fill:#E74C3C,stroke:#C0392B,color:#fff
    classDef dbStyle fill:#2C3E50,stroke:#1A252F,color:#fff

    class Controller,Swagger,Program apiStyle
    class CreateHandler,GetByIdHandler,GetByDateHandler,CreateReq,BookingDto,Queries appStyle
    class Booking,Flight,BookingId,FlightId,SeatCount,BookingDate,BookingDest,DomainEvent,BookingCreated,IBookingRepo,IFlightRepo domainStyle
    class DbContext,BookingRepo,FlightRepo,Migrations,SwaggerEx infraStyle
    class SQLite dbStyle
    class xUnit testStyle
```

#### Dependency Rule

```mermaid
graph LR
    A["API"] -->|depends on| B["Application"]
    B -->|depends on| C["Domain"]
    D["Infrastructure"] -->|depends on| C
    A -->|depends on| D

    style A fill:#4A90D9,stroke:#2E6DA4,color:#fff
    style B fill:#F5A623,stroke:#D4891A,color:#fff
    style C fill:#7ED321,stroke:#5DA018,color:#fff
    style D fill:#9B59B6,stroke:#7D3C98,color:#fff
```

> **Domain** is the innermost layer with zero external dependencies. **Infrastructure** implements Domain interfaces (Dependency Inversion Principle). **Application** orchestrates use cases via MediatR. **API** is the entry point that wires everything together.

#### Request Flow

```mermaid
sequenceDiagram
    participant Client
    participant Controller as BookingsController
    participant MediatR
    participant Handler as CreateBookingHandler
    participant FlightRepo as IFlightRepository
    participant BookingEntity as Booking (Domain)
    participant BookingRepo as IBookingRepository
    participant DB as SQLite

    Client->>Controller: POST /api/bookings
    Controller->>MediatR: Send(CreateBookingRequest)
    MediatR->>Handler: Handle(request)
    Handler->>FlightRepo: GetByIdAsync(flightId)
    FlightRepo->>DB: Query Flight
    DB-->>FlightRepo: Flight entity
    FlightRepo-->>Handler: Flight
    Handler->>Handler: Validate seat availability
    Handler->>BookingEntity: Booking.Create(date, seats, dest, flightId)
    Note over BookingEntity: Enforces invariants via Value Objects<br/>Raises BookingCreatedEvent
    Handler->>BookingRepo: AddAsync(booking)
    BookingRepo->>DB: INSERT Booking
    Handler->>FlightRepo: UpdateAsync(flight)
    FlightRepo->>DB: UPDATE Flight seats
    Handler-->>MediatR: BookingDto
    MediatR-->>Controller: BookingDto
    Controller-->>Client: 201 Created
```

## 🧠 Key DDD Concepts
* **Aggregate Roots:** `Booking` and `Flight` ensure data consistency.
* **Value Objects:** `BookingId`, `FlightId`, and `SeatCount` prevent "primitive obsession."
* **Domain Events:** `BookingCreatedEvent` handles decoupled side effects.
* **Business Rules:** * Seat counts must be greater than zero.
    * Backdated bookings are prohibited.
    * Bookings cannot exceed a flight's total seat capacity.

## 🌐 API Endpoints
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/bookings` | Create a new flight booking |
| `GET` | `/api/bookings/{id}` | Retrieve booking details by ID |
| `GET` | `/api/bookings/bydaterange` | Filter bookings by `startDate` & `endDate` |

## 🗄️ Persistence & Data
* **Engine:** EF Core + SQLite.
* **Mappings:** Value Objects are mapped using EF Conversions.
* **Seed Data:** Pre-configured flights included:
    * Delhi → Mumbai (100 seats)
    * Bangalore → Hyderabad (50 seats)

## 🧪 Testing
The solution includes **xUnit** tests for the Domain and Application layers, utilizing an in-memory database provider to ensure logic reliability without infrastructure overhead.

## ▶️ Run Locally
1. **Clone the repository.**
2. **Update the database:**
   ```bash
   dotnet ef database update -p BookingHub.Infrastructure -s BookingHub.API

## 🚀 Future Roadmap

### 1. Reliability & Consistency

- **Transactional consistency**  
  Introduce a unit of work or explicit EF Core transaction so creating a booking and updating flight seats are atomic.

- **Concurrency control**  
  Add optimistic concurrency checks (rowversion / concurrency tokens) for `Flight` to safely handle simultaneous seat bookings.


### 2. Domain Events & Integration

- **Domain event dispatch**  
  Wire `BookingCreatedEvent` into an event dispatcher (e.g. MediatR notifications or a custom dispatcher) and implement handlers for side effects (e.g. notifications, projections, audit logs).

- **Transactional outbox**  
  Implement a transactional outbox pattern so domain events are persisted with the main transaction and published reliably to external systems (e.g. message bus).


### 3. Validation & Error Handling

- **Centralized validation**  
  Add request validation (e.g. FluentValidation) to enforce input rules before handlers execute.

- **Consistent error responses**  
  Introduce global exception handling middleware to standardize error payloads (`ProblemDetails`), and map domain/validation/business exceptions to appropriate HTTP status codes.


### 4. Testing & Quality

- **Unit tests**  
  Add tests for value objects, entities (`Flight.ReduceAvailableSeats`, `Booking.Create`), and application handlers (happy paths and edge cases).

- **Integration tests**  
  Add tests against a real or in-memory SQLite database to validate repository behavior and end-to-end booking flows.

- **API tests**  
  Use `WebApplicationFactory` to exercise HTTP endpoints and verify status codes, contracts, and error handling.


### 5. Features & Extensibility

- **Flight search & listing**  
  Add endpoints to search flights by route/date and expose the currently seeded flights.

- **Cancellation & modification flows**  
  Implement booking cancellation and modification use cases, including returning seats to flights and enforcing new invariants.

- **User / customer context**  
  Introduce a customer or user concept, link bookings to customers, and consider authentication/authorization for protected endpoints.

- **Pagination & filtering**  
  Extend query endpoints with pagination, sorting, and richer filters for bookings and flights.


### 6. Architecture & Operational Concerns

- **Configuration & environments**  
  Split configuration for local/dev/prod (connection strings, logging levels) and add environment-specific appsettings.

- **Observability**  
  Add structured logging, basic metrics, and request tracing to support diagnostics in real environments.

- **Database evolution**  
  Harden migration strategy for production (explicit migration commands, versioning, and rollback approach).
