using FluentAssertions;
using SpectrumCare.BuildingBlocks.Domain.Events;
using SpectrumCare.BuildingBlocks.Domain.Primitives;
using Xunit;

namespace SpectrumCare.BuildingBlocks.UnitTests.Primitives;

/// <summary>
/// Unit tests for BaseEntity domain event handling and audit fields.
/// </summary>
public class BaseEntityTests
{
    private sealed class TestEntity : BaseEntity
    {
        public TestEntity(Guid id) : base(id) { }
        public TestEntity() { }
    }

    private sealed class TestDomainEvent : DomainEvent
    {
        public TestDomainEvent() { }
    }

    [Fact]
    public void BaseEntity_ShouldHaveId_WhenCreatedWithId()
    {
        var id = Guid.NewGuid();
        var entity = new TestEntity(id);

        entity.Id.Should().Be(id);
    }

    [Fact]
    public void RaiseDomainEvent_ShouldAddEventToDomainEvents()
    {
        var entity = new TestEntity(Guid.NewGuid());
        var domainEvent = new TestDomainEvent();

        entity.RaiseDomainEvent(domainEvent);

        entity.DomainEvents.Should().HaveCount(1);
        entity.DomainEvents.First().Should().Be(domainEvent);
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllEvents()
    {
        var entity = new TestEntity(Guid.NewGuid());
        entity.RaiseDomainEvent(new TestDomainEvent());
        entity.RaiseDomainEvent(new TestDomainEvent());

        entity.ClearDomainEvents();

        entity.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void SetCreatedBy_ShouldSetCreatedByField()
    {
        var entity = new TestEntity(Guid.NewGuid());

        entity.SetCreatedBy("test-user");

        entity.CreatedBy.Should().Be("test-user");
    }

    [Fact]
    public void SetUpdatedBy_ShouldSetUpdatedByAndUpdatedAt()
    {
        var entity = new TestEntity(Guid.NewGuid());

        entity.SetUpdatedBy("test-user");

        entity.UpdatedBy.Should().Be("test-user");
        entity.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void BaseEntity_ShouldHaveCreatedAt_WhenCreated()
    {
        var before = DateTime.UtcNow;
        var entity = new TestEntity(Guid.NewGuid());
        var after = DateTime.UtcNow;

        entity.CreatedAt.Should().BeOnOrAfter(before);
        entity.CreatedAt.Should().BeOnOrBefore(after);
    }
}