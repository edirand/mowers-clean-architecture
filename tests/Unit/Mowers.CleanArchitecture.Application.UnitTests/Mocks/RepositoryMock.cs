using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using Mowers.CleanArchitecture.Application.Contracts.Infrastructure.FileStorage;
using Mowers.CleanArchitecture.Application.Contracts.Persistence;
using Mowers.CleanArchitecture.Domain.Entities;

namespace Mowers.CleanArchitecture.Application.UnitTests.Mocks;

public class RepositoryMock
{
    public static Mock<IProcessingRepository> GetProcessingRepository()
    {
        var fixture = new Fixture();
        var mockProcessingRepository = new Mock<IProcessingRepository>();
        mockProcessingRepository.Setup(x => x.ListNotCompletedProcessing())
            .ReturnsAsync(fixture.Build<FileProcessing>().With(x=>x.Completed, false).CreateMany().ToList());
        mockProcessingRepository.Setup(x => x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(fixture.Create<FileProcessing>());
        return mockProcessingRepository;
    }

    public static Mock<IFileStorage> GetFileStorage()
    {
        var mockFileStorage = new Mock<IFileStorage>();
        mockFileStorage.Setup(x => x.Store(It.IsAny<Stream>())).ReturnsAsync(Path.GetRandomFileName);
        mockFileStorage.Setup(x => x.Load(It.IsAny<string>())).Returns(new MemoryStream());

        return mockFileStorage;
    }
}