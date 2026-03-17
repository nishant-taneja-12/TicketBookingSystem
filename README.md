\# 🎟️ BookingHub - Ticket Booking System (DDD + Clean Architecture)



\## 📌 Overview



\*\*BookingHub\*\* is a ticket booking system designed using \*\*Domain-Driven Design (DDD)\*\* and \*\*Clean Architecture\*\* principles.

It demonstrates how to build a scalable, maintainable, and testable backend system by properly separating concerns and encapsulating business logic.



The application allows users to:



\* Create bookings

\* Retrieve bookings by ID

\* Retrieve bookings by date



\---



\## 🧠 Architectural Approach



This project strictly follows:



\* ✅ \*\*Domain-Driven Design (DDD)\*\*

\* ✅ \*\*Clean Architecture\*\*

\* ✅ \*\*SOLID Principles\*\*



The system is structured into multiple layers to ensure \*\*clear separation of concerns\*\* and \*\*dependency control\*\*.



\---



\## 🏗️ Solution Structure



```

BookingHub.sln

│

├── BookingHub.Domain          # Core business logic (Entities, Value Objects, Domain Events)

├── BookingHub.Application     # Use cases (Handlers, DTOs)

├── BookingHub.Infrastructure  # Persistence (EF Core, SQLite, Repositories)

├── BookingHub.API             # Entry point (Controllers, DI setup)

└── BookingHub.Tests           # Unit tests

```



\---



\## 🔄 Dependency Flow (Clean Architecture)



```

API → Application → Domain

Infrastructure → Domain + Application

```



\* The \*\*Domain layer is completely independent\*\*

\* No business logic exists outside the Domain

\* Infrastructure concerns (DB, EF Core) are isolated



\---



\## 🧩 Domain Design (DDD)



\### 🟢 Aggregate Root



\*\*Booking\*\* is the central aggregate root.



Responsibilities:



\* Maintains business invariants

\* Controls state changes

\* Raises domain events



\---



\### 🟣 Value Objects



To avoid \*\*primitive obsession\*\*, the system uses Value Objects:



\* `BookingId` → wraps Guid

\* `BookingDate` → enforces valid future dates

\* `SeatCount` → enforces seat constraints (e.g., > 0, max limit)



\#### Benefits:



\* Encapsulated validation

\* Immutability

\* Strong domain modeling



\---



\### 🔵 Domain Events



Example:



\* `BookingCreatedEvent`



Raised when:



\* A new booking is successfully created



\#### Why Domain Events?



\* Decouple side effects from core logic

\* Enable scalability (notifications, integrations)

\* Improve maintainability



\---



\## ⚙️ Application Layer



Handles \*\*use cases\*\* and orchestration.



\### Use Cases:



\* `CreateBookingHandler`

\* `GetBookingByIdHandler`

\* `GetBookingsByDateHandler`



Responsibilities:



\* Coordinate domain logic

\* Do NOT contain business rules



\---



\## 🗄️ Infrastructure Layer



Handles external concerns such as:



\* Database (SQLite via EF Core)

\* Repository implementations



\### Key Features:



\* EF Core configurations

\* Value Object mappings using conversions

\* Persistence isolation



\---



\## 🌐 API Layer



\* ASP.NET Core Web API

\* Thin controllers

\* Delegates logic to Application layer via MediatR



\### Example Endpoints:



```

POST   /api/bookings

GET    /api/bookings/{id}

GET    /api/bookings/date/{date}

```



\---



\## 🧪 Testing



\* Dedicated test project (`BookingHub.Tests`)

\* Focus on Domain and Application layers

\* Ensures business rules are validated independently



\---



\## 🔐 Business Rules Enforced



\* Booking date cannot be in the past

\* Seat count must be greater than zero

\* Maximum seat limit per booking enforced

\* All validations reside inside the Domain



\---



\## 🚀 Technologies Used



\* .NET (ASP.NET Core Web API)

\* Entity Framework Core

\* SQLite

\* MediatR

\* xUnit (for testing)



\---



\## 💡 Key Design Decisions



\### ✔ Use of Value Objects



Prevents invalid states and keeps validation close to data.



\### ✔ Aggregate Root Pattern



Ensures consistency and control over domain operations.



\### ✔ Domain Events



Allows extensibility without tightly coupling components.



\### ✔ Clean Architecture



Improves:



\* Testability

\* Maintainability

\* Scalability



\---



\## 📈 Future Enhancements



\* Event-driven architecture (publish domain events via message broker)

\* Payment integration

\* Seat-level booking (advanced modeling)

\* Caching for read queries

\* Authentication \& authorization



\---



\## 🧠 What This Project Demonstrates



\* Real-world application of \*\*DDD concepts\*\*

\* Proper \*\*layered architecture\*\*

\* Strong separation between \*\*domain and infrastructure\*\*

\* Writing \*\*maintainable and scalable backend systems\*\*



\---



\## 👨‍💻 Author



\*\*Nishant Taneja\*\*



\---



\## ⭐ Final Note



This project is designed not just to work —

but to demonstrate \*\*how to think like a software architect\*\*.



