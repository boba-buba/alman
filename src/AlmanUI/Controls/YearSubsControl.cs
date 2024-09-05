using Alman.SharedDefinitions;
using Alman.SharedModels;
using DbAccess.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AlmanUI.Controls;

/// <summary>
/// API for managing the data in YearSubsViewModel.
/// </summary>
public class YearSubsControl : ControlBase<YearSub, IYearSubBase>
{
    /// <summary>
    /// Save items based on the read-only list <paramref name="itemsToSave"/>. Delete, update, add.
    /// </summary>
    /// <param name="itemsToSave">List of items that was modified bu user.</param>
    /// <returns>RetuenCode.OK if successfully saved, ERR otherwise.</returns>
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
