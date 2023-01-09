namespace Mowers.CleanArchitecture.Domain.Entities;

/// <summary>
/// A standard implementation of <see cref="IMower"/>.
/// </summary>
public class Mower : IMower
{
    private static readonly List<Direction> Directions = new() { Direction.N, Direction.E, Direction.S, Direction.W };
    private int _currentDirection;

    /// <inheritdoc />
    public Point Position { get; private set; }
    
    /// <summary>
    /// The current direction of the mower.
    /// </summary>
    public Direction Direction => Directions[_currentDirection];

    /// <summary>
    /// Initializes a new instance of the <see cref="Mower"/> class.
    /// </summary>
    /// <param name="x">The X coordinate of the mower.</param>
    /// <param name="y">The Y coordinate of the mower.</param>
    /// <param name="direction">The initial direction of the mower.</param>
    public Mower(int x, int y, Direction direction)
    {
        _currentDirection = Directions.IndexOf(direction);
        Position = new Point(x, y);
    }

    /// <inheritdoc />
    public void TurnLeft() => Turn(-1);

    /// <inheritdoc />
    public void TurnRight() => Turn(1);

    /// <inheritdoc />
    public void MoveOn(ILawn lawn)
    {
        var nextPosition = Direction switch
        {
            Direction.N => Position with { Y = Position.Y + 1 },
            Direction.E => Position with { X = Position.X + 1 },
            Direction.S => Position with { Y = Position.Y - 1 },
            Direction.W => Position with { X = Position.X - 1 }
        };

        if (!lawn.IsInside(nextPosition)) return;
        if(lawn.IsOccupied(nextPosition)) return;

        Position = nextPosition;
    }

    private void Turn(int offset)
    {
        _currentDirection += offset;
        if (_currentDirection < 0) _currentDirection = Directions.Count - 1;
        else _currentDirection %= Directions.Count;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Position.X} {Position.Y} {Direction}";
    }
}