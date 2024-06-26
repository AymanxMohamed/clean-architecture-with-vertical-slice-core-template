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
    - [Messaging](#messaging)
        - [Domain Events](#domain-events)
        - [Integration Events](#integration-events)
    - [Hangfire Background Jobs](#hangfire-background-jobs)
      - [Creating The Background Jobs](#creating-the-background-jobs)
      - [Running the Background Jobs](#running-the-background-jobs)

## Getting Started

- Install the template using

```sh
 dotnet new install .
```

- Create a new project with the template

```sh
dotnet new core-template -o Petsouky
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
- **Fluent Validation**: Validation is performed using the Fluent validation package.
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

### Messaging

#### Domain Events

In Order to use messages you have to follow these steps

- **Define Domain Event**: Declare the domain event you want to publish Domain Events is used within
  the same bounded context (Microservice) only and is
  declared inside the Events folder for the specific aggregate that the event will happen inside

```csharp
namespace Core.Domain.Aggregates.UserAggregate.Events;

public record UserCreatedDomainEvent(UserId UserId) : IDomainEvent;
```
- **Raise The Domain Event within the aggregate**: Rasie the event using AddDomainEvent method

```csharp
    private User(
        string firstName,
        string lastName,
        string email,
        string passwordHash,
        UserId? id = null)
        : base(id ?? UserId.CreateUnique())
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        _passwordHash = passwordHash;
        
        AddDomainEvent(new UserCreatedDomainEvent(Id));
    }
```

- **Handling the Domain Event**: Inside your ApplicationLayer.FeatureFolder create DomainEvents folder and Implement IDomainEventHandler

```csharp
namespace Core.Application.Test.DomainEvents;

public class UserCreatedDomainEventHandler : IDomainEventHandler<UserCreatedDomainEvent>
{
    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine("Domain Event Handler");
        
        await Task.CompletedTask;
    }
}
```

#### Integration Events

- **Define the Message**: Declare the message in the shared Kernel as IIntegrationEvent
  object in a folder with the name of the Application that will publish the message

```csharp
namespace SharedKernel.IntegrationEvents.UserManagement;

public record UserCreatedIntegrationEvent(Guid UserId) : IIntegrationEvent;
```

- **Add Json Derived Type**: Add the json Derived Type attribute to the IIntegration Event to help with the serialization

```csharp
[JsonDerivedType(typeof(UserCreatedIntegrationEvent), typeDiscriminator: nameof(UserCreatedIntegrationEvent))]
public interface IIntegrationEvent : INotification;
```

- **Create New Integration Event Writer**: Add new Integration Event Writer Class inside the integration events folder
  inside the Infrastructure.Integrations Assembly you will have to only implement the GenerateIntegrationEvent method
  that is responsible for converting the Domain event to integration event

```csharp
namespace Core.Infrastructure.Integrations.IntegrationEvents;

public class UserCreatedDomainIntegrationEventOutboxOutboxWriter(IOutboxWriter outboxWriter) 
    : IntegrationEventOutboxWriter<UserCreatedDomainEvent, UserCreatedIntegrationEvent>(outboxWriter)
{
    protected override UserCreatedIntegrationEvent GenerateIntegrationEvent(UserCreatedDomainEvent domainEvent)
    {
        return new UserCreatedIntegrationEvent(domainEvent.UserId.Value);
    }
}
```

- **Handle The Integration Event**: Inside your ApplicationLayer.FeatureFolder create IntegrationEvents folder and Implement IIntegrationEventHandler

```csharp
namespace Core.Application.Test.IntegrationEvents;

public class UserCreatedIntegrationEventHandler(ILogger<UserCreatedIntegrationEventHandler> logger) 
    : IIntegrationEventHandler<UserCreatedIntegrationEvent>
{
    public async Task Handle(UserCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            message: "new user has been created and has been processed by the event handler {UserId}",
            notification.UserId);
        
        await Task.CompletedTask;
    }
}
```


### Hangfire Background Jobs

Application Supports Background Jobs using Hangfire and following the steps to register new background job

#### Creating the Background Jobs


- **Fire and forget Job**: inside the Infrastructure.Integrations.HangfireBackgroundJobs namespace 
define new object that inherits from the FireAndForgetJobBase class and implement the Execute Async Method

```csharp
namespace ProjectName.Infrastructure.Integrations.HangfireBackgroundJobs;

public class ConsumeIntegrationEventsFireAndForGetJob : FireAndForgetJobBase
{
    public override Task ExecuteAsync()
    {
        return Task.CompletedTask;
    }
}
```

- **Recurring Job**: inside the Infrastructure.Integrations.HangfireBackgroundJobs namespace
define new object that inherits from RecurringFireAndForgetJobBase base class and implement the Execute Async method
- you have to also implement the GetJobId() method
- you can override the default CronExpression by overloading the GetCronExpression method
- you can also override the default queue by override the GetQueueName method but you will have to add the queue 
in the HangfireConstants.Queues.AllQueues array


```csharp
namespace ProjectName.Infrastructure.Integrations.HangfireBackgroundJobs;

public class PublishIntegrationEventsRecurringJob(ICronExpressionGenerator cronExpressionGenerator) 
    : RecurringFireAndForgetJobBase(cronExpressionGenerator)
{
    public override string GetQueueName() => HangfireConstants.Queues.PublishingIntegrationEventsQueue;
    
    public override string GetJobId() => JobId;
    
    public override string GetCronExpression() => _cronExpressionGenerator.SecondsInterval(5);

    public override Task ExecuteAsync()
    {
        return Task.CompletedTask;
    }
}
```

#### Running the background Jobs

Inside the ProjectName.Infrastructure.Integrations.HangfireBackgroundJobs namespace 

you will have to execute the Run method on the background job in the BackgroundJobsRegistration class

```csharp
    public static IApplicationBuilder AddHangfireBackgroundJobs(this IApplicationBuilder app)
    {
        var serviceProvider = app.ApplicationServices;
        serviceProvider.GetRequiredService<PublishIntegrationEventsRecurringJob>().Run();
        serviceProvider.GetRequiredService<ConsumeIntegrationEventsFireAndForGetJob>().Run();
        // add you new jobs her
        return app;
    }
```



