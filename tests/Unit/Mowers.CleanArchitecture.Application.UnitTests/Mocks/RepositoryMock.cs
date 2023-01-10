using System;
using System.IO;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using Mowers.CleanArchitecture.Application.Contracts.Infrastructure.FileStorage;
using Mowers.CleanArchitecture.Application.Contracts.Persistence;

namespace Mowers.CleanArchitecture.Application.UnitTests.Mocks;

public class RepositoryMock
{
    public static Mock<IProcessingRepository> GetProcessingRepository()
    {
        var fixture = new Fixture();
        var mockProcessingRepository = new Mock<IProcessingRepository>();

        return mockProcessingRepository;
    }

    public static Mock<IFileStorage> GetFileStorage()
    {
        var mockFileStorage = new Mock<IFileStorage>();
        mockFileStorage.Setup(x => x.Store(It.IsAny<Stream>())).ReturnsAsync(Path.GetRandomFileName);

        return mockFileStorage;
    }
}