using MediatR;
using SpectrumCare.BuildingBlocks.Domain.Primitives;

namespace SpectrumCare.BuildingBlocks.Application.Abstractions.Messaging;

/// <summary>
/// Marker interface for queries that return a typed Result.
/// Implement this for use cases that retrieve data without modifying state.
/// Queries should never cause side effects — they only read data.
/// Example: GetClientByIdQuery, GetAppointmentListQuery.
/// </summary>
/// <typeparam name="TResponse">The type of data returned on success.</typeparam>
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}