using Alman.SharedModels;
using System.Collections.Generic;
namespace AlmanUI.Models;

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

public class YearMonthActivityCompositeItem
{
    public IChildBase? YMChild { get; set; }
    public IList<IYearMonthActivityBase>? YMActivities { get; set; }
}