namespace Alman.SharedModels;

/// <summary>
/// Requirement for id property.
/// </summary>
public interface IIdentifier
{
    public int Id { get; set; }
}

/// <summary>
/// Represents common model of the Activity entity.
/// </summary>
public interface IActivityBase : IIdentifier
{
    /// <summary>
    /// Name of the activity.
    /// </summary>
    public string ActivityName { get; set; }

    /// <summary>
    /// Price of the activity.
    /// </summary>
    public int ActivityPrice { get; set; }
}