using Alman.SharedDefinitions;
using Alman.SharedModels;
using DbAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.Controls;

public class YearMonthOtherControl : ControlBase<YearMonthOther, IYearMonthOtherBase>
{
    public static ReturnCode SaveItems(IReadOnlyList<IYearMonthOtherBase> itemsToSave, IList<int> itemsIdsToDelete)
    {
        ReturnCode retCode = ReturnCode.OK;
        if (itemsToSave.Count == 0)
        {
            return retCode;
        }
        int year = itemsToSave[0].Year;
        int month = itemsToSave[0].Month;
        var yearMonthOthersFromDb = GetItemsByFilter(ymOther => ymOther.Year == year && ymOther.Month == month).ToList();

        int dbCount = yearMonthOthersFromDb.Count;
        int difference = itemsToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedOther = itemsToSave.Where(other => other.InGroup(yearMonthOthersFromDb)).ToList();
            retCode = UpdateItems(updatedOther);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with updating {nameof(IYearMonthOtherBase)}");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newOthers = itemsToSave.Where(other => !other.InGroup(yearMonthOthersFromDb)).ToList();
            retCode = AddItems(newOthers);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with adding {nameof(IYearMonthOtherBase)}");
            }
        }
        return retCode;
    }
}

public static class YearMonthOtherBaseExtensions
{
    public static bool DbEquals(this IYearMonthOtherBase item, IYearMonthOtherBase other)
    {
        if (item.OtherActivityName != other.OtherActivityName) return false;
        if (item.Year != other.Year) return false;
        if (item.Month != other.Month) return false;
        return true;
    }

    public static bool InGroup(this IYearMonthOtherBase item, ICollection<IYearMonthOtherBase> group)
    {
        if (group.Count == 0) return false;
        var itemInGroup = group.SingleOrDefault(it => it.DbEquals(item));
        if (itemInGroup is null) return false;
        return true;
    }
}
