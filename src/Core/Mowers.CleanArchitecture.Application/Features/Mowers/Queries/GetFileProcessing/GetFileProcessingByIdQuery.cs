using MediatR;

namespace Mowers.CleanArchitecture.Application.Features.Mowers.Queries.GetFileProcessing;

/// <summary>
/// A query to get a <see cref="FileProcessingResult"/> by ID.
/// </summary>
public record GetFileProcessingByIdQuery (Guid Id): IRequest<FileProcessingResult>;
