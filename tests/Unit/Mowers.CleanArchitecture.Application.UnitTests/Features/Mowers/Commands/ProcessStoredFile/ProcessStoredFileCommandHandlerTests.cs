using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Moq;
using Mowers.CleanArchitecture.Application.Contracts.Infrastructure.FileStorage;
using Mowers.CleanArchitecture.Application.Contracts.Persistence;
using Mowers.CleanArchitecture.Application.Exceptions;
using Mowers.CleanArchitecture.Application.Features.Mowers.Commands.ProcessFile;
using Mowers.CleanArchitecture.Application.Features.Mowers.Commands.ProcessStoredFile;
using Mowers.CleanArchitecture.Application.UnitTests.Mocks;
using Mowers.CleanArchitecture.Domain.Entities;
using Shouldly;
using Xunit;

namespace Mowers.CleanArchitecture.Application.UnitTests.Features.Mowers.Commands.ProcessStoredFile;

/// <summary>
/// Tests for the <see cref="ProcessStoredFileCommandHandler"/> class.
/// </summary>
public class ProcessStoredFileCommandHandlerTests
{
    private readonly Mock<IFileStorage> _fileStorageMock;
    private readonly Mock<IProcessingRepository> _repositoryMock;
    private readonly ProcessStoredFileCommandHandler _sut;

    public ProcessStoredFileCommandHandlerTests()
    {
        var fixture = new Fixture();
        _fileStorageMock = RepositoryMock.GetFileStorage();
        _repositoryMock = RepositoryMock.GetProcessingRepository();
        var handlerMock = new Mock<IRequestHandler<ProcessFileCommand, ProcessFileCommandResponse>>();
        handlerMock
            .Setup(x => x.Handle(It.IsAny<ProcessFileCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fixture.Create<ProcessFileCommandResponse>());
        _sut = new ProcessStoredFileCommandHandler(_fileStorageMock.Object, _repositoryMock.Object,
            handlerMock.Object);
    }

    [Fact]
    public async Task ShouldLoadFileProcessingById()
    {
        var id = Guid.NewGuid();
        await Act(id);
        _repositoryMock.Verify(x => x.GetById(id), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenFileProcessingDoesNotExist()
    {
        var id = Guid.NewGuid();
        _repositoryMock.Setup(x => x.GetById(id)).ReturnsAsync((FileProcessing?)null);
        await Act(id).ShouldThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ShouldLoadFileDataWhenFileProcessingExist()
    {
        await Act();
        _fileStorageMock.Verify(x => x.Load(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task ShouldSaveProcessingWhenCompleted()
    {
        FileProcessing? savedFileProcessing = null;
        _repositoryMock
            .Setup(x => x.Update(It.IsAny<FileProcessing>()))
            .Callback<FileProcessing>(fileProcessing => savedFileProcessing = fileProcessing);
        await Act();

        savedFileProcessing.ShouldNotBeNull();
        savedFileProcessing.Completed.ShouldBeTrue();
        savedFileProcessing.Mowers.ShouldNotBeNull();
    }

    private Task Act() => Act(Guid.NewGuid());

    private async Task Act(Guid id) => await _sut.Handle(new ProcessStoredFileCommand(id), CancellationToken.None);
}