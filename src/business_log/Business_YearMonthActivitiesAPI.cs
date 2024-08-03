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

    public static ReturnCode DeleteYMACtivities(IList<int> yMActivitiesIds)
    {
        var db = new DbChildren();
        var yMActivitiesToDelete = db.GetYearMonthActivities(act => yMActivitiesIds.Contains(act.YmactivityId));
        return db.DeleteYearMonthActivities(yMActivitiesToDelete);
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

    public static ReturnCode UpdateYMActivities(IReadOnlyList<IYearMonthActivityBase> updatedYMActivities)
    {
        var db = new DbChildren();
        if (updatedYMActivities.Count == 0)
        {
            return ReturnCode.OK;
        }
        int year = updatedYMActivities[0].Year;
        int month = updatedYMActivities[0].Month;

        var yMActivitesToUpdate = db.GetYearMonthActivities(act => act.Year == year && act.Month == month);
        foreach (var yMActivity in yMActivitesToUpdate)
        {
            var updatedYMActivity = updatedYMActivities.Single(act =>
                act.YmchildId == yMActivity.YmchildId &&
                act.YmactivityId == yMActivity.YmactivityId &&
                act.Month == yMActivity.Month &&
                act.Year == yMActivity.Year);
            yMActivity.YmwasPaid = updatedYMActivity.YmwasPaid;
            yMActivity.YmwayOfPaying = updatedYMActivity.YmwayOfPaying;
            yMActivity.YmactivitySum = updatedYMActivity.YmactivitySum;
        }

        return db.UpdateYearMonthActivities(yMActivitesToUpdate);
    }
}