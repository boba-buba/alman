using Alman.SharedDefinitions;
using Alman.SharedModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace AlmanUI.Controls;


/// <summary>
/// API for ActivitiesViewModel.
/// </summary>
public class ActivitiesControl : ControlBase<DbAccess.Models.Activity, IActivityBase>
{    
    /// <summary>
    /// Save items based on the read-only list <paramref name="itemsToSave"/>. Delete, update, add.
    /// </summary>
    /// <param name="itemsToSave">List of items that was modified bu user.</param>
    /// <param name="itemsIdsToDelete">Ids of the items that must be deleted from database.</param>
    /// <returns>RetuenCode.OK if successfully saved, ERR otherwise.</returns>
    public static ReturnCode SaveItems(IReadOnlyList<IActivityBase> itemsToSave, IList<int> itemsIdsToDelete)
    {
        ReturnCode retCode = ReturnCode.OK;

        if (itemsIdsToDelete.Count > 0)
        {
            retCode = DeleteItems(itemsIdsToDelete);
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