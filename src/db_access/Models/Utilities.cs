using Alman.SharedModels;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Models;

/// <summary>
/// Database model for the User entity.
/// </summary>
public class User : IUserBase, IDeleteDependable
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Password { get; set; } = "";

    public int Permissions { get; set; }
    public void DeleteDependable(DbContext dbContext)
    {

    }
}

/// <summary>
/// Database model for the yearly results model.
/// </summary>
public class YearResult : IYearResultBase, IDeleteDependable
{
    public int Id { get; set; }
    public int Year { get; set; }
    public int YearRemainder { get; set; }
    public void DeleteDependable(DbContext dbContext)
    {

    }
}