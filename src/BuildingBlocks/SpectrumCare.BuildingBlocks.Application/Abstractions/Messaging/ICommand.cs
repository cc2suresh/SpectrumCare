using MediatR;
using SpectrumCare.BuildingBlocks.Domain.Primitives;

namespace SpectrumCare.BuildingBlocks.Application.Abstractions.Messaging;

/// <summary>
/// Marker interface for commands that return a non-generic Result.
/// Implement this for use cases that perform an action without returning data.
/// Example: DeleteClient, SendNotification.
/// </summary>
public interface ICommand : IRequest<Result>
{
}

/// <summary>
/// Marker interface for commands that return a typed Result.
/// Implement this for use cases that perform an action and return data.
/// Example: CreateClient returns Result{Guid}, LoginUser returns Result{TokenResponse}.
/// </summary>
/// <typeparam name="TResponse">The type of data returned on success.</typeparam>
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}