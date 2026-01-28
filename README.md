# French Revolution API
[![Build](https://github.com/clashmar/FrenchRevolution/actions/workflows/build.yml/badge.svg)](https://github.com/clashmar/FrenchRevolution/actions/workflows/build.yml)
[![Tests](https://github.com/clashmar/FrenchRevolution/actions/workflows/test-and-report.yml/badge.svg)](https://github.com/clashmar/FrenchRevolution/actions/workflows/test-and-report.yml)

### "Liberté, Égalité, Fraternité"

A simple .NET 10 Web API using ASP.NET and modern backend development patterns. 
The French Revolution provides a backdrop to showcase clean, CQRS 
architecture and Domain-Driven Design.

## Key Features

- CQRS pattern using MediatR
- Role based authentication using JWT and Cooakies
- PostgreSQL database and Entity Framework Core
- Redis distributed caching using a cache-aside pattern
- OpenTelemetry (tracing, metrics, logging) via an Aspire dashboard
- Rate limiting and health checks
- Docker and Kubernetes ready
- Comprehensive and fast integration tests using xUnit, Test Containers 
and Respawner
- CI pipeline using GitHub actions for test/coverage reports and updating
public Docker image
- Razor pages admin portal (built with Claude Code CLI)

## API Features

- Problem Details (RFC 7807) for standardized error responses
- Global exception handling middleware
- Cross-cutting logging pipeline 
- URL-based API versioning (v1)
- Soft deletion

## Tech Stack

- .NET 10, ASP.NET Core
- PostgreSQL, Entity Framework Core
- Redis
- MediatR, FluentValidation
- Serilog, OpenTelemetry
- Docker, Kubernetes & Helm

## Join the Revolution

### Prerequisites

- Docker

### Setup

1. Clone the repository
2. Create a `.env` file in the project root with the following variables:

```
DB_HOST=db-service
DB_PORT=5432

# Update these values:
DB_NAME=local_db
DB_USER=your_username
DB_PASSWORD=your_password
ADMIN_EMAIL=admin@example.com
ADMIN_PASSWORD=your_admin_password
JWT_SECRET=your-secret-key-at-least-32-characters
```

3. Run the application:

```bash
cd FrenchRevolution
docker compose up
```

4. Wait for the application to run migrations and seed the database once
db-service is healthy. An admin user will be created with your ADMIN_EMAIL 
and ADMIN_PASSWORD values as credentials, which can also be used to access 
the Admin portal and PostgreSQL admin UI. 
5. The Aspire token needed for logging in to the dashboard can be found in the 
logs of the dashboard container.

### Access Points

- Landing page: http://localhost:5000/
- Scalar UI: http://localhost:5000/scalar
- Admin Portal: http://localhost:5000/Admin
- Aspire Dashboard: http://localhost:18888
- PostgresSQL Admin: http://localhost:8080

## Endpoints

| Resource                | Description                                             |
|-------------------------|---------------------------------------------------------|
| `/api/v1/characters`    | CRUD operations with pagination, filtering, and sorting |
| `/api/v1/factions`      | Faction queries                                         |
| `/api/v1/auth/login`    | User authentication                                     |
| `/api/v1/auth/register` | User registration                                       |
| `/healthz`              | Health check endpoint                                   |

## Deployment

The application includes manifests and a Helm chart for deploying all 
services to a Kubernetes cluser. Copy the contents of `secrets-template.yaml`
into a new `secrets.yaml` file in the the same directory and fill in the values 
with your secrets.


```bash
cd FrenchRevolution
helm install french-revolution ./k8s/french-revolution
```

The latest image is always available from the 
[public Docker repository](https://hub.docker.com/r/clashmarr/frenchrevolution) 
and will be used by `values.yaml`. Everything is currently 
configured for local development but can be extended easily for 
production/staging environments.

## Development Notes
The Razor Pages admin portal was built using Claude Code CLI to demonstrate use
of AI-assisted workflows. 

## Roadmap

This application is a means to practice and showcase the latest 
(.NET 10) backend technology and best practices. The API itself could of 
course be extended with more endpoints (e.g historical events, places etc)
and a modern front-end, but hopefully the application  in its current
state provides a good fundamental understanding of .NET development. 
That being said, these are some patterns and tools I would like to add at
some point, or maybe to a different project:

- Polly
- Minimal APIs
- Dapper ORM 
- HATEOAS links
- Contract testing
- Multi-tenancy architecture
- WebSocket notifications
