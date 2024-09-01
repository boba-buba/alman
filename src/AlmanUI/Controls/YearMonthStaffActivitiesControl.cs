using Alman.SharedDefinitions;
using Alman.SharedModels;
using DbAccess.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AlmanUI.Controls;

/// <summary>
/// API for managing data in YearMonthStaffActivitiesViewModel.
/// </summary>
public class YearMonthStaffActivitiesControl : ControlBase<YearMonthStaffActivity, IYearMonthStaffActivityBase>
{
    /// <summary>
    /// Save items based on the read-only list <paramref name="itemsToSave"/>. Delete, update, add.
    /// </summary>
    /// <param name="itemsToSave">List of items that was modified bu user.</param>
    /// <param name="year">Year for which tha data are saved.</param>
    /// <param name="month">Month during which the data are saved.</param>
    /// <returns>RetuenCode.OK if successfully saved, ERR otherwise.</returns>
    public static ReturnCode SaveItems(IReadOnlyList<IYearMonthStaffActivityBase> itemsToSave, int year, int month)
    {
        ReturnCode retCode = ReturnCode.OK;
        var ymStaffActivitiesFromDb = GetItemsByFilter(act => act.Year ==  year && act.Month == month);
        int dbCount = ymStaffActivitiesFromDb.Count;
        int difference = itemsToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedYMStaffActivities = itemsToSave.Where(item => item.InGroupId(ymStaffActivitiesFromDb)).ToList();
            retCode = UpdateItems(updatedYMStaffActivities);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with updating {nameof(IYearMonthStaffActivityBase)}");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newYMStaffActivities = itemsToSave.Where(item => !item.InGroup(ymStaffActivitiesFromDb)).ToList();
            retCode = AddItems(newYMStaffActivities);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with adding {nameof(IYearMonthStaffActivityBase)}");
            }
        }
        return retCode;
    }
}


public static class YearMonthStaffActivitiesBaseExtensions
{
    public static bool DbEquals(this IYearMonthStaffActivityBase item, IYearMonthStaffActivityBase other)
    {
        if (item.StaffMemberId != other.StaffMemberId) return false;
        if (item.Year != other.Year) return false;
        if (item.Month != other.Month) return false;
        if (item.StaffActivityId != other.StaffActivityId) return false;
        return true;
    }

    public static bool InGroup(this IYearMonthStaffActivityBase item, IReadOnlyCollection<IYearMonthStaffActivityBase> group)
    {
        if (group.Count == 0) return false;
        var itemInGroup = group.SingleOrDefault(it => it.DbEquals(item));
        if (itemInGroup == null) return false;
        return true;
    }
}
    