using Alman.SharedModels;
using System;
using System.Collections.Generic;
using Business;

namespace AlmanUI.Controls;


public static class ChildrenControl
{
    public static IReadOnlyList<IChildBase> GetChildren()
    {
        return BusinessChildrenApi.GetChildren();
    }

    public static IReadOnlyList<IChildBase> GetChildrenOnCondition(Func<IChildBase, bool> selector)
    {
        return BusinessChildrenApi.GetChildrenByFilter(selector);
    }

    public static IReadOnlyList<IChildBase> GetChildrenByFilter(Func<IChildBase, bool> filter) =>
        BusinessChildrenApi.GetChildrenByFilter(filter);
 
}