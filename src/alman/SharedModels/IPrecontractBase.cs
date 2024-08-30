namespace Alman.SharedModels;

/// <summary>
/// Common model for the Precontract entity.
/// </summary>
public interface IPrecontractBase : IIdentifier
{
    /// <summary>
    /// Id of the child precontract is for.
    /// </summary>
    public int PchildId { get; set; }

    /// <summary>
    /// Sum that was paid by the parents.
    /// </summary>
    public int Psum { get; set; }

    /// <summary>
    /// Comment place for notes.
    /// </summary>
    public string? Pcomment { get; set; }

    /// <summary>
    /// Year of the precontract (must be equal to the startYear of the child).
    /// </summary>
    public int? PYear { get; set; }
    
    /// <summary>
    /// Month of the precontract (must be equal to the StartMonth of the child).
    /// </summary>
    public int? PMonth { get; set; }
}
