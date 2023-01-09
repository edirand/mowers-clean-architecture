namespace Mowers.CleanArchitecture.Domain.Entities;

/// <summary>
/// Defines a mower.
/// </summary>
public interface IMower
{
    /// <summary>
    /// The current position of the mower.
    /// </summary>
    public Point Position { get; }
    
    /// <summary>
    /// Turns left.
    /// </summary>
    public void TurnLeft();
    
    /// <summary>
    /// Turns right.
    /// </summary>
    public void TurnRight();
    
    /// <summary>
    /// Move forward on a <see cref="ILawn"/>.
    /// </summary>
    /// <param name="lawn">The <see cref="ILawn"/> to move on.</param>
    public void MoveOn(ILawn lawn);
}