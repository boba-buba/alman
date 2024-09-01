using Alman.SharedDefinitions;
using Alman.SharedModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace AlmanUI.Controls;


/// <summary>
/// API for the viewmodel. Intermediate layer between business logic and ViewModels.
/// </summary>
public class ActivitiesControl : ControlBase<DbAccess.Models.Activity, IActivityBase>
{    
    /// <summary>
    /// Save items based on the read-only list <paramref name="itemsToSave"/>. Delete, update, add.
    /// </summary>
    /// <param name="itemsToSave">List of items that was modified bu user.</param>
    /// <param name="activitiesIdsToDelete">Ids of the items that must be deleted from database.</param>
    /// <returns></returns>
    public static ReturnCode SaveItems(IReadOnlyList<IActivityBase> itemsToSave, IList<int> activitiesIdsToDelete)
    {
        ReturnCode retCode = ReturnCode.OK;

        if (activitiesIdsToDelete.Count > 0)
        {
            retCode = DeleteItems(activitiesIdsToDelete);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Something went wrong wile deleting {nameof(IActivityBase)}'s.");
                return retCode;
            }
        }

        var activitiesFromDb = GetItems();
        int dbCount = activitiesFromDb.Count;
        int difference = itemsToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedActivities = itemsToSave.Where(act => act.InGroupId(activitiesFromDb)).ToList();
            retCode = UpdateItems(updatedActivities);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Something went wrong wile updating {nameof(IActivityBase)}'s.");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newActivities = itemsToSave.Where(act => !act.InGroupId(activitiesFromDb)).ToList();
            retCode = AddItems(newActivities);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with adding {nameof(IActivityBase)}");
            }
        }
        
        return retCode;
    }
}

public static class ActivitiesBase
{
    public static bool DbEquals(this IActivityBase item, IActivityBase other)
    {
        if (item.Id != other.Id) return false;
        return true;
    }
    
    public static bool InGroup(this IActivityBase item, IReadOnlyCollection<IActivityBase> group)
    {
        if (group.Count == 0) return false;
        var itemInGroup = group.SingleOrDefault(act => act.Id == item.Id);
        if (itemInGroup is null) return false;
        return true;
    }
}