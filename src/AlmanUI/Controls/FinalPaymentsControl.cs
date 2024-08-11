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

public class FinalPaymentsControl : ControlBase<FinalPayment, IFinalPaymentBase>
{
    public static ReturnCode SaveItems(IReadOnlyList<IFinalPaymentBase> itemsToSave)
    {
        ReturnCode retCode = ReturnCode.OK;
        int year = itemsToSave[0].Year;
        int month = itemsToSave[0].Month;

        var finalPayementsFromDb = GetItemsByFilter(fp => fp.Year == year && fp.Month == month).ToList();
        int dbCount = finalPayementsFromDb.Count;
        int difference = itemsToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedFinalPayments = itemsToSave.Where(fp => fp.InGroup(finalPayementsFromDb)).ToList();
            retCode = UpdateItems(updatedFinalPayments);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with updating {nameof(IFinalPaymentBase)}");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newFinalPayements = itemsToSave.Where(fp => !fp.InGroup(finalPayementsFromDb)).ToList();
            retCode = AddItems(newFinalPayements);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with adding {nameof(IFinalPaymentBase)}");
            }
        }
        return retCode;
    }
}


public static class FinalPaymentsExtensions
{
    public static bool DbEquals(this IFinalPaymentBase item,  IFinalPaymentBase other)
    {
        if (item.StaffMemberId != other.StaffMemberId) return false;
        if (item.Year != other.Year) return false;
        if (item.Month != other.Month) return false;
        return true;
    }

    public static bool InGroup(this IFinalPaymentBase item, IReadOnlyCollection<IFinalPaymentBase> group)
    {
        if (group.Count == 0) return false;
        var itemInGroup = group.SingleOrDefault(fp => fp.DbEquals(item));
        if (itemInGroup == null) return false;
        return true;
    }
}

