using Alman.SharedModels;
namespace AlmanUI.Models;

/// <summary>
/// UI model of child entity.
/// </summary>
public partial class ChildUI : IChildBase
{
    public int Id { get; set; }

    public string ChildName { get; set; } = null!;

    public string ChildLastName { get; set; } = null!;

    public int ChildContract { get; set; }

    public int ChildGroup { get; set; }

    public int ChildState { get; set; }

    public int ChildStartYear { get; set; }

    public int ChildStartMonth { get; set; }
    
}
