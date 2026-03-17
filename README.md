\# 🎟️ BookingHub - Ticket Booking System (DDD + Clean Architecture)



\## 📌 Overview



\*\*BookingHub\*\* is a ticket booking system designed using \*\*Domain-Driven Design (DDD)\*\* and \*\*Clean Architecture\*\* principles.



It demonstrates how to build a scalable, maintainable, and testable backend system by properly separating concerns and encapsulating business logic.



\### Features



\* Create bookings

\* Retrieve bookings by ID

\* Retrieve bookings by date



\---



\## 🧠 Architectural Approach



This project strictly follows:



\* ✅ Domain-Driven Design (DDD)

\* ✅ Clean Architecture

\* ✅ SOLID Principles



\---



\## 🏗️ Solution Structure



```

BookingHub.sln

│

├── BookingHub.Domain

├── BookingHub.Application

├── BookingHub.Infrastructure

├── BookingHub.API

└── BookingHub.Tests

```



\---



\## 🔄 Dependency Flow



```

API → Application → Domain

Infrastructure → Domain + Application

```



\* Domain layer is completely independent

\* Business logic resides only in Domain

\* Infrastructure concerns are isolated



\---



\## 🧩 Domain Design



\### 🟢 Aggregate Root



\*\*Booking\*\* is the aggregate root.



Responsibilities:



\* Maintains business invariants

\* Controls state changes

\* Raises domain events



\---



\### 🟣 Value Objects



To avoid primitive obsession, the system uses:



\* `BookingId` → wraps Guid

\* `BookingDate` → ensures valid dates

\* `SeatCount` → enforces seat rules



Benefits:



\* Immutability

\* Encapsulation of validation

\* Strong domain modeling



\---



\### 🔵 Domain Events



Example:



\* `BookingCreatedEvent`



Why:



\* Decouples side effects

\* Improves scalability

\* Enables future integrations



\---



\## ⚙️ Application Layer



Handles use cases:



\* CreateBookingHandler

\* GetBookingByIdHandler

\* GetBookingsByDateHandler



Responsibilities:



\* Orchestrates domain logic

\* Contains no business rules



\---



\## 🗄️ Infrastructure Layer



Handles:



\* EF Core (SQLite)

\* Repository implementations



Key points:



\* Value Object mapping

\* Persistence isolation



\---



\## 🌐 API Layer



\* ASP.NET Core Web API

\* Thin controllers

\* Uses MediatR



\### Endpoints



```

POST   /api/bookings

GET    /api/bookings/{id}

GET    /api/bookings/date/{date}

```



\---



\## 🧪 Testing



\* xUnit test project

\* Focus on Domain and Application layers



\---



\## 🔐 Business Rules



\* Booking date cannot be in the past

\* Seat count must be greater than zero

\* Maximum seat limit enforced

\* All validations live in Domain



\---



\## 🚀 Technologies



\* .NET Web API

\* Entity Framework Core

\* SQLite

\* MediatR

\* xUnit



\---



\## 💡 Key Design Decisions



\### ✔ Value Objects



Encapsulate validation and prevent invalid states.



\### ✔ Aggregate Root



Ensures consistency and controls changes.



\### ✔ Domain Events



Decouple side effects and improve extensibility.



\### ✔ Clean Architecture



Improves maintainability, testability, and scalability.



\---



\## 📈 Future Improvements



\* Event-driven architecture

\* Payment integration

\* Seat-level booking

\* Caching

\* Authentication \& Authorization



\---



\## 👨‍💻 Author



Nishant Taneja



\---



\## ⭐ Final Note



This project demonstrates how to apply DDD and Clean Architecture in a real-world backend system.



