namespace Alman.SharedModels;

/// <summary>
/// Common model for the Staff activity entity.
/// </summary>
public interface IStaffActivityBase : IIdentifier
{
    /// <summary>
    /// Name of the activity staff member can do.
    /// </summary>
    public string? ActivityName { get; set; }

}
