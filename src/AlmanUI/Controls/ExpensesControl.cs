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

public class ExpensesControl : ControlBase<Expense, IExpenseBase>
{
    public static ReturnCode SaveItems(IReadOnlyList<IExpenseBase> itemsToSave, int year, int month, IList<int> itemIdsToDelete)
    {
        ReturnCode retCode = ReturnCode.OK;
        if (itemIdsToDelete.Count > 0)
        {
            retCode = DeleteItems(itemIdsToDelete);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with deleting {nameof(IExpenseBase)}");
                return retCode;
            }
        }

        var expensesFromDb = GetItemsByFilter(exp => exp.Year == year && exp.Month == month);
        int dbCount = expensesFromDb.Count;
        int difference = itemsToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedExpenses = itemsToSave.Where(exp => exp.InGroup(expensesFromDb)).ToList();
            retCode = UpdateItems(updatedExpenses);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with updating {nameof(IExpenseBase)}");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var addedExpenses = itemsToSave.Where(exp => !exp.InGroup(expensesFromDb)).ToList();
            retCode = AddItems(addedExpenses);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with adding {nameof(IExpenseBase)}");
                return retCode;
            }
        }

        return retCode;
    }
}


public static class ExpensesBaseExtensions
{
    public static bool InGroup(this IExpenseBase item, IReadOnlyCollection<IExpenseBase> itemsGroup)
    {
        if (itemsGroup.Count == 0) { return false; }
        var groupIds = (from exp in itemsGroup select exp.Id).ToList();

        if (groupIds.Contains(item.Id)) { return true; }
        return false;
    }
}

public static class IIdentifierExtensions
{
    public static bool InGroupId(this IIdentifier item, IReadOnlyCollection<IIdentifier> itemsGroup)
    {
        if (itemsGroup.Count == 0) { return false; }
        var groupIds = (from exp in itemsGroup select exp.Id).ToList();

        if (groupIds.Contains(item.Id)) { return true; }
        return false;
    }
}