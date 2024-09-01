using Alman.SharedModels;
using Microsoft.EntityFrameworkCore;
namespace DbAccess.Models;

/// <summary>
/// Database model for the yearly subscription entity.
/// </summary>
public partial class YearSub : IYearSubBase, IDeleteDependable
{
    public int Id { get; set; }

    public int YchildId { get; set; }

    public int Yyear { get; set; }
    public int Month { get; set; }

    public int Payment { get; set; }

    public int WayOfaying { get; set; }

    /// <summary>
    /// Child that to whom the subscription belongs.
    /// </summary>
    public virtual Child Ychild { get; set; } = null!;

    public void DeleteDependable(DbContext dbContext) { }
}
