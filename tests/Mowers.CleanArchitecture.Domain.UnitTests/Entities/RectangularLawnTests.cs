using Mowers.CleanArchitecture.Domain.Entities;
using Shouldly;
using Xunit;

namespace Mowers.CleanArchitecture.Domain.UnitTests.Entities;

/// <summary>
/// Tests for the <see cref="RectangularLawn"/> class.
/// </summary>
public class RectangularLawnTests
{
    private readonly RectangularLawn _sut;

    public RectangularLawnTests()
    {
        _sut = new RectangularLawn(5, 5);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(5, 5)]
    [InlineData(1, 4)]
    public void ShouldBeInside(int x, int y)
    {
        var point = new Point(x, y);
        _sut.IsInside(point).ShouldBeTrue();
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(6, 0)]
    [InlineData(0, 6)]
    [InlineData(6, 6)]
    public void ShouldNotBeInside(int x, int y)
    {
        var point = new Point(x, y);
        _sut.IsInside(point).ShouldBeFalse();
    }
}