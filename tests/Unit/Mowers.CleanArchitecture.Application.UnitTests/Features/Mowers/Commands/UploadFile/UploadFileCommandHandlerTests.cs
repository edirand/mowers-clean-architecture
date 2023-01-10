using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Mowers.CleanArchitecture.Application.Contracts.Infrastructure.FileStorage;
using Mowers.CleanArchitecture.Application.Contracts.Persistence;
using Mowers.CleanArchitecture.Application.Features.Mowers.Commands.UploadFile;
using Mowers.CleanArchitecture.Application.UnitTests.Mocks;
using Mowers.CleanArchitecture.Domain.Entities;
using Shouldly;
using Xunit;

namespace Mowers.CleanArchitecture.Application.UnitTests.Features.Mowers.Commands.UploadFile;

public class UploadFileCommandHandlerTests
{
    private readonly Mock<IFileStorage> _fileStorageMock;
    private readonly Mock<IProcessingRepository> _processingRepositoryMock;
    private readonly UploadFileCommandHandler _sut;

    public UploadFileCommandHandlerTests()
    {
        _fileStorageMock = RepositoryMock.GetFileStorage();
        _processingRepositoryMock = RepositoryMock.GetProcessingRepository();
        _sut = new UploadFileCommandHandler(_fileStorageMock.Object, _processingRepositoryMock.Object);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task ShouldStoreFile()
    {
        await Act();
        _fileStorageMock.Verify(x => x.Store(It.IsAny<MemoryStream>()), Times.Once);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task ShouldSaveFileProcessing()
    {
        await Act();
        _processingRepositoryMock.Verify(x => x.Add(It.IsAny<FileProcessing>()), Times.Once);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task ShouldReturnFileProcessingId()
    {
        var result = await Act();
        result.ShouldNotBeNull();
        result.ProcessingId.ShouldNotBe(Guid.Empty);
    }

    private async Task<UploadFileCommandResponse> Act() =>
        await _sut.Handle(new UploadFileCommand(new MemoryStream()), CancellationToken.None);
}