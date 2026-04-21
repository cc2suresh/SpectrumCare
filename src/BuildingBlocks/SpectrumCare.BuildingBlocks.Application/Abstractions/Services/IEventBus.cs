using SpectrumCare.BuildingBlocks.Domain.Events;

namespace SpectrumCare.BuildingBlocks.Application.Abstractions.Services;

/// <summary>
/// Abstraction for publishing domain events across services.
/// Today implemented using MediatR in-process publishing.
/// Future: swap implementation to Azure Service Bus without any code changes.
/// All cross-service communication must go through this interface.
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Publishes a domain event to all registered subscribers.
    /// Fire-and-forget for async events. Awaitable for in-process events.
    /// </summary>
    /// <typeparam name="TEvent">The domain event type to publish.</typeparam>
    /// <param name="domainEvent">The event instance to publish.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task PublishAsync<TEvent>(
        TEvent domainEvent,
        CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent;
}