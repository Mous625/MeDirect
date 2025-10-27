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

The API contains 4 endpoints: 
- /Trades/{tradeId} [GET]
- /Trades/client/{clientId} [GET]
- /Trades [POST]
- /Health [GET] - for docker

The database isn't automatically seeded with data. Please execute a trade first using the POST endpoint,<br/> 
then the other endpoints can be used by using the response (TradeId) or the ClientId used in the request. <br />


Sample Correlation ID:
`08de1591-9bec-4b27-84fc-9d53f65dbe67`

Sample JSON paylod for /Trades endpoint (POST) (endpoint to execute a trade):
`{
  "clientId": "123abc",
  "symbol": "ABC",
  "quantity": 1,
  "price": 10
}`

## How To Run:

From the root folder:

Run `docker compose up --build`

- API available at http://localhost:8080/swagger/index.html
- RMQ management console: http://localhost:15672/#/
- Consumer Application outputs logs to the console running the docker compose

The shell will output `exchange-consumer  | Consumer: Waiting for logs.` when everything is ready.

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