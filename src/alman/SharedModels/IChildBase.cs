namespace Alman.SharedModels;

/// <summary>
/// Represents Child entity common model.
/// </summary>
public interface IChildBase : IIdentifier
{
    /// <summary>
    /// First name of teh child.
    /// </summary>
    public string ChildName { get; set; }

    /// <summary>
    /// Last name of the child.
    /// </summary>
    public string ChildLastName { get; set; }

    /// <summary>
    /// Type of the contract of the child.
    /// </summary>
    public int ChildContract { get; set; }

    /// <summary>
    /// Number of the group, the child is in.
    /// </summary>
    public int ChildGroup { get; set; }

    /// <summary>
    /// State that indicates if the child is active (goes to the kindergarten or no).
    /// </summary>
    public int ChildState { get; set; }

    /// <summary>
    /// Year when the child started going to kindergarten.
    /// </summary>
    public int ChildStartYear { get; set; }

    /// <summary>
    /// Month when the child started going to the kindergarten.
    /// </summary>
    public int ChildStartMonth { get; set; }
}