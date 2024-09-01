using Alman.SharedModels;

namespace AlmanUI.Models;

/// <summary>
/// UI model of the satff activity model.
/// </summary>
public class StaffActivityUI : IStaffActivityBase
{
    public int Id { get; set; }

    public string? ActivityName { get; set; }
}
