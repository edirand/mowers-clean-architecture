namespace Mowers.CleanArchitecture.Domain.Entities;

/// <summary>
/// A rectangular lawn.
/// </summary>
public class RectangularLawn : ILawn
{
    private readonly Point _topRight;
    private readonly List<IMower> _mowers;

    /// <summary>
    /// Initializes a new instance of the <see cref="RectangularLawn"/> class.
    /// </summary>
    /// <param name="height">The height of the lawn.</param>
    /// <param name="width">The width of the lawn.</param>
    public RectangularLawn(int height, int width)
    {
        _topRight = new Point(height, width);
        _mowers = new List<IMower>();
    }

    /// <inheritdoc />
    public bool IsInside(Point point)
    {
        return
            0 <= point.X
            && point.X <= _topRight.X
            && 0 <= point.Y
            && point.Y <= _topRight.Y;
    }

    /// <inheritdoc />
    public void AddMower(IMower mower)
    {
        _mowers.Add(mower);
    }

    public bool IsOccupied(Point point)
    {
        return _mowers.Any(x => x.Position == point);
    }
}