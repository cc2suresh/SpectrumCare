using FluentAssertions;
using SpectrumCare.BuildingBlocks.Domain.ValueObjects;
using Xunit;

namespace SpectrumCare.BuildingBlocks.UnitTests.ValueObjects;

/// <summary>
/// Unit tests for ValueObject equality and comparison behavior.
/// </summary>
public class ValueObjectTests
{
    private sealed class TestValueObject : ValueObject
    {
        public TestValueObject(string value1, int value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public string Value1 { get; }
        public int Value2 { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value1;
            yield return Value2;
        }
    }

    [Fact]
    public void TwoValueObjects_WithSameValues_ShouldBeEqual()
    {
        var vo1 = new TestValueObject("test", 1);
        var vo2 = new TestValueObject("test", 1);

        vo1.Equals(vo2).Should().BeTrue();
        (vo1 == vo2).Should().BeTrue();
    }

    [Fact]
    public void TwoValueObjects_WithDifferentValues_ShouldNotBeEqual()
    {
        var vo1 = new TestValueObject("test", 1);
        var vo2 = new TestValueObject("test", 2);

        vo1.Equals(vo2).Should().BeFalse();
        (vo1 != vo2).Should().BeTrue();
    }

    [Fact]
    public void TwoValueObjects_WithSameValues_ShouldHaveSameHashCode()
    {
        var vo1 = new TestValueObject("test", 1);
        var vo2 = new TestValueObject("test", 1);

        vo1.GetHashCode().Should().Be(vo2.GetHashCode());
    }
}