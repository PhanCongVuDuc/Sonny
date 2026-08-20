# Serilog Logging Setup

> 8 nodes · cohesion 0.29

## Key Concepts

- **LoggerConfiguration** (4 connections) — `source/Sonny.Application/Config/Logging/LoggerConfiguration.cs`
- **.AddSerilogConfiguration()** (3 connections) — `source/Sonny.Application/Config/Logging/LoggerConfiguration.cs`
- **.CreateDefaultLogger()** (3 connections) — `source/Sonny.Application/Config/Logging/LoggerConfiguration.cs`
- **Sonny.Application.Config.Logging** (2 connections) — `source/Sonny.Application/Config/Logging/LoggerConfiguration.cs`
- **LoggerConfiguration.cs** (2 connections) — `source/Sonny.Application/Config/Logging/LoggerConfiguration.cs`
- **Logger** (1 connections)
- **IServiceCollection** (1 connections)
- **string** (1 connections)

## Relationships

- No strong cross-community connections detected

## Source Files

- `source/Sonny.Application/Config/Logging/LoggerConfiguration.cs`

## Audit Trail

- EXTRACTED: 8 (100%)
- INFERRED: 0 (0%)
- AMBIGUOUS: 0 (0%)

---

*Part of the graphify knowledge wiki. See [index](index.md) to navigate.*