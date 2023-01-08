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
}