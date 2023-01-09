using MediatR;
using Mowers.CleanArchitecture.Domain;
using Mowers.CleanArchitecture.Domain.Entities;
using Mowers.CleanArchitecture.Domain.Factories;

namespace Mowers.CleanArchitecture.Application.Features.Mowers.Commands.ProcessFile;

/// <summary>
/// A handler for <see cref="ProcessFileCommand"/>.
/// </summary>
public class ProcessFileCommandHandler : IRequestHandler<ProcessFileCommand, ProcessFileCommandResponse>
{
    /// <summary>
    /// Processes a file. The handler parses the lines in the file and invokes the corresponding instructions on the mowers.
    /// </summary>
    /// <param name="request">The request containing the file data to process.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A instance of <see cref="ProcessFileCommandResponse"/> class containing the final position and orientation of all the mowers in the file.</returns>
    public Task<ProcessFileCommandResponse> Handle(ProcessFileCommand request, CancellationToken cancellationToken)
    {
        var factory = new RectangularLawnMowerFactory();
        using var reader = new StreamReader(request.Data);

        var lawnDimensions = reader.ReadLine().Split(' ');
        var lawn = factory.CreateLawn(new Point(int.Parse(lawnDimensions[0]), int.Parse(lawnDimensions[1])));
        var mowers = new List<IMower>();
        
        while (!reader.EndOfStream)
        {
            var mowerLine = reader.ReadLine().Split(' ');
            var mower = factory.CreateMower(new Point(int.Parse(mowerLine[0]), int.Parse(mowerLine[1])), Enum.Parse<Direction>(mowerLine[2], true));
            lawn.AddMower(mower);
            var instructions = reader.ReadLine();
            foreach (var instruction in instructions)
            {
                switch (instruction)
                {
                    case 'L':
                        mower.TurnLeft();
                        break;
                    case 'R':
                        mower.TurnRight();
                        break;
                    case 'F':
                        mower.MoveOn(lawn);
                        break;
                }
            }
            
            mowers.Add(mower);
        }

        return Task.FromResult(new ProcessFileCommandResponse(mowers.Select(x => x.ToString()!)));
    }
}
