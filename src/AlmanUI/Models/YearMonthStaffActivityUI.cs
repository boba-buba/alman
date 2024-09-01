using Alman.SharedModels;
using System.Collections.Generic;

namespace AlmanUI.Models;

/// <summary>
/// UI model of the monthly staff activity entity.
/// </summary>
public class YearMonthStaffActivityUI : IYearMonthStaffActivityBase
{
    public int Id { get; set; }
    public int StaffMemberId { get; set; }
    public int StaffActivityId { get; set; }
    public decimal? SumPaid { get; set; }
    public int WasPaid {  get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}

/// <summary>
/// Utility class for YearMonthStaffActivitiesViews.
/// </summary>
public class YearMonthStaffActivityCompositeItem
{
    /// <summary>
    /// Staff member.
    /// </summary>
    public IStaffMemberBase? StaffMember { get; set; }

    /// <summary>
    /// All activities of the staff members for particular month of the particular year.
    /// </summary>
    public IList<IYearMonthStaffActivityBase>? Activities {get; set;}
}
