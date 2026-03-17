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
