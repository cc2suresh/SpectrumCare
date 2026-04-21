using FluentAssertions;
using SpectrumCare.BuildingBlocks.Domain.Primitives;
using Xunit;

namespace SpectrumCare.BuildingBlocks.UnitTests.Primitives;

/// <summary>
/// Unit tests for AggregateRoot versioning behavior.
/// </summary>
public class AggregateRootTests
{
    private sealed class TestAggregate : AggregateRoot
    {
        public TestAggregate(Guid id) : base(id) { }
    }

    [Fact]
    public void AggregateRoot_ShouldHaveZeroVersion_WhenCreated()
    {
        var aggregate = new TestAggregate(Guid.NewGuid());

        aggregate.Version.Should().Be(0);
    }

    [Fact]
    public void IncrementVersion_ShouldIncreaseVersionByOne()
    {
        var aggregate = new TestAggregate(Guid.NewGuid());

        aggregate.IncrementVersion();

        aggregate.Version.Should().Be(1);
    }

    [Fact]
    public void IncrementVersion_MultipleTimes_ShouldIncreaseVersionAccordingly()
    {
        var aggregate = new TestAggregate(Guid.NewGuid());

        aggregate.IncrementVersion();
        aggregate.IncrementVersion();
        aggregate.IncrementVersion();

        aggregate.Version.Should().Be(3);
    }
}