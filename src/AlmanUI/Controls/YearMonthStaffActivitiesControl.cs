using Alman.SharedDefinitions;
using Alman.SharedModels;
using DbAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.Controls;

public class YearMonthStaffActivitiesControl : ControlBase<YearMonthStaffActivity, IYearMonthStaffActivityBase>
{
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
    