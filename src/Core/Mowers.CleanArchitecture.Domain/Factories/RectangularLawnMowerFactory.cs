using Mowers.CleanArchitecture.Domain.Entities;

namespace Mowers.CleanArchitecture.Domain.Factories;

/// <summary>
/// Represents a concrete factory to create <see cref="RectangularLawn"/> and <see cref="Mower"/> instances.
/// </summary>
public class RectangularLawnMowerFactory : IMowersFactory
{
    /// <inheritdoc />
    public IMower CreateMower(Point position, Direction orientation)
    {
        return new Mower(position.X, position.Y, orientation);
    }

    /// <inheritdoc />
    public ILawn CreateLawn(Point topRight)
    {
        return new RectangularLawn(topRight.X, topRight.Y);
    }
}
