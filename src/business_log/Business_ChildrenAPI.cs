using DbAccess.Models;
using DatabaseAccess;
using Alman.SharedModels;
using System.Runtime.CompilerServices;
namespace Business;

public static class BusinessChildrenAPI
{
    public static IReadOnlyList<IChildBase> GetChildren()
    {
        var db = new DbChildren();
        return db.GetChildren(ch => true);
    }

    public static void AddChildren(IReadOnlyList<IChildBase> childrenToAdd)
    {
        List<Child> newChildren = new List<Child>();
        foreach (var child in childrenToAdd)
        {
            newChildren.Add(new Child { 
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
        var retCode = db.AddChildren(newChildren);
    }

    public static void SaveChildren(IReadOnlyList<IChildBase> children)
    {
        List<Child> newChildren = new List<Child>();
        foreach (var child in children)
        {
            newChildren.Add(new Child
            {
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
        var retCode = db.UpdateChildren(newChildren);
    }

}


