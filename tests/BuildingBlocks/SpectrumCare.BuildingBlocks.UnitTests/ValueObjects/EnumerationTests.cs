using FluentAssertions;
using SpectrumCare.BuildingBlocks.Domain.ValueObjects;
using Xunit;

namespace SpectrumCare.BuildingBlocks.UnitTests.ValueObjects;

/// <summary>
/// Unit tests for Enumeration base class behavior.
/// </summary>
public class EnumerationTests
{
    private sealed class TestEnumeration : Enumeration
    {
        public static readonly TestEnumeration First = new(1, "First");
        public static readonly TestEnumeration Second = new(2, "Second");
        public static readonly TestEnumeration Third = new(3, "Third");

        private TestEnumeration(int id, string name) : base(id, name) { }
    }

    [Fact]
    public void GetAll_ShouldReturnAllEnumerationValues()
    {
        var all = Enumeration.GetAll<TestEnumeration>();

        all.Should().HaveCount(3);
    }

    [Fact]
    public void ToString_ShouldReturnName()
    {
        TestEnumeration.First.ToString().Should().Be("First");
    }

    [Fact]
    public void Equals_SameId_ShouldBeEqual()
    {
        TestEnumeration.First.Equals(TestEnumeration.First).Should().BeTrue();
    }

    [Fact]
    public void Equals_DifferentId_ShouldNotBeEqual()
    {
        TestEnumeration.First.Equals(TestEnumeration.Second).Should().BeFalse();
    }

    [Fact]
    public void CompareTo_ShouldReturnCorrectOrder()
    {
        var result = TestEnumeration.First.CompareTo(TestEnumeration.Second);

        result.Should().BeNegative();
    }
}