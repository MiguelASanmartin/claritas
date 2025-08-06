# Task Management System

Enterprise-grade Task Management System built with Clean Architecture, CQRS, and .NET 8. Features cross-platform development with Docker containerization and comprehensive API documentation.

## Architecture
┌─────────────────┐    ┌──────────────────┐    ┌─────────────────┐    ┌──────────────────┐
│   Presentation  │───▶│   Application    │───▶│  Infrastructure │───▶│     Domain       │
│                 │    │                  │    │                 │    │                  │
│ • Controllers   │    │ • Commands       │    │ • EF Core       │    │ • Entities       │
│ • Swagger UI    │    │ • Queries        │    │ • Repositories  │    │ • Value Objects  │
│ • Middleware    │    │ • Handlers       │    │ • External APIs │    │ • Interfaces     │
│ • DTOs          │    │ • Validators     │    │ • Database      │    │ • Domain Events  │
└─────────────────┘    └──────────────────┘    └─────────────────┘    └──────────────────┘

### Project Structure
TaskManagementSystem/
├── TMS.Domain/             # Core business logic and entities
│   ├── Entities/           # User, Project, Task
│   ├── ValueObjects/       # TaskStatus, TaskPriority
│   ├── Interfaces/         # Repository contracts
│   └── Events/             # Domain events
├── TMS.Application/        # Business use cases
│   ├── Commands/           # Write operations (CQRS)
│   ├── Queries/            # Read operations (CQRS)
│   ├── Handlers/           # MediatR request handlers
│   ├── DTOs/               # Data transfer objects
│   ├── Extensions/         # Manual mapping extensions
│   └── Validators/         # FluentValidation rules
├── TMS.Infrastructure/     # External integrations
│   ├── Data/               # EF Core DbContext & configurations
│   ├── Repositories/       # Repository implementations
│   └── Services/           # External service integrations
├── TMS.Web.API/           # REST API endpoints
├── TMS.Web.MVC/           # Web UI (future implementation)
└── TMS.Tests/             # Unit & integration tests

## Quick Start

### Windows Development (LocalDB)

#### Prerequisites: Visual Studio, .NET 8 SDK, LocalDB

#### Clone repository
##### git clone https://github.com/MiguelASanmartin/claritas
##### cd TaskManagementSystem

#### Setup database and run
##### setup-windows.bat
##### dotnet run --project TMS.Web.API

##### Access application
###### API: https://localhost:7000
###### Swagger: https://localhost:7000/swagger

### Linux Development (Docker)

#### Prerequisites: Docker, .NET 8 SDK

#### Clone repository  
##### git clone https://github.com/MiguelASanmartin/claritas
##### cd TaskManagementSystem

#### Create environment file
##### cat > .env << EOF
##### SA_PASSWORD=YourPassword
##### ENVIRONMENT=Development
##### EOF

#### Setup and run
##### chmod +x setup-linux.sh
##### ./setup-linux.sh
##### dotnet run --project TMS.Web.API

##### Access application
###### API: http://localhost:5000
###### Swagger: http://localhost:5000/swagger
###### DB Admin: http://localhost:8080

## Commit Convention

### feat: - New features
### fix: - Bug fixes
### docs: - Documentation updates
### refactor: - Code refactoring
### test: - Test additions/updates
### chore: - Maintenance tasks

## Learning Resources
### Architecture & Patterns

#### Clean Architecture by Robert Martin
#### .NET Application Architecture Guides
#### Domain-Driven Design Reference

### Technical Documentation

#### Entity Framework Core Documentation
#### MediatR Documentation
#### Docker Documentation
#### ASP.NET Core Documentation

## License
### This project is licensed under the MIT License - see the LICENSE file for details.
### Author
#### Miguel Ángel Sanmartin Díaz

### GitHub: @MiguelASanmartin
### LinkedIn: Miguel Sanmartín
### Email: miguelsd151@gmail.com


## ⭐ Star this repository if you found it helpful!
### Built using .NET 8 and Clean Architecture principles.
### *Documentation enhanced with AI assistance*