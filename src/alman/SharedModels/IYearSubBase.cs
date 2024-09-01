namespace Alman.SharedModels;

/// <summary>
/// Common model for the yearly child subscription.
/// </summary>
public interface IYearSubBase : IIdentifier
{
    /// <summary>
    /// Id of the child.
    /// </summary>
    public int YchildId { get; set; }

    /// <summary>
    /// Year for which the subscription is.
    /// </summary>
    public int Yyear { get; set; }

    /// <summary>
    /// Month for which the subscription is.
    /// </summary>
    public int Month { get; set; }

    /// <summary>
    /// Sum that was paid by parents.
    /// </summary>
    public int Payment { get; set; }

    /// <summary>
    /// Way of paying for the subsription.
    /// </summary>
    public int WayOfaying { get; set; } 
}
