# ExchangeSystem – Microservice Architecture

ExchangeSystem is a backend microservice developed as part of a backend developer assessment.
The goal is to demonstrate a clean, modular architecture and proper implementation of microservices principles such as decoupled components, asynchronous messaging, and observability.

The system provides:

- An API to execute and retrieve trades (using Swagger to test)

- Persistence of trades in a MySQL database

- Asynchronous messaging using RabbitMQ

- A consumer service that listens to trade events and logs them

- Centralized logging with Serilog

- A correlation ID system to trace logs across distributed services

## Architecture

This project follows the Onion Architecture pattern to ensure a clean separation of concerns and maintainable code.

## Layers

- Domain Layer (ExchangeSystem.Domain)
  - Contains core business models and domain logic.

- Application Layer (ExchangeSystem.Application)
  - Contains service interfaces, application logic, and DTOs.

- Infrastructure Layer (ExchangeSystem.Infrastructure)
  - Implements database access using Entity Framework Core and handles external integrations such as RabbitMQ.

- API Layer (ExchangeSystem.Host.Api)
  - Exposes the RESTful endpoint for trade execution and retrieval.

- Test Layer (ExchangeSystem.UnitTests)

## Technologies Used
- C# (.NET 8)
- RabbitMQ
- MySQL
- Serilog
- Docker
- NUnit

## How To Run:

From the root folder: MeDirectAssessment

Run `docker compose up --build`

- API available at http://localhost:8080/swagger/index.html
- RMQ management console: http://localhost:15672/#/
- Consumer Application outputs logs to the console running the docker compose
