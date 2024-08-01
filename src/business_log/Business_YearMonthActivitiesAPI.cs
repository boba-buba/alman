using DbAccess.Models;
using Alman.SharedDefinitions;
using DatabaseAccess;
using Alman.SharedModels;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
namespace Business;

public static class BusinessYearMonthActivitiesApi
{
    public static IReadOnlyList<IYearMonthActivityBase> GetYMActivities(int year, int month)
    {
        var db = new DbChildren();
        return db.GetYearMonthActivities(act => act.Year == year && act.Month == month);
    }

    public static ReturnCode AddYearMonthActivities(IReadOnlyList<IYearMonthActivityBase> activities)
    {
        var db = new DbChildren();
        List<YearMonthActivity> yearMonthActivities = new List<YearMonthActivity>();
        foreach (var activity in activities)
        {
            yearMonthActivities.Add(new YearMonthActivity
            {
                Year = activity.Year,
                Month = activity.Month,
                YmactivityId = activity.YmactivityId,
                YmchildId = activity.YmchildId,
                YmactivitySum = activity.YmactivitySum,
                YmwayOfPaying = activity.YmwayOfPaying,
            });
        }
        return db.AddYearMonthActivities(yearMonthActivities);
    }

    public static ReturnCode UpdateYMActivities(IReadOnlyList<IYearMonthActivityBase> activities)
    {
        var db = new DbChildren();
        List<YearMonthActivity> yearMonthActivities = new List<YearMonthActivity>();
        foreach (var activity in activities)
        {
            yearMonthActivities.Add(new YearMonthActivity
            {
                Year = activity.Year,
                Month = activity.Month,
                YmactivityId = activity.YmactivityId,
                YmchildId = activity.YmchildId,
                YmactivitySum = activity.YmactivitySum,
                YmwayOfPaying = activity.YmwayOfPaying,
            });
        }
        return db.UpdateYearMonthActivities(yearMonthActivities);
    }
}