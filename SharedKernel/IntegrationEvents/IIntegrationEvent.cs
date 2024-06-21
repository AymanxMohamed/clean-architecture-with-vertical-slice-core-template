using System.Text.Json.Serialization;

using MediatR;

using SharedKernel.IntegrationEvents.UserManagement;

namespace SharedKernel.IntegrationEvents;

[JsonDerivedType(typeof(UserCreatedIntegrationEvent), typeDiscriminator: nameof(UserCreatedIntegrationEvent))]
public interface IIntegrationEvent : INotification;