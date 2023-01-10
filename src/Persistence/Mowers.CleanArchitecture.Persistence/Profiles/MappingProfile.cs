using AutoMapper;
using Mowers.CleanArchitecture.Domain.Entities;
using Mowers.CleanArchitecture.Persistence.Models;

namespace Mowers.CleanArchitecture.Persistence.Profiles;

/// <summary>
/// A mapping profile for the persistence layer.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of <see cref="MappingProfile"/> class.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<FileProcessing, FileProcessingDocument>()
            .ReverseMap();
    }
}
