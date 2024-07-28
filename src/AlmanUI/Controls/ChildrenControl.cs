using Alman.SharedModels;
using System;
using System.Collections.Generic;
using Business;

namespace AlmanUI.Controls;


public static class ChildrenControl
{
    public static IReadOnlyList<IChildBase> GetChildren()
    {
        return BusinessChildrenAPI.GetChildren();
    }

    public static IReadOnlyList<IChildBase> GetChildrenOnCondition(Func<IChildBase, bool> selector)
    {
        return BusinessChildrenAPI.GetChildrenOnCondition(selector);
    }
}