using Alman.SharedDefinitions;
using Alman.SharedModels;
using DbAccess.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AlmanUI.Controls;

/// <summary>
/// API for for managing the data n StaffActivitiesViewModel.
/// </summary>
public class StaffActivitiesControl : ControlBase<StaffActivity, IStaffActivityBase>
{
    /// <summary>
    /// Save items based on the read-only list <paramref name="itemsToSave"/>. Delete, update, add.
    /// </summary>
    /// <param name="itemsToSave">List of items that was modified bu user.</param>
    /// <param name="itemsIdsToDelete">Ids of the items that must be deleted from database.</param>
    /// <returns>RetuenCode.OK if successfully saved, ERR otherwise.</returns>

    public static ReturnCode SaveItems(IReadOnlyList<IStaffActivityBase> itemsToSave, IList<int> itemsIdsToDelete)
    {
        ReturnCode retCode = ReturnCode.OK;
        if (itemsIdsToDelete.Count > 0)
        {
            retCode = DeleteItems(itemsIdsToDelete);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Something went wrong wile deleting {nameof(IStaffActivityBase)}'s.");
                return retCode;
            }
        }

        var staffActivitiesFromDb = GetItems();
        int dbCount = staffActivitiesFromDb.Count;
        int difference = itemsToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedStaffActivities = itemsToSave.Where(act => act.InGroup(staffActivitiesFromDb)).ToList();
            retCode = UpdateItems(updatedStaffActivities);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with updating {nameof(IStaffActivityBase)}");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newStaffActivities = itemsToSave.Where(act => !act.InGroup(staffActivitiesFromDb)).ToList();
            retCode = AddItems(newStaffActivities);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with adding {nameof(IStaffActivityBase)}");
            }
        }

        return retCode;
    }
}


public static class StaffActivityBaseExtensions
{
    public static bool DbEquals(this IStaffActivityBase item,  IStaffActivityBase other)
    {
        if (item.Id != other.Id) return false;
        return true;
    }

    public static bool InGroup(this IStaffActivityBase item, IReadOnlyCollection<IStaffActivityBase> group) 
    {
        if (group.Count == 0) return false;
        var itemInGroup = group.SingleOrDefault(act => act.Id == item.Id);
        if (itemInGroup is null) return false;
        return true;
    }
}


