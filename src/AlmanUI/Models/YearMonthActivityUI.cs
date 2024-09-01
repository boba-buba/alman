using Alman.SharedModels;
using System.Collections.Generic;
namespace AlmanUI.Models;

/// <summary>
/// UI model of the monthly activity entity.
/// </summary>
public class YearMonthActivityUI : IYearMonthActivityBase
{
    public int Id { get; set; }
    public int YmchildId { get; set; }

    public int YmactivityId { get; set; }

    public int YmactivitySum { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public int YmwayOfPaying { get; set; }

    public int YmwasPaid { get; set; }
}

/// <summary>
/// Utility class for YearMonthActivitiesView.
/// </summary>
public class YearMonthActivityCompositeItem
{
    /// <summary>
    /// Child.
    /// </summary>
    public IChildBase? YMChild { get; set; }

    /// <summary>
    /// All child's activities for the particular month for the particular year.
    /// </summary>
    public IList<IYearMonthActivityBase>? YMActivities { get; set; }
}