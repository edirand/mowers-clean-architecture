namespace Mowers.CleanArchitecture.Domain.Entities;

/// <summary>
/// Defines a lawn.
/// </summary>
public interface ILawn
{
    /// <summary>
    /// Verifies if a point is inside the lawn.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <returns><c>true</c> if the point is inside the lawn; otherwise, <c>false</c>.</returns>
    public bool IsInside(Point point);

    /// <summary>
    /// Adds a mower on the lawn.
    /// </summary>
    /// <param name="mower">The mower to add on the lawn.</param>
    public void AddMower(IMower mower);

    /// <summary>
    /// Verifies if a point in the lawn is occupied by a mower.
    /// </summary>
    /// <param name="point">The point on the lawn.</param>
    /// <returns><c>true</c> if the point is occupied by a mower; otherwise, <c>false</c></returns>
    public bool IsOccupied(Point point);
}