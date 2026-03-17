# Ticket Booking System (DDD + Clean Architecture)

## 🌐 Overview

**BookingHub** is a ticket booking system designed using **Domain-Driven Design (DDD)** and **Clean Architecture** principles. 

It demonstrates how to build a scalable, maintainable, and testable backend system by properly separating concerns and encapsulating business logic.

### Features
* Create bookings
* Retrieve bookings by ID
* Retrieve bookings by date range

---

## 🧠 Architectural Approach

This project strictly follows:
* ✅ **Domain-Driven Design (DDD)**
* ✅ **Clean Architecture**
* ✅ **SOLID Principles**

---

## 🏗️ Solution Structure

```text
BookingHub.sln
│
├── BookingHub.Domain         # Enterprise logic & Entities
├── BookingHub.Application    # Use Cases & Handlers
├── BookingHub.Infrastructure # Persistence & External Services
├── BookingHub.API            # Presentation Layer
└── BookingHub.Tests          # Unit & Integration Tests
