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

public class PrepaymentsControl : ControlBase<Prepayment, IPrepaymentBase>
{
    public static ReturnCode SaveItems(IReadOnlyList<IPrepaymentBase> itemsToSave)
    {
        ReturnCode retCode = ReturnCode.OK;
        int year = itemsToSave[0].Year;
        int month = itemsToSave[0].Month;

        var prepaymentsFromDb = GetItemsByFilter(pr => pr.Year == year && pr.Month == month);
        int dbCount = prepaymentsFromDb.Count;
        int difference = itemsToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedPrepayments = itemsToSave.Where(pr => pr.InGroup(prepaymentsFromDb)).ToList();
            retCode = UpdateItems(updatedPrepayments);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with updating {nameof(IPrepaymentBase)}");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newPrepayments = itemsToSave.Where(pr => !pr.InGroup(prepaymentsFromDb)).ToList();
            retCode = AddItems(newPrepayments);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with adding {nameof(IPrepaymentBase)}");
            }
        }
        return retCode;
    }
}


public static class PrepaymentsBaseExtensions
{
    public static bool DbEquals(this IPrepaymentBase item, IPrepaymentBase other)
    {
        if (item.StaffMemberId != other.StaffMemberId) return false;
        if (item.Year != other.Year) return false;
        if (item.Month !=  other.Month) return false; 
        return true;
    }

    public static bool InGroup(this IPrepaymentBase item, IReadOnlyCollection<IPrepaymentBase> group) 
    {
        if (group.Count == 0) return false;
        var itemInGroup = group.SingleOrDefault(pr => pr.DbEquals(item));
        if (itemInGroup == null) return false;
        return true;
    }
}
