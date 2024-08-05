using Alman.SharedDefinitions;
using DbAccess.Models;
using DatabaseAccess;
using Alman.SharedModels;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
namespace Business;

public static class BusinessChildrenApi
{
    public static IReadOnlyList<IChildBase> GetChildren()
    {
        var db = new DbChildren();
        return db.GetChildren(ch => true);
    }

    public static IReadOnlyList<IChildBase> GetChildrenByFilter(Func<IChildBase, bool> selector)
    {
        var db = new DbChildren();
        return db.GetChildren(selector);
    }

    public static ReturnCode DeleteChildren(IList<int> childrenIds)
    {
        var db = new DbChildren();
        var childrenToDelete = db.GetChildren(ch => childrenIds.Contains(ch.ChildId));
        return db.DeleteChildren(childrenToDelete);
    }
    

    public static ReturnCode AddChildren(IReadOnlyList<IChildBase> newChildren)
    {
        Collection<Child> children = new Collection<Child>();
        foreach (var child in newChildren)
        {
            children.Add(new Child { 
                ChildName = child.ChildName, 
                ChildLastName = child.ChildLastName, 
                ChildContract = child.ChildContract, 
                ChildGroup = child.ChildGroup,
                ChildState = child.ChildState,
                ChildStartYear = child.ChildStartYear,
                ChildStartMonth = child.ChildStartMonth
            });
        }
        var db = new DbChildren();
        return db.AddChildren(children);
    }

    public static ReturnCode UpdateChildren(IReadOnlyList<IChildBase> updatedChildren)
    {
        var db = new DbChildren();
        var childrenToUpdate = db.GetChildren(ch => true);

        foreach (var child in childrenToUpdate)
        {
            var updatedChild = updatedChildren.Single(ch => ch.ChildId ==  child.ChildId);
            child.ChildName = updatedChild.ChildName;
            child.ChildLastName = updatedChild.ChildLastName;
            child.ChildGroup = updatedChild.ChildGroup;
            child.ChildState = updatedChild.ChildState;
            child.ChildStartYear = updatedChild.ChildStartYear;
            child.ChildStartMonth = updatedChild.ChildStartMonth;
        }
        return db.UpdateChildren(childrenToUpdate);
    }

}


