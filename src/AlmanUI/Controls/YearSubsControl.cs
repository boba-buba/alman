using Alman.SharedDefinitions;
using Alman.SharedModels;
using Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.Controls;

public static class YearSubsControl
{
    public static IReadOnlyList<IYearSubBase> GetYearSubs() =>
        BusinessYearSubsApi.GetYearSubs();

    public static IReadOnlyList<IYearSubBase> GetYearSubsByFilter(Func<IYearSubBase, bool> filter) =>
        BusinessYearSubsApi.GetYearSubsByFilter(filter);

    public static ReturnCode AddYearSubs(IReadOnlyList<IYearSubBase> yearSubs) =>
        BusinessYearSubsApi.AddYearSubs(yearSubs);

    public static ReturnCode UpdateYearSubs(IReadOnlyList<IYearSubBase> yearSubs) =>
        BusinessYearSubsApi.UpdateYearSubs(yearSubs);

    public static ReturnCode SaveYearSubs(IReadOnlyList<IYearSubBase> yearSubsToSave)
    {
        ReturnCode retCode = ReturnCode.OK;
        int year = yearSubsToSave[0].Yyear;

        var yearSubsFromDb = GetYearSubsByFilter(ys => ys.Yyear == year);
        int dbCount = yearSubsFromDb.Count;
        int difference = yearSubsToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedYearSubs = yearSubsToSave.Where(cf => cf.InGroup(yearSubsFromDb)).ToList();
            retCode = UpdateYearSubs(updatedYearSubs);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with updating {nameof(IYearSubBase)}");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newYearSubs = yearSubsToSave.Where(cf => !cf.InGroup(yearSubsFromDb)).ToList();
            retCode = AddYearSubs(newYearSubs);
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


