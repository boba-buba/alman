namespace Alman.SharedModels;

/// <summary>
/// Common model for the User entity.
/// </summary>
public interface IUserBase : IIdentifier
{
    /// <summary>
    /// Name of the user (used as a login).
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Password of the user.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Permissions flags, to what extent user can manage other users.
    /// </summary>
    public int Permissions { get; set; }

}
