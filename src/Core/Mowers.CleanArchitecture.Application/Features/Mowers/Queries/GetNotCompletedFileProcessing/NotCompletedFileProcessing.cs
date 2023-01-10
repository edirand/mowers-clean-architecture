namespace Mowers.CleanArchitecture.Application.Features.Mowers.Queries.GetNotCompletedFileProcessing;

/// <summary>
/// Not completed file processing response for <see cref="GetNotCompletedFileProcessingQuery"/>.
/// </summary>
/// <param name="Ids">The ids of the not completed file processing.</param>
public record NotCompletedFileProcessing(IEnumerable<Guid> Ids);
