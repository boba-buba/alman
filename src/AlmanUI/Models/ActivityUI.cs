using Alman.SharedModels;
namespace AlmanUI.Models;

/// <summary>
/// UI model of Activity entity.
/// </summary>
public partial class ActivityUI : IActivityBase
{
    public int Id { get; set; }

    public string ActivityName { get; set; } = null!;

    public int ActivityPrice { get; set; }
}