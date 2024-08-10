using Alman.SharedModels;
using System;
using DbAccess.Models;
using System.Collections.Generic;
using Business;
using Alman.SharedDefinitions;
using System.Diagnostics;
using System.Linq;
using Alman.SharedDefinitions;
using System.Diagnostics;
using System.Linq;

namespace AlmanUI.Controls;


public class ChildrenControl
{

    private static BusinessEntity<Child, IChildBase> BusinessLog { get; set; } = new BusinessEntity<Child, IChildBase>();

    public static IReadOnlyList<IChildBase> GetChildren() =>
        BusinessLog.GetEntities();

    public static IReadOnlyList<IChildBase> GetChildrenByFilter(Func<IChildBase, bool> selector) => 
        BusinessLog.GetEntitiesByFilter(selector);
    
    public static ReturnCode DeleteChildren(IList<int> childrenIds) =>
        BusinessLog.DeleteEntities(childrenIds);

    public static ReturnCode AddChildren(IReadOnlyList<IChildBase> children) =>
        BusinessLog.AddEntities(children);

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
        var childrenIdsFromDb = (from child in childrenFromDb select child.Id).ToList();

        var updatedChildren = childrenToSave.Where(ch => childrenIdsFromDb.Contains(ch.Id)).ToList();
        if (updatedChildren.Any())
        {
            retCode = BusinessLog.UpdateEntities(updatedChildren);
            if (retCode != ReturnCode.OK )
            {
                Debug.WriteLine($"Something went wrong wile updating {nameof(IChildBase)}'s.");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newChildren = childrenToSave.Where(ch => ch.Id == 0).ToList();
            retCode = AddChildren(newChildren);
            if (retCode != ReturnCode.OK ) 
            {
                Debug.WriteLine($"Something went wrong wile adding new {nameof(IChildBase)}'s.");
            }
        }
        return retCode;
    }

 
}