using Mowers.CleanArchitecture.Domain.Entities;

namespace Mowers.CleanArchitecture.Domain.Factories;

/// <summary>
/// Defines an abstract factory for mowers and lawns.
/// </summary>
public interface IMowersFactory
{
    /// <summary>
    /// Creates an instance of <see cref="IMower"/> at the given position and with the given original orientation.
    /// </summary>
    /// <param name="position">The position of the mower.</param>
    /// <param name="orientation">The orientation of the mower.</param>
    /// <returns>An instance of <see cref="IMower"/> at the given position and with the given orientation.</returns>
    public IMower CreateMower(Point position, Direction orientation);
    
    /// <summary>
    /// Creates an instance of <see cref="ILawn"/>.
    /// </summary>
    /// <param name="topRight">The top right point of the lawn.</param>
    /// <returns>An instance of <see cref="ILawn"/>.</returns>
    public ILawn CreateLawn(Point topRight);
}
