using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Mowers.CleanArchitecture.Application.Contracts.Persistence;
using Mowers.CleanArchitecture.Application.Exceptions;
using Mowers.CleanArchitecture.Application.Features.Mowers.Queries.GetFileProcessing;
using Mowers.CleanArchitecture.Application.UnitTests.Mocks;
using Mowers.CleanArchitecture.Domain.Entities;
using Shouldly;
using Xunit;

namespace Mowers.CleanArchitecture.Application.UnitTests.Features.Mowers.Queries.GetFileProcessing;

/// <summary>
/// Tests for the <see cref="GetFileProcessingByIdQueryHandler"/> class.
/// </summary>
public class GetFileProcessingByIdHandlerTests
{
    private readonly Mock<IProcessingRepository> _repositoryMock;
    private readonly GetFileProcessingByIdQueryHandler _sut;

    public GetFileProcessingByIdHandlerTests()
    {
        _repositoryMock = RepositoryMock.GetProcessingRepository();
        _sut = new GetFileProcessingByIdQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task ShouldLoadItemFromRepository()
    {
        var id = Guid.NewGuid();
        await Act(id);
        _repositoryMock.Verify(x=>x.GetById(id), Times.Once);
    }

    [Fact]    
    [Trait("Category", "UnitTest")]
    public async Task ShouldThrowExceptionWhenItemDoesNotExist()
    {
        _repositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync((FileProcessing?)null);
        await Act().ShouldThrowAsync<NotFoundException>();
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task ShouldReturnFileProcessingResultWhenItemExists()
    {
        var result = await Act();
        result.ShouldNotBeNull();
    }

    private Task<FileProcessingResult> Act() => Act(Guid.NewGuid());
    private async Task<FileProcessingResult> Act(Guid id) => await _sut.Handle(new GetFileProcessingByIdQuery(id), CancellationToken.None);
}