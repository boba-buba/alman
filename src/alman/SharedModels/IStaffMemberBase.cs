namespace Alman.SharedModels;

/// <summary>
/// Common model for the staff member entity.
/// </summary>
public interface IStaffMemberBase : IIdentifier
{
    /// <summary>
    /// First name of the satff member.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Last name of the staff member.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Year when the staff member started to work.
    /// </summary>
    public int StartYear { get; set; }

    /// <summary>
    /// Month when the staff member started to work.
    /// </summary>
    public int StartMonth { get; set; }

    /// <summary>
    /// Flag that indicates if the staff member is still working or no.
    /// </summary>
    public int State { get; set; }

    /// <summary>
    /// Name of the staff member's position.
    /// </summary>
    public string? PositionName { get; set; }

    /// <summary>
    /// Salary that is paid for the position.
    /// </summary>
    public int PositionSalary { get; set; }
}