using DbAccess.Models;
using DatabaseAccess;
using Alman.SharedModels;
using Alman.SharedDefinitions;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
namespace Business;

public static class BusinessYearMonthActivitiesAPI
{
    public static IReadOnlyList<IYearMonthActivityBase> GetYMActivities(int year, int month)
    {
        var db = new DbChildren();
        return db.GetYearMonthActivities(act => act.Year == year && act.Month == month);
    }
}