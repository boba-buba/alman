using Alman.SharedModels;
using Business;
using System.Collections.Generic;
namespace Avalonia.Controls;


public static class YearMonthActivitiesControl
{ 
    public static IReadOnlyList<IYearMonthActivityBase> GetYearMonthActivities(int year, int month)
    {
        var activiries = BusinessYearMonthActivitiesAPI.GetYMActivities(year, month);
        return activiries;
    }
}
