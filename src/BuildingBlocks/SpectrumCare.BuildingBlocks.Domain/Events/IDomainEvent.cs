using MediatR;

namespace SpectrumCare.BuildingBlocks.Domain.Events;

public interface IDomainEvent : INotification
{
    Guid EventId { get; }

    DateTime OccurredOn { get; }

    string EventType { get; }
}