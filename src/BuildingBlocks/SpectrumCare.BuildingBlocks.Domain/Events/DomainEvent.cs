namespace SpectrumCare.BuildingBlocks.Domain.Events;

public abstract class DomainEvent : IDomainEvent
{
    protected DomainEvent()
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
        EventType = GetType().Name;
    }

    public Guid EventId { get; }

    public DateTime OccurredOn { get; }

    public string EventType { get; }
}