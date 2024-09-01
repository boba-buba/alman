using Alman.SharedModels;

namespace AlmanUI.Models;

/// <summary>
/// UI model of the Precontract entity.
/// </summary>
internal class PrecontractUI : IPrecontractBase
{
    public int Id { get; set; }
    public int PchildId { get; set; }

    public int Psum { get; set; }

    public string? Pcomment { get; set; }

    public int? PYear { get; set; }

    public int? PMonth { get; set; }
}

/// <summary>
/// Utility class for Precontracts View.
/// </summary>
public class PrecontractCompositeItem
{
    /// <summary>
    /// Child.
    /// </summary>
    public IChildBase? PChild { get; set; }

    /// <summary>
    /// Child's precontract.
    /// </summary>
    public IPrecontractBase? Precontract { get; set; }
}

