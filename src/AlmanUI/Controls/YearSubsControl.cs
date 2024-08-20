using Alman.SharedDefinitions;
using Alman.SharedModels;
using Business;
using DbAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.Controls;

public class YearSubsControl : ControlBase<YearSub, IYearSubBase>
{
    public static ReturnCode SaveItems(IReadOnlyList<IYearSubBase> itemsToSave)
    {
        ReturnCode retCode = ReturnCode.OK;
        int year = itemsToSave[0].Yyear;

        var yearSubsFromDb = GetItemsByFilter(ys => ys.Yyear == year);
        int dbCount = yearSubsFromDb.Count;
        int difference = itemsToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedYearSubs = itemsToSave.Where(cf => cf.InGroupId(yearSubsFromDb)).ToList();
            retCode = UpdateItems(updatedYearSubs);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with updating {nameof(IYearSubBase)}");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newYearSubs = itemsToSave.Where(cf => !cf.InGroupId(yearSubsFromDb)).ToList();
            retCode = AddItems(newYearSubs);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with adding {nameof(IYearSubBase)}");
            }
        }
        return retCode;
    }
}

public static class YearSubExtensions
{
    public static bool DbEquals(this IYearSubBase item, IYearSubBase other)
    {
        if (item.YchildId != other.YchildId) return false;
        if (item.Yyear != other.Yyear) return false;
        return true;
    }

    public static bool InGroup(this IYearSubBase item, IReadOnlyList<IYearSubBase> group)
    {
        if (group.Count == 0) { return false; }
        var itemInGroup = group.SingleOrDefault(cf => cf.DbEquals(item));
        if (itemInGroup == null) { return false; }
        return true;
    }
}


