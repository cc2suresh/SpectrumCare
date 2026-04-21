using FluentAssertions;
using SpectrumCare.BuildingBlocks.Application.Common;
using Xunit;

namespace SpectrumCare.BuildingBlocks.UnitTests.Common;

/// <summary>
/// Unit tests for PaginatedList pagination behavior.
/// </summary>
public class PaginatedListTests
{
    [Fact]
    public void Create_ShouldReturnCorrectPage()
    {
        var items = Enumerable.Range(1, 20).ToList();

        var result = PaginatedList<int>.Create(items, 2, 5);

        result.Items.Should().HaveCount(5);
        result.Items.First().Should().Be(6);
        result.PageNumber.Should().Be(2);
        result.TotalCount.Should().Be(20);
        result.TotalPages.Should().Be(4);
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public void Create_FirstPage_ShouldNotHavePreviousPage()
    {
        var items = Enumerable.Range(1, 10).ToList();

        var result = PaginatedList<int>.Create(items, 1, 5);

        result.HasPreviousPage.Should().BeFalse();
        result.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public void Create_LastPage_ShouldNotHaveNextPage()
    {
        var items = Enumerable.Range(1, 10).ToList();

        var result = PaginatedList<int>.Create(items, 2, 5);

        result.HasNextPage.Should().BeFalse();
        result.HasPreviousPage.Should().BeTrue();
    }
}