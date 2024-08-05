using Alman.SharedModels;
using System;
using System.Collections.Generic;
using Business;
using Alman.SharedDefinitions;
using System.Diagnostics;
using System.Linq;

namespace AlmanUI.Controls;


public static class ChildrenControl
{
    public static IReadOnlyList<IChildBase> GetChildren() =>
        BusinessChildrenApi.GetChildren();
  

    public static IReadOnlyList<IChildBase> GetChildrenByFilter(Func<IChildBase, bool> selector) => 
        BusinessChildrenApi.GetChildrenByFilter(selector);
    
    public static ReturnCode DeleteChildren(IList<int> childrenIds) =>
        BusinessChildrenApi.DeleteChildren(childrenIds);

    public static ReturnCode AddChildren(IReadOnlyList<IChildBase> children) =>
        BusinessChildrenApi.AddChildren(children);

    public static ReturnCode SaveChildren(IReadOnlyList<IChildBase> childrenToSave, IList<int> childrenIdsToDelete)
    {
        ReturnCode retCode = ReturnCode.OK;
        if (childrenIdsToDelete.Any())
        {
            retCode = DeleteChildren(childrenIdsToDelete);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Something went wrong wile deleting {nameof(IChildBase)}'s.");
                return retCode;
            }
        }

        var childrenFromDb = GetChildren();
        var dbCount = childrenFromDb.Count;
        var difference = childrenToSave.Count - dbCount;
        var childrenIdsFromDb = (from child in childrenFromDb select child.ChildId).ToList();

        var updatedChildren = childrenToSave.Where(ch => childrenIdsFromDb.Contains(ch.ChildId)).ToList();
        if (updatedChildren.Any())
        {
            retCode = BusinessChildrenApi.UpdateChildren(updatedChildren);
            if (retCode != ReturnCode.OK )
            {
                Debug.WriteLine($"Something went wrong wile updating {nameof(IChildBase)}'s.");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newChildren = childrenToSave.Where(ch => ch.ChildId == 0).ToList();
            retCode = AddChildren(newChildren);
            if (retCode != ReturnCode.OK ) 
            {
                Debug.WriteLine($"Something went wrong wile adding new {nameof(IChildBase)}'s.");
            }
        }
        return retCode;
    }

 
}