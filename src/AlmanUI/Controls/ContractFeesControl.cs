using Alman.SharedDefinitions;
using Alman.SharedModels;
using DbAccess.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AlmanUI.Controls;

/// <summary>
/// API for ContractFeesViewModel.
/// </summary>
public class ContractFeesControl : ControlBase<ContractFee, IContractFeeBase>
{
    /// <summary>
    /// Save items based on the read-only list <paramref name="itemsToSave"/>. Delete, update, add.
    /// </summary>
    /// <param name="itemsToSave">List of items that was modified bu user.</param>
    /// <returns>RetuenCode.OK if successfully saved, ERR otherwise.</returns>
    public static ReturnCode SaveItems(IReadOnlyList<IContractFeeBase> itemsToSave)
    {
        ReturnCode retCode = ReturnCode.OK;
        int year = itemsToSave[0].Cfyear;
        int month = itemsToSave[0].Cfmonth;

        var contractFeesFromDb = GetItemsByFilter(cf => cf.Cfyear == year && cf.Cfmonth == month);
        int dbCount = contractFeesFromDb.Count;
        int difference = itemsToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedContractFees = itemsToSave.Where(cf => cf.InGroupId(contractFeesFromDb)).ToList();
            retCode = UpdateItems(updatedContractFees);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with updating {nameof(IContractFeeBase)}");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newContractFees = itemsToSave.Where(cf => !cf.InGroupId(contractFeesFromDb)).ToList();
            retCode = AddItems(newContractFees);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with adding {nameof(IContractFeeBase)}");
            }
        }
        return retCode;
    }
}


public static class ContractFeesExtensions
{
    public static bool DbEquals(this IContractFeeBase item, IContractFeeBase other)
    {
        if (item.CfchildId != other.CfchildId) { return false; }
        if (item.Cfmonth != other.Cfmonth) { return false;}
        if (item.Cfyear != other.Cfyear) { return false;}
        return true;
    }

    public static bool InGroup(this IContractFeeBase item, IReadOnlyList<IContractFeeBase> group)
    {
        if (group.Count == 0) { return false; }
        var itemInGroup = group.SingleOrDefault(cf => cf.DbEquals(item));
        if (itemInGroup == null) {  return false; }
        return true;
    }
}