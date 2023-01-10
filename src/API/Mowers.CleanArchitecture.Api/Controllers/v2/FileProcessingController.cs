using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mowers.CleanArchitecture.Api.Models;
using Mowers.CleanArchitecture.Application.Features.Mowers.Commands.UploadFile;
using Mowers.CleanArchitecture.Application.Features.Mowers.Queries.GetFileProcessing;

namespace Mowers.CleanArchitecture.Api.Controllers.v2;

/// <summary>
/// A controller to manage file processing.
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
[Produces("application/json")]
public class FileProcessingController : ControllerBase
{
     private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Initializes a new instance of <see cref="FileProcessingController"/> class.
    /// </summary>
    /// <param name="mediator">An instance of <see cref="IMediator"/>.</param>
    /// <param name="mapper">An instance of <see cref="IMapper"/>.</param>
    public FileProcessingController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Get a file processing result
    /// </summary>
    /// <remarks>
    /// Gets the result of a previously scheduled file processing.
    /// </remarks>
    /// <param name="id">The identifier of the file processing to get.</param>
    [HttpGet("{id}", Name = "get-file-id")]
    [ProducesResponseType(typeof(FileProcessing), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FileProcessing), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFileProcessing([Required] string id)
    {
        var item = await _mediator.Send(new GetFileProcessingByIdQuery(Guid.Parse(id)));
        return Ok(_mapper.Map<FileProcessing>(item));
    }
    
    /// <summary>
    /// Create a new file processing
    /// </summary>
    /// <remarks>
    /// Uploads a file containing instructions to process and schedule it to be processed asynchronously.
    /// </remarks>
    [HttpPost(Name = "post-upload-file")]
    [ProducesResponseType(typeof(FileProcessing), StatusCodes.Status201Created)]
    public async Task<IActionResult> UploadFile([Required] IFormFile file)
    {
        var result = await _mediator.Send(new UploadFileCommand(file.OpenReadStream()));
        return CreatedAtAction(nameof(GetFileProcessing), new { id = result.Id.ToString() }, _mapper.Map<FileProcessing>(result));
    }
}
