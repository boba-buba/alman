using Alman.SharedModels;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Models;

/// <summary>
/// Database model for the monthly staff activity expense wntity.
/// </summary>
public partial class YearMonthStaffActivity : IYearMonthStaffActivityBase, IDeleteDependable
{
    public int Id { get; set; }

    public int StaffMemberId { get; set; }

    public int StaffActivityId { get; set; }

    public decimal? SumPaid { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public int WasPaid { get; set; }

    /// <summary>
    /// Staff activity that was during the month.
    /// </summary>
    public virtual StaffActivity StaffActivity { get; set; } = null!;

    /// <summary>
    /// Staff member thet carried out the actiity.
    /// </summary>
    public virtual StaffMember StaffMember { get; set; } = null!;

    public void DeleteDependable(DbContext dbContext)
    {

    }
}
