using FluentAssertions;
using SpectrumCare.BuildingBlocks.Domain.Primitives;
using Xunit;

namespace SpectrumCare.BuildingBlocks.UnitTests.Primitives;

/// <summary>
/// Unit tests for Result and Result{T} pattern.
/// Validates success, failure, and error propagation behavior.
/// </summary>
public class ResultTests
{
    [Fact]
    public void Success_ShouldReturnSuccessResult()
    {
        var result = Result.Success();

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().Be(Error.None);
    }

    [Fact]
    public void Failure_ShouldReturnFailureResult()
    {
        var error = new Error("Test.Error", "Test error message");

        var result = Result.Failure(error);

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void SuccessWithValue_ShouldReturnValueOnAccess()
    {
        var result = Result.Success<string>("test-value");

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("test-value");
    }

    [Fact]
    public void FailureWithValue_ShouldThrowOnValueAccess()
    {
        var error = new Error("Test.Error", "Test error message");
        var result = Result.Failure<string>(error);

        result.IsFailure.Should().BeTrue();
        var act = () => result.Value;
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Success_WithErrorNone_ShouldNotThrow()
    {
        var act = () => Result.Success();
        act.Should().NotThrow();
    }

    [Fact]
    public void Failure_WithErrorNone_ShouldThrow()
    {
        var act = () => Result.Failure(Error.None);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Error_NotFound_ShouldReturnCorrectError()
    {
        var error = Error.NotFound("Client", Guid.NewGuid());

        error.Code.Should().Contain("NotFound");
        error.Message.Should().Contain("Client");
    }

    [Fact]
    public void ImplicitConversion_ShouldCreateSuccessResult()
    {
        Result<string> result = "implicit-value";

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("implicit-value");
    }
}