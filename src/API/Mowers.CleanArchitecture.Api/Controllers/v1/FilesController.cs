using System.ComponentModel.DataAnnotations;
using System.Text;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mowers.CleanArchitecture.Api.Models;
using Mowers.CleanArchitecture.Application.Features.Mowers.Commands.ProcessFile;

namespace Mowers.CleanArchitecture.Api.Controllers.v1;

/// <summary>
/// A controller to import files.
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Produces("application/json")]
public class FilesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Initializes a new instance of <see cref="FilesController"/> class.
    /// </summary>
    /// <param name="mediator">An instance of <see cref="IMediator"/>.</param>
    /// <param name="mapper">An instance of <see cref="IMapper"/>.</param>
    public FilesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Process an instructions file.
    /// </summary>
    /// <remarks>
    /// Uploads a text file containing instructions to process and returns the result of the processing operation.
    /// A template for the instruction file can be found [here](https://github.com/edirand/mowers-clean-architecture).
    /// </remarks>
    /// <param name="file">The file containing instructions to process.</param>
    /// <returns>The final position and orientation of each mower.</returns>
    [HttpPost(Name = "post-files")]
    [ProducesResponseType(typeof(FileProcessingResult), StatusCodes.Status200OK, "application/json")]
    [Produces("application/json", "text/plain")]
    public async Task<IActionResult> PostFile([Required] IFormFile file)
    {
        var result = await _mediator.Send(new ProcessFileCommand(file.OpenReadStream()));
        if (Request.Headers.Accept == "application/json") return Ok(_mapper.Map<FileProcessingResult>(result));

        var sb = new StringBuilder();
        foreach (var resultMower in result.Mowers)
        {
            sb.AppendLine(resultMower);
        }

        return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/plain");
    }
}
