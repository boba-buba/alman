using Alman.SharedModels;
using Alman.SharedDefinitions;
using Business;
using System.Collections.Generic;
using AlmanUI.Models;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Diagnostics;
namespace Avalonia.Controls;


public static class YearMonthActivitiesControl
{ 
    public static IReadOnlyList<IYearMonthActivityBase> GetYearMonthActivities(int year, int month)
    {
        var activiries = BusinessYearMonthActivitiesApi.GetYMActivities(year, month);
        return activiries;
    }

    private static ReturnCode AddYearMonthActivities(IReadOnlyList<IYearMonthActivityBase> ymActivities)
    {
        return BusinessYearMonthActivitiesApi.AddYearMonthActivities(ymActivities);
    }

    public static ReturnCode UpdateYearMonthActivities(IReadOnlyList<IYearMonthActivityBase> activities)
    {
        return BusinessYearMonthActivitiesApi.UpdateYMActivities(activities);
        
    }

    public static ReturnCode SaveYearMonthActivities(IReadOnlyList<IYearMonthActivityBase> ymActivitiesToSave, int year, int month)
    {
        ReturnCode retCode = ReturnCode.ERR;
        var ymActivitiesFromDb = BusinessYearMonthActivitiesApi.GetYMActivities(year, month);
        int dbCount = ymActivitiesFromDb.Count;
        int difference = ymActivitiesToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedYMActivities = ymActivitiesToSave.Where(ymAct => ymAct.InGroup(ymActivitiesFromDb)).ToList();

            retCode = BusinessYearMonthActivitiesApi.UpdateYMActivities(updatedYMActivities);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with updating {nameof(IYearMonthActivityBase)}");
                return retCode;
            }
        }

        if (difference > 0)
        {

            var newYMActivities = ymActivitiesToSave.Where(ymAct => !ymAct.InGroup(ymActivitiesFromDb)).ToList();
            retCode = AddYearMonthActivities(newYMActivities);
        }

        return retCode;
    }

}


public static class YearMonthActivitiesExtensions
{
    public static bool DbEquals(this IYearMonthActivityBase item,  IYearMonthActivityBase other)
    {
        if (item.YmchildId !=  other.YmchildId) 
            return false;
        if (item.YmactivityId != other.YmactivityId) 
            return false;
        if (item.Month !=  other.Month) 
            return false;
        if (item.Year != other.Year) 
            return false;
        return true;
    }

    public static bool InGroup(this IYearMonthActivityBase item, IReadOnlyList<IYearMonthActivityBase> group)
    {
        foreach (var groupItem in group)
        {
            if (item.DbEquals(groupItem)) 
                return true;
        }
        return false;
    }
}