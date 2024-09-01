using Alman.SharedModels;

namespace AlmanUI.Models;

/// <summary>
/// UI model of the User entity.
/// </summary>
public class UserUI : IUserBase
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Password { get; set; } = "";

    public int Permissions { get; set; }
}
