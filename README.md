<div align="center">

# Room Booking API

**A test-driven ASP.NET Core Web API where every behavior started as a failing test.**

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![EF Core](https://img.shields.io/badge/EF%20Core-9.0-512BD4?style=for-the-badge)
![xUnit](https://img.shields.io/badge/xUnit-tested-5E1F87?style=for-the-badge)
![Tests](https://img.shields.io/badge/tests-13%20cases-2EA44F?style=for-the-badge)

[Overview](#overview) · [Architecture](#architecture) · [TDD in Action](#tdd-in-action) · [Test Suite](#test-suite) · [API](#api-reference) · [Getting Started](#getting-started)

</div>

---

## Overview

Room Booking API lets a client book a conference room for a given date. The API checks which rooms are free, books the first available one, and returns a clear success or failure result.

I built it to apply what I learned in **[Test Driven Development (TDD) in .NET](https://www.udemy.com/)** by Trevoir Williams, and to practice a mindset I care about: **AI can generate code quickly, but a developer has to be able to prove it is correct.** Here, the tests define the requirements first, and the code exists to satisfy them.

**What this project demonstrates**

- The Red, Green, Refactor cycle applied to real business rules
- Business logic fully isolated from the database and the web layer
- Unit tests, controller tests, and EF Core data-access tests in separate projects
- Dependency inversion: the core defines the contracts, the outer layers implement them
- Input validation built into the domain model

---

## Architecture

The solution is split into four layers plus three test projects. Dependencies point inward, so the business logic never depends on EF Core or ASP.NET.

```mermaid
flowchart LR
    API["<b>RoomBookingApp.API</b><br/>Controllers · Swagger · DI setup"]
    Core["<b>RoomBookingApp.Core</b><br/>Booking logic · Service contracts"]
    Persistence["<b>RoomBookingApp.Persistence</b><br/>EF Core · Data access"]
    Domain["<b>RoomBookingApp.Domain</b><br/>Entities · Validation rules"]

    API --> Core
    API --> Persistence
    Persistence --> Core
    Persistence --> Domain
    Core --> Domain
```

| Layer | Responsibility | Tested by |
|---|---|---|
| `RoomBookingApp.API` | HTTP endpoints, request validation, Swagger, dependency injection | `RoomBookingApp.API.Tests` |
| `RoomBookingApp.Core` | `RoomBookingRequestProcessor` with the booking rules, plus `IRoomBookingService` | `RoomBookingApp.Core.Test` |
| `RoomBookingApp.Persistence` | `RoomBookingService` and `RoomBookingAppDbContext` (EF Core) | `RoomBookingApp.Persistence.Test` |
| `RoomBookingApp.Domain` | `Room`, `RoomBooking`, shared base model with validation | Covered through the layers above |

### How a booking works

1. The client sends `POST /api/RoomBooking` with a name, email, and date.
2. The controller validates the model (required fields, valid email, **date must be in the future**).
3. The processor asks `IRoomBookingService` for the rooms available on that date.
4. If a room is free, the booking is saved against the first available room and the result is `Success` with a booking ID.
5. If no room is free, the API returns `400 Bad Request` with `No Rooms Available For Given Date`.

---

## TDD in Action

Each rule below began as a test that failed, then got the smallest implementation to pass, then was refactored.

| Requirement (written as a test first) | Test |
|---|---|
| The result mirrors the request values | `Should_Return_RoomBooking_Response_With_Request_Values` |
| A null request is rejected with a clear `ArgumentNullException` | `Should_Throw_ArgumentNullException_When_Request_Is_Null` |
| A booking is saved with the correct room | `Should_Save_RoomBooking_Request` |
| Nothing is saved when no room is free | `Should_Not_Save_RoomBooking_Request_If_Room_Is_Unavailable` |
| The result flag reflects success or failure | `Should_Return_SuccessOrFailure_Flag_In_Result` |
| A booking ID is returned only on success | `Should_Return_BookingId_In_Result` |
| The controller returns `200` or `400` and calls the processor only when valid | `Should_Call_Booking_Method_When_Valid` |
| Only unbooked rooms are returned for a date | `Should_Return_Available_Rooms` |
| A booking is persisted correctly | `Should_Save_Room_Booking` |

A short example of the style, using Moq to isolate the unit and Shouldly for readable assertions:

```csharp
[Fact]
public void Should_Not_Save_RoomBooking_Request_If_Room_Is_Unavailable()
{
    _availableRooms.Clear(); // simulate a fully booked day

    _processor.BookRoom(_request);

    _roomBookingServiceMock.Verify(x => x.Save(It.IsAny<RoomBooking>()), Times.Never);
}
```

---

## Test Suite

**10 test methods, 13 test cases, across 3 test projects.**

| Project | Type | Techniques |
|---|---|---|
| `RoomBookingApp.Core.Test` | Unit tests | xUnit `[Fact]` and `[Theory]`, Moq mocks with callbacks and `Verify`, Shouldly assertions |
| `RoomBookingApp.API.Tests` | Controller tests | `[Theory]` with `[InlineData]` covering valid and invalid model states, verifying the correct `IActionResult` type and call count |
| `RoomBookingApp.Persistence.Test` | Data-access tests | EF Core InMemory provider to test real queries and saves without a database server |

**Testing approach**

- **Unit tests** mock the data service so the booking rules are tested in isolation, with no database involved.
- **Theories** cover several scenarios (success and failure, valid and invalid) with a single test body.
- **Data-access tests** run real EF Core queries, because mocks cannot catch query or mapping mistakes.

Run everything:

```bash
dotnet test RoomBookingApp.slnx
```

---

## API Reference

### `POST /api/RoomBooking`

**Request**

```json
{
  "fullName": "Jane Doe",
  "email": "jane.doe@example.com",
  "date": "2026-12-01T00:00:00"
}
```

**Success: `200 OK`** (`flag: 0` means `Success`)

```json
{
  "flag": 0,
  "roomBookingId": 1,
  "fullName": "Jane Doe",
  "email": "jane.doe@example.com",
  "date": "2026-12-01T00:00:00"
}
```

**No room available: `400 Bad Request`**

```json
{
  "Date": ["No Rooms Available For Given Date"]
}
```

**Validation rules**

| Field | Rule |
|---|---|
| `fullName` | Required, up to 80 characters |
| `email` | Required, valid email format, up to 80 characters |
| `date` | Must be in the future |

The app seeds three rooms (Conference Room A, B, and C), so the **fourth** booking for the same date returns `400`.

---

## Getting Started

**Prerequisites:** [.NET 9 SDK](https://dotnet.microsoft.com/download)

```bash
# 1. Clone the repository
git clone https://github.com/<your-username>/<your-repo-name>.git
cd <your-repo-name>

# 2. Run the tests
dotnet test RoomBookingApp.slnx

# 3. Start the API
dotnet run --project RoomBookingApp.API
```

Then open **http://localhost:5103/swagger** and try the `POST /api/RoomBooking` endpoint.

> The API uses an in-memory SQLite database, so data resets every time the app restarts. This keeps the project easy to run with zero setup.

---

## Tech Stack

| Area | Tools |
|---|---|
| Framework | .NET 9, ASP.NET Core Web API |
| Data | Entity Framework Core 9, SQLite (runtime), EF Core InMemory (tests) |
| Testing | xUnit, Moq, Shouldly |
| Docs | Swagger / Swashbuckle |

---

## Project Structure

```
RoomBookingApp
├── RoomBookingApp.API                # Controllers, Program.cs, Swagger
├── RoomBookingApp.API.Tests          # Controller tests
├── RoomBookingApp.Core               # Business logic and service contracts
│   ├── DataServices
│   ├── Enums
│   ├── Models
│   └── Processors
├── RoomBookingApp.Core.Test          # Unit tests for the booking logic
├── RoomBookingApp.Domain             # Entities and validation base model
├── RoomBookingApp.Persistence        # EF Core context and data service
├── RoomBookingApp.Persistence.Test   # Data-access tests
└── RoomBookingApp.slnx
```

---

## Roadmap

- [ ] GitHub Actions pipeline that runs the tests on every push and pull request
- [ ] Branch protection so failing tests block merges
- [ ] End-to-end API tests with `WebApplicationFactory`
- [ ] Async data access
- [ ] Persistent database (SQL Server) with migrations
- [ ] Code coverage report

---

## About

Built by **[Belal Saied](www.linkedin.com/in/belal-saied)** while moving from software engineer toward product engineer: owning a feature from requirement through to tested, reliable delivery.

Course: *Test Driven Development (TDD) in .NET* by Trevoir Williams on Udemy.

If this project was useful, a star on the repo is appreciated.
