# Slot service

A service responsible for the slot machine logic. It allows players to spin as long as their spin balance is greater than zero. Each spin generates a random result consisting of three digits, each between 0 and 9.

---

## Technologies Used

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis)
- [MassTransit](https://masstransit.io/)

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

| Parameter Name          | Description                        | Mandatory | Example Value         |
|-------------------------|------------------------------------|-----------|-----------------------|
| Redis__ConnectionString | Redis connection string            | &#9745;   | localhost             |
| Rabbit__Host            | RabbitMQ host                      | &#9745;   | localhost             |
| Rabbit__Username        | RabbitMQ username                  | &#9745;   | guest                 |
| Rabbit__Password        | RabbitMQ password                  | &#9745;   | guest                 |

---

## Tests

### Unit tests

```
dotnet test
```

---