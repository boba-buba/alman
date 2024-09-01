using Alman.SharedDefinitions;
using Alman.SharedModels;
using DbAccess.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AlmanUI.Controls;

/// <summary>
/// API for the ChildrenViewModel.
/// </summary>
public class ChildrenControl : ControlBase<Child, IChildBase>
{
    /// <summary>
    /// Save items based on the read-only list <paramref name="itemsToSave"/>. Delete, update, add.
    /// </summary>
    /// <param name="itemsToSave">List of items that was modified bu user.</param>
    /// <param name="itemsIdsToDelete">Ids of the items that must be deleted from database.</param>
    /// <returns>RetuenCode.OK if successfully saved, ERR otherwise.</returns>
    public static ReturnCode SaveItems(IReadOnlyList<IChildBase> itemsToSave, IList<int> itemsIdsToDelete)
    {
        ReturnCode retCode = ReturnCode.OK;
        if (itemsIdsToDelete.Any())
        {
            retCode = DeleteItems(itemsIdsToDelete);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Something went wrong wile deleting {nameof(IChildBase)}'s.");
                return retCode;
            }
        }

        var childrenFromDb = GetItems();
        var dbCount = childrenFromDb.Count;
        var difference = itemsToSave.Count - dbCount;
        var childrenIdsFromDb = (from child in childrenFromDb select child.Id).ToList();

        var updatedChildren = itemsToSave.Where(ch => childrenIdsFromDb.Contains(ch.Id)).ToList();
        if (updatedChildren.Any())
        {
            retCode = UpdateItems(updatedChildren);
            if (retCode != ReturnCode.OK )
            {
                Debug.WriteLine($"Something went wrong wile updating {nameof(IChildBase)}'s.");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newChildren = itemsToSave.Where(ch => ch.Id == 0).ToList();
            retCode = AddItems(newChildren);
            if (retCode != ReturnCode.OK ) 
            {
                Debug.WriteLine($"Something went wrong wile adding new {nameof(IChildBase)}'s.");
            }
        }
        return retCode;
    }
}
