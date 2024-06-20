# Core Project Template

## Table Of Content

- [Getting Started](#getting-started)
- [Overview](#overview)
- [Packages](#packages)
  - [MediatR](#mediatr)
  - [Fluent Validation](#fluentvalidation)
  - [Fluent Assertion](#fluentassertions)
  - [Mapster](#mapster)
  - [ErrorOr](#erroror)
- [Folder Structure](#folder-structure)
- [Features](#features)
  - [Application Layer](#application-layer)
  - [Infrastructure Layer](#infrastructure-layer)
  - [Presentation Layer](#presentation-layer)
  - [Additional Service](#additional-services)
  - [Notes](#notes)
- [Cross Cutting Concerns](#cross-cutting-concerns)
  - [Logging](#Logging)
  - [Caching](#caching)
    - [Caching Service](#caching-service)
    - [Generic Cached Repository](#generic-cached-repository)
  - [Api Versioning](#api-versioning)
  - [Health Checks](#health-checks)

## Getting Started

- Install the template using

```sh
 dotnet new --install .
```

- Create a new project with the template

```sh
dotnet new core-template -o ProjectName
```

## Overview

This repository implements a modern clean architecture following domain-driven design principles and employing vertical slice architecture in each layer. The architecture encompasses the following key components:

- **Clean Architecture**: The project is structured according to clean architecture principles.
- **Domain-Driven Design (DDD)**: DDD principles guide the design and organization of domain logic.
- **Vertical Slice Architecture**: Each layer of the clean architecture employs vertical slice architecture for better organization and separation of concerns.

## Packages

### MediatR

[MediatR](https://github.com/jbogard/MediatR) is used for implementing the mediator pattern, allowing for loosely coupled communication between components.

### FluentValidation

[FluentValidation](https://fluentvalidation.net/) is used for building strongly-typed validation rules for commands and queries.

### FluentAssertions

[FluentAssertions](https://fluentassertions.com/) is used for writing more readable and maintainable unit tests with a fluent syntax.

### Mapster

[Mapster](https://github.com/MapsterMapper/Mapster) is used for object mapping, transforming objects between different layers of the application.

### ErrorOr

[ErrorOr](https://github.com/amantinband/error-or) is used for handling errors and results consistently throughout the application using the model result pattern.

## Folder Structure

```txt
domain/
└── common/
    └── errors/
        └── errors.user.cs
└── user_aggregate/
    ├── value_objects/
    └── enums/

infrastructure/
└── common/
└── authentication/
    ├── commands/
    │   ├── register/
    │   │   ├── register.command.cs
    │   │   ├── register.handler.cs
    │   │   └── register.validator.cs
    ├── queries/
    │   └── login/
    │       └── login.query.cs
    └── specifications/

presentation/
└── authentication/
    ├── DTOs/
    │   ├── requests/
    │   └── responses/
    ├── mappings/
    └── controllers/
```

## Features

### Application Layer

- **CQRS with Mediator Pattern**: Commands and queries are separated using the CQRS pattern and mediated through a mediator pattern.
- **Cloned Validation**: Validation is performed using the cloned validation package.
- **Unit of Work and Generic Repository**: Implements a unit of work with a generic repository for data access.
- **Specification Pattern**: Specifications are used to encapsulate query logic.

### Infrastructure Layer

- **Infrastructure and Infrastructure.Persistence**: Infrastructure handles application services not related to the database, while Infrastructure.Persistence focuses on database logic.
- **Options Pattern for Configuration**: Configuration objects are managed using the options pattern.
- **Supported Database Providers**: PostgreSQL and SQL Server are supported database providers.
- **DatabaseConfigurations**: Database configurations can be specified in the app.settings.json file.

### Presentation Layer

- **Feature-Based Structure**: The presentation layer is organized into feature folders.
- **Controller and Mapping**: Each feature folder contains controllers and a mapping folder utilizing the Mapster library for DTO mapping.
- **DTOs for Requests and Responses**: DTOs are provided for requests and responses specific to each feature.

### Additional Services

- **JWT Token Generator Service**: Generates JWT tokens and assigns claims.
- **IUserContext Service**: Retrieves user context from the HttpContextAccessor object.

### Notes

- **Presentation.API Layer**: Serves as the web application responsible for assembling all other layers without maintaining business logic.

## Cross Cutting Concerns

### Logging

The Template supports Logging with Serilog with different sinks all configured using appsettings.json file

- **Console**
- **File**
- **Seq**
- **Elastic Search**


### Caching

#### Caching Service

The Template Contains ICachingService that relies behind the scene on IDistributed Cache Interface

Examples:

```csharp

public interface ICachingService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        where T : class;
    
    Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T?>> factory, CancellationToken cancellationToken = default)
        where T : class;
    
    Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        where T : class;
    
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    Task RemoveByPrefixAsync(string keyPrefix, CancellationToken cancellationToken = default);

    Task<ConcurrentDictionary<string, bool>> GetCacheKeys(CancellationToken cancellationToken = default);
}

// the factory method will be executed if the key doesn't exists 
// in he cache and after that it will store it in the cache
 await cachingService.GetOrCreateAsync(
            key: "products", 
            factory: async () => await GetProductsFromDatabase(), 
            cancellationToken);
```

### Generic Cached Repository

Supported an interface ICachedGenericRepository that handles caching behind the scenes for all the repository methods

```csharp
public interface ICachedGenericRepository<TEntity, TEntityId> : IGenericRepository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : notnull;
```

### Api Versioning

### Health Checks

