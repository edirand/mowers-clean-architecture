using Mowers.CleanArchitecture.Domain.Entities;
using Shouldly;
using Xunit;

namespace Mowers.CleanArchitecture.Domain.UnitTests.Entities;

/// <summary>
/// Tests for the <see cref="Mower"/> class.
/// </summary>
public class MowerTests
{
    [Theory]
    [InlineData(Direction.N, Direction.W)]
    [InlineData(Direction.W, Direction.S)]
    [InlineData(Direction.S, Direction.E)]
    [InlineData(Direction.E, Direction.N)]
    public void ShouldTurnLeft(Direction orientation, Direction expected)
    {
        var mower = new Mower(0, 0, orientation);
        mower.TurnLeft();
        mower.Direction.ShouldBe(expected);
    }

    [Theory]
    [InlineData(Direction.N, Direction.E)]
    [InlineData(Direction.E, Direction.S)]
    [InlineData(Direction.S, Direction.W)]
    [InlineData(Direction.W, Direction.N)]
    public void ShouldTurnRight(Direction orientation, Direction expected)
    {
        var mower = new Mower(0, 0, orientation);
        mower.TurnRight();
        mower.Direction.ShouldBe(expected);
    }

    [Theory]
    [InlineData(Direction.N, 1, 2)]
    [InlineData(Direction.S, 1, 0)]
    [InlineData(Direction.E, 2, 1)]
    [InlineData(Direction.W, 0, 1)]
    public void ShouldMoveForward(Direction orientation, int expectedX, int expectedY)
    {
        var mower = new Mower(1, 1, orientation);
        mower.MoveOn(new RectangularLawn(5, 5));
        mower.Position.ShouldBe(new Point(expectedX, expectedY));
    }

    [Theory]
    [InlineData(0, 0, Direction.W)]
    [InlineData(0, 0, Direction.S)]
    [InlineData(5, 0, Direction.E)]
    [InlineData(0, 5, Direction.N)]
    public void ShouldStayInPositionIfOutsideOfTheLawnAfterMoving(int x, int y, Direction orientation)
    {
        var lawn = new RectangularLawn(5, 5);
        var mower = new Mower(x, y, orientation);
        mower.MoveOn(lawn);
        mower.Position.ShouldBe(new Point(x, y));
    }

    [Fact]
    public void ShouldStayInPositionIfCollideWithOtherMower()
    {
        var lawn = new RectangularLawn(5, 5);
        var mower = new Mower(0, 0, Direction.N);
        lawn.AddMower(mower);
        var collisionMower = new Mower(0, 1, Direction.N);
        lawn.AddMower(collisionMower);
        mower.MoveOn(lawn);
        mower.Position.ShouldBe(new Point(0, 0));
    }

    [Theory]
    [InlineData(0, 0, Direction.N)]
    [InlineData(2, 4, Direction.S)]
    [InlineData(3, 2, Direction.E)]
    [InlineData(1, 1, Direction.W)]
    public void ShouldPrintStateToString(int x, int y, Direction orientation)
    {
        var mower = new Mower(x, y, orientation);
        var print = mower.ToString();
        print.ShouldBe($"{x} {y} {orientation}");
    }
}