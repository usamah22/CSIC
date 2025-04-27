# Computer Science Industry Club (CSIC) Backend

## Overview

This repository contains the backend API for the Computer Science Industry Club (CSIC) web application, designed to connect Aston University students, academic staff, and industry professionals. The backend is built using ASP.NET Core with a clean architecture approach, providing robust API endpoints to support the CSIC platform's event management, booking, feedback, and communication features.

## Architecture

The backend follows Clean Architecture principles with a domain-driven design approach, consisting of four main layers:

- **Domain Layer**: Business entities, domain events, rules, and repository interfaces
- **Application Layer**: Application business logic, commands, queries, and DTOs
- **Infrastructure Layer**: Persistence, identity, and external services
- **API Layer**: RESTful endpoints and HTTP request handling

## Features

- **Authentication & Authorization**: Token-based authentication with JWT (JSON Web Tokens) and role-based access control
- **Event Management**: Create, update, retrieve, and cancel events
- **Booking System**: Process event bookings with validation rules
- **Feedback Collection**: Manage and retrieve user feedback for events
- **Contact System**: Handle and process contact messages from users
- **Notification Service**: Send notifications to users
- **Job Postings**: Manage industry partner job postings
- **Error Handling**: Centralised exception handling with logging

## Technology Stack

- **Framework**: ASP.NET Core 8.0
- **ORM**: Entity Framework Core
- **Database**: PostgreSQL
- **Authentication**: JWT (JSON Web Tokens)
- **Validation**: FluentValidation
- **Logging**: Built-in ASP.NET Core logging to file system

## Project Structure

```
CSIC/
├── API/                    # API Layer
│   ├── Controllers/        # API Controllers
│   ├── Middleware/         # Custom Middleware
│   ├── Settings/           # Configuration Settings
├── Application/            # Application Layer
│   ├── Behaviours/         # Cross-cutting behaviours
│   ├── Common/             # Interfaces and mappings
│   ├── DTOs/               # Data Transfer Objects
│   └── Features/           # CQRS Features
├── Domain/                 # Domain Layer
│   ├── Common/             # Base entities and abstractions
│   ├── Events/             # Domain events
│   ├── Exceptions/         # Domain-specific exceptions
│   ├── Repositories/       # Repository interfaces
│   └── ValueObjects/       # Value objects
└── Infrastructure/         # Infrastructure Layer
    ├── Identity/           # Identity implementation
    ├── Migrations/         # EF Core migrations
    ├── Persistence/        # Database context and repositories
    └── Services/           # External service integrations
```

## Getting Started

### Prerequisites

- .NET SDK 8.0 or later
- PostgreSQL server
- IDE (Visual Studio, VS Code, or JetBrains Rider)

### Installation

Clone the repository:

```bash
git clone https://github.com/usamah22/CSIC.git
cd CSIC
```

### Configuration

Update your database connection string and JWT settings in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=CSIC;Username=postgres;Password=yourpassword"
  },
  "JwtSettings": {
    "Secret": "your-secret-key-here",
    "TokenLifetime": "01:00:00"
  }
}
```

### Database Setup

Apply the database migrations:

```bash
cd API
dotnet ef database update
```

### Running the API

```bash
cd API
dotnet run
```

The API will be available at:

- https://localhost:5001
- http://localhost:5000

## API Endpoints

- **Auth**: User registration, login, and token generation
- **Events**: CRUD operations
- **Bookings**: Manage event bookings
- **Feedback**: Submit and retrieve feedback
- **Contact Messages**: Handle user queries
- **Users**: Admin-only user management
- **Job Postings**: Industry job management
- **Notifications**: Send and manage notifications

## Key Features Implementation

### CQRS Pattern

- **Commands**: Mutating operations (create, update, delete)
- **Queries**: Read-only operations (retrieve)
- **Handlers**: Process Commands and Queries via MediatR

### Domain-Driven Design

- **Entities** and **Value Objects** model business logic
- **Domain Events** capture significant state changes
- **Validation Rules** ensure consistency

### Error Handling

Centralised middleware captures errors and returns consistent responses.

## Contributing

This project was developed for academic purposes and is currently not accepting external contributions.

## License

This project was developed for academic purposes at Aston University.

## Acknowledgments

- Aston University Computer Science Department
- Contributing industry partners
- Faculty advisors and project supervisors

 
