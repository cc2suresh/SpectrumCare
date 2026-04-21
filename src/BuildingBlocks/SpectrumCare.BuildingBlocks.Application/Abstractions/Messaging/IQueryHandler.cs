using MediatR;
using SpectrumCare.BuildingBlocks.Domain.Primitives;

namespace SpectrumCare.BuildingBlocks.Application.Abstractions.Messaging;

/// <summary>
/// Defines a handler for a query that returns a typed Result.
/// All query handlers in the system must implement this interface.
/// Query handlers must be side-effect free — no writes, no state changes.
/// Example: GetClientByIdQueryHandler implements IQueryHandler{GetClientByIdQuery, ClientResponse}.
/// </summary>
/// <typeparam name="TQuery">The query type to handle.</typeparam>
/// <typeparam name="TResponse">The type of data returned on success.</typeparam>
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}