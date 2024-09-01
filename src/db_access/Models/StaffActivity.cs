using Alman.SharedModels;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Models;

/// <summary>
/// Database model for the staff member entity.
/// </summary>
public partial class StaffActivity : IStaffActivityBase, IDeleteDependable
{
    public int Id { get; set; }

    public string? ActivityName { get; set; }

    /// <summary>
    /// Collection of monthly expenses that belong to the activity.
    /// </summary>
    public virtual ICollection<YearMonthStaffActivity> YearMonthStaffActivities { get; set; } = new List<YearMonthStaffActivity>();

    public void DeleteDependable(DbContext dbContext)
    {
        AlmanContext ctx = (AlmanContext)dbContext;
        ctx.RemoveRange(ctx.YearMonthStaffActivities.Where(activity => activity.StaffActivityId == Id).ToList());
    }
}
