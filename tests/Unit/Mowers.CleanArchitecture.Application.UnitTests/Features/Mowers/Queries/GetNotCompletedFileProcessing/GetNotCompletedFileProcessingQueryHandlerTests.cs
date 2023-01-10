using System.Threading;
using System.Threading.Tasks;
using Moq;
using Mowers.CleanArchitecture.Application.Contracts.Persistence;
using Mowers.CleanArchitecture.Application.Features.Mowers.Queries.GetNotCompletedFileProcessing;
using Mowers.CleanArchitecture.Application.UnitTests.Mocks;
using Shouldly;
using Xunit;

namespace Mowers.CleanArchitecture.Application.UnitTests.Features.Mowers.Queries.GetNotCompletedFileProcessing;

/// <summary>
/// Tests for the <see cref="GetNotCompletedFileProcessingQueryHandler"/> class.
/// </summary>
public class GetNotCompletedFileProcessingQueryHandlerTests
{
    private readonly Mock<IProcessingRepository> _repositoryMock;
    private readonly GetNotCompletedFileProcessingQueryHandler _sut;

    public GetNotCompletedFileProcessingQueryHandlerTests()
    {
        _repositoryMock = RepositoryMock.GetProcessingRepository();
        _sut = new GetNotCompletedFileProcessingQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task ShouldLoadNotCompletedFileProcessingFromRepository()
    {
        await Act();
        _repositoryMock.Verify(x=>x.ListNotCompletedProcessing(), Times.Once);
    }
    
    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task ShouldReturnNotCompletedFileProcessing()
    {
        var result = await Act();
        result.ShouldNotBeNull();
        result.Ids.ShouldNotBeNull();
    }

    private async Task<NotCompletedFileProcessing> Act() =>
        await _sut.Handle(new GetNotCompletedFileProcessingQuery(), CancellationToken.None);
}