using Alman.SharedModels;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Models;

/// <summary>
/// Database model for the montly activity model.
/// </summary>
public partial class YearMonthActivity : IYearMonthActivityBase, IDeleteDependable
{
    public int Id { get; set; }

    public int YmchildId { get; set; }

    public int YmactivityId { get; set; }

    public int YmactivitySum { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public int YmwayOfPaying { get; set; }

    public int YmwasPaid { get; set; }

    /// <summary>
    /// Activity to which the monthly expens belongs.
    /// </summary>
    public virtual Activity Ymactivity { get; set; } = null!;

    /// <summary>
    /// Child to whom the monthly expense for the activity belongs.
    /// </summary>
    public virtual Child Ymchild { get; set; } = null!;

    public void DeleteDependable(DbContext dbContext) { }
}
