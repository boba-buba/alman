using Alman.SharedDefinitions;
using Alman.SharedModels;
using DbAccess.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AlmanUI.Controls;

//Delete here not necessary because there is 1 precontract for every child 
// and it cannot be deleted

/// <summary>
/// API for the PrecontractsViewModel.
/// </summary>
public class PrecontractsControl : ControlBase<Precontract, IPrecontractBase>
{
    /// <summary>
    /// Save items based on the read-only list <paramref name="itemsToSave"/>. Delete, update, add.
    /// </summary>
    /// <param name="itemsToSave">List of items that was modified bu user.</param>
    /// <returns>RetuenCode.OK if successfully saved, ERR otherwise.</returns>
    public static ReturnCode SaveItems(IReadOnlyList<IPrecontractBase> itemsToSave)
    {
        ReturnCode retCode = ReturnCode.OK;
        int? year = itemsToSave[0].PYear;
        int? month = itemsToSave[0].PMonth;

        var precontractsFromDb = GetItemsByFilter(pr => pr.PYear == year && pr.PMonth == month);
        int dbCount = precontractsFromDb.Count;
        int difference = itemsToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedPrecontracts = itemsToSave.Where(pr => pr.InGroup(precontractsFromDb)).ToList();
            retCode = UpdateItems(updatedPrecontracts);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with updating {nameof(IPrecontractBase)}");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newPrecontracts = itemsToSave.Where(pr => !pr.InGroup(precontractsFromDb)).ToList();
            retCode = AddItems(newPrecontracts);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with adding {nameof(IPrecontractBase)}");
            }
        }
        return retCode;
    }
}


public static class PrecontractsBaseExtensions
{
    public static bool DbEquals(this IPrecontractBase item,  IPrecontractBase other)
    {
        if (item.PchildId == other.PchildId) { return true; }
        return false;
    }

    public static bool InGroup(this IPrecontractBase item, IReadOnlyList<IPrecontractBase> group)
    {
        if (group.Count == 0) { return false; }
        var itemInGroup = group.SingleOrDefault(pr => pr.DbEquals(item));
        if (itemInGroup == null) { return false; }
        return true;
    }
}
