using MediatR;

namespace Mowers.CleanArchitecture.Application.Features.Mowers.Queries.GetNotCompletedFileProcessing;

/// <summary>
/// A query to get not completed file processing.
/// </summary>
public record GetNotCompletedFileProcessingQuery() : IRequest<NotCompletedFileProcessing>;
