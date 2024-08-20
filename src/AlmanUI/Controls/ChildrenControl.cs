using Alman.SharedModels;
using System;
using DbAccess.Models;
using System.Collections.Generic;
using Business;
using Alman.SharedDefinitions;
using System.Diagnostics;
using System.Linq;

namespace AlmanUI.Controls;

public class ChildrenControl : ControlBase<Child, IChildBase>
{
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
