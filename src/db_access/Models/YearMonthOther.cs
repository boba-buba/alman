using Alman.SharedModels;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Models;

/// <summary>
/// Database model for the monthly other activities.
/// </summary>
public partial class YearMonthOther : IDeleteDependable, IYearMonthOtherBase
{
    public int Id { get; set; }

    public string OtherActivityName { get; set; } = "";

    public int Month { get; set; }

    public int Year { get; set; }

    public int? FirstWeek { get; set; }

    public int PayingWayFirst { get; set; }
    public int? SecondWeek { get; set; }

    public int PayingWaySecond { get; set; }

    public int? ThirdWeek { get; set; }
    public int PayingWayThird { get; set; }


    public int? FourthWeek { get; set; }

    public int PayingWayFourth { get; set; }


    public int? FifthWeek { get; set; }

    public int PayingWayFifth { get; set; }

    public void DeleteDependable(DbContext dbContext) { }
}
