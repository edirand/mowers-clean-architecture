using AutoMapper;
using Mowers.CleanArchitecture.Api.Models;
using Mowers.CleanArchitecture.Application.Features.Mowers.Commands.ProcessFile;
using Mowers.CleanArchitecture.Application.Features.Mowers.Commands.UploadFile;

namespace Mowers.CleanArchitecture.Api.Profiles;

/// <summary>
/// A mapping profile for the API.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of <see cref="MappingProfile"/> class.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<ProcessFileCommandResponse, FileProcessingResult>();
        CreateMap<UploadFileCommandResponse, FileProcessing>();
        CreateMap<Application.Features.Mowers.Queries.GetFileProcessing.FileProcessingResult, FileProcessing>();
    }
}