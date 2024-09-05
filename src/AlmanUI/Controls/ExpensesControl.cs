using Alman.SharedDefinitions;
using Alman.SharedModels;
using DbAccess.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AlmanUI.Controls;

/// <summary>
/// API for the ExpenseViewModel.
/// </summary>
public class ExpensesControl : ControlBase<Expense, IExpenseBase>
{
    /// <summary>
    /// Save items based on the read-only list <paramref name="itemsToSave"/>. Delete, update, add.
    /// </summary>
    /// <param name="itemsToSave">List of items that was modified bu user.</param>
    /// <param name="itemsIdsToDelete">Ids of the items that must be deleted from database.</param>
    /// <returns>RetuenCode.OK if successfully saved, ERR otherwise.</returns>
    public static ReturnCode SaveItems(IReadOnlyList<IExpenseBase> itemsToSave, int year, int month, IList<int> itemsIdsToDelete)
    {
        ReturnCode retCode = ReturnCode.OK;
        if (itemsIdsToDelete.Count > 0)
        {
            retCode = DeleteItems(itemsIdsToDelete);
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
            var updatedExpenses = itemsToSave.Where(exp => exp.InGroupId(expensesFromDb)).ToList();
            retCode = UpdateItems(updatedExpenses);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with updating {nameof(IExpenseBase)}");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var addedExpenses = itemsToSave.Where(exp => !exp.InGroupId(expensesFromDb)).ToList();
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


/// <summary>
/// Provides functionality to check if the item is in the table.
/// </summary>
public static class IIdentifierExtensions
{
    /// <summary>
    /// Check if the row <paramref name="item"/> is in the table.
    /// </summary>
    /// <param name="item">The item that has the structure of the row of the table <paramref name="itemsGroup"/></param>
    /// <param name="itemsGroup">The table if the database.</param>
    /// <returns>True if the row is in the table, false otherwise.</returns>
    public static bool InGroupId(this IIdentifier item, IReadOnlyCollection<IIdentifier> itemsGroup)
    {
        if (itemsGroup.Count == 0) { return false; }
        var groupIds = (from exp in itemsGroup select exp.Id).ToList();

        if (groupIds.Contains(item.Id)) { return true; }
        return false;
    }
}