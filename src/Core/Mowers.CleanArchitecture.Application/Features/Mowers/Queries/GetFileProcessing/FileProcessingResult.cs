namespace Mowers.CleanArchitecture.Application.Features.Mowers.Queries.GetFileProcessing;

/// <summary>
/// A result for <see cref="GetFileProcessingByIdQuery"/>.
/// </summary>
public record FileProcessingResult(Guid Id, bool Completed, IEnumerable<string> Mowers, DateTime CreatedAt, DateTime UpdatedAt);
