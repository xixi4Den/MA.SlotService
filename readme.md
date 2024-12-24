# Slot service

A service responsible for the slot machine logic. It allows players to spin as long as their spin balance is greater
than zero. Each spin generates a random result consisting of three digits, each between 0 and 9.

---

## Technologies Used

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis)
- [MassTransit](https://masstransit.io/)
- [MediatR](https://github.com/jbogard/MediatR)
- [TestContainers](https://dotnet.testcontainers.org/)
- [OpenTelemetry](https://opentelemetry.io/)

---

## Getting Started

### Prerequisites

[.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Installation

1. Clone the repository
2. Navigate to the solution directory
3. Run `dotnet restore` to restore dependencies
4. Run `dotnet build` to build the solution

### Usage

1. Set up the required env variables

> Alternatively, you can add `appsettings.Development.json` file in MA.SlotService.Host project for local development

2. Run `dotnet run --project MA.SlotService.Host` to start the service
3. Access the service:
    - API: https://localhost:7248/api
    - Swagger UI: https://localhost:7248/swagger/index.html
    - Health check:  https://localhost:7248/health

---

## Configuration

| Parameter Name          | Description                                         | Mandatory | Example Value                      |
|-------------------------|-----------------------------------------------------|-----------|------------------------------------|
| Redis__ConnectionString | Redis connection string                             | &#9745;   | localhost                          |
| Rabbit__Host            | RabbitMQ host                                       | &#9745;   | localhost                          |
| Rabbit__Username        | RabbitMQ username                                   | &#9745;   | guest                              |
| Rabbit__Password        | RabbitMQ password                                   | &#9745;   | guest                              |
| EXPORTER_OTLP_ENDPOINT  | OTLP endpoint. If not set, Console exporter is used | &#9744;   | http://127.0.0.1:9411/api/v2/spans |

---

## Tests

This project includes both unit tests and integration tests.

Run the following command to run all tests

```
dotnet test
```

### Integration tests

Integration tests will run Redis instance in a container using [TestContainers](https://dotnet.testcontainers.org/), so ensure Docker is installed and running on your system.

## Observability

The service leverages OpenTelemetry to expose both traces and metrics.

### Traces

The service uses the OTLP exporter for traces if `EXPORTER_OTLP_ENDPOINT` env variable is set; otherwise, it defaults to the Console exporter.

### Metrics

Metrics are exposed using the Prometheus exporter. Scraping endpoint: https://localhost:7248/metrics


