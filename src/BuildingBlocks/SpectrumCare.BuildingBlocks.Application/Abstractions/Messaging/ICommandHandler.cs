using MediatR;
using SpectrumCare.BuildingBlocks.Domain.Primitives;

namespace SpectrumCare.BuildingBlocks.Application.Abstractions.Messaging;

/// <summary>
/// Defines a handler for a command that returns a non-generic Result.
/// All command handlers in the system must implement this interface.
/// Ensures consistent handling pattern across all services.
/// </summary>
/// <typeparam name="TCommand">The command type to handle.</typeparam>
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

/// <summary>
/// Defines a handler for a command that returns a typed Result.
/// All command handlers that return data must implement this interface.
/// Example: CreateClientCommandHandler implements ICommandHandler{CreateClientCommand, Guid}.
/// </summary>
/// <typeparam name="TCommand">The command type to handle.</typeparam>
/// <typeparam name="TResponse">The type of data returned on success.</typeparam>
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}