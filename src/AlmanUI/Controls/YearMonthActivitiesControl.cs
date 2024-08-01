using Alman.SharedModels;
using Alman.SharedDefinitions;
using Business;
using System.Collections.Generic;
using AlmanUI.Models;
namespace Avalonia.Controls;


public static class YearMonthActivitiesControl
{ 
    public static IReadOnlyList<IYearMonthActivityBase> GetYearMonthActivities(int year, int month)
    {
        var activiries = BusinessYearMonthActivitiesApi.GetYMActivities(year, month);
        return activiries;
    }

    public static ReturnCode UpdateYearMonthActivities(IReadOnlyList<YearMonthActivityUI> activities)
    {
        return BusinessYearMonthActivitiesApi.UpdateYMActivities(activities);
        
    }
}
