using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Models;
using DbAccess.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace AlmanUI.Controls;

/// <summary>
/// API for HomeViewModel.
/// </summary>
public static class HomeControl
{
    /// <summary>
    /// Calculate the sum of all expenses for that year.
    /// </summary>
    /// <param name="year">The year for which the calculations are provided. </param>
    /// <returns>Result sum.</returns>
    public static int CalculateExpenses(int year)
        => ExpensesControl.GetItemsByFilter(item => item.Year == year).Sum(item => item.ExpenseSum);

    public static int ClaculateYearSubsSum(int year)
        => YearSubsControl.GetItemsByFilter(item => item.Yyear == year).Sum(item => item.Payment);

}

/// <summary>
/// API for the UsersViewModel (which is not implemented yet.)
/// </summary>
public class UserUIControl : ControlBase<User, IUserBase>
{
    /// <summary>
    /// Check if the user is in database.
    /// </summary>
    /// <param name="name">Name of the user.</param>
    /// <param name="password">Password of the user.</param>
    /// <returns>True if the user is in database, false otherwise.</returns>
    public static bool UserInDb(string name, string password)
    {
        var usersFromDb = GetItemsByFilter(item => item.Name == name &&  item.Password == password);
        if (usersFromDb.Count == 1)
        {
            return true;
        }
        return false;
    }
    public static ReturnCode SaveItems(IReadOnlyList<IUserBase> itemsToSave, IList<int> itemsIdsToDelete)
    {
        ReturnCode retCode = ReturnCode.OK;

        if (itemsIdsToDelete.Count > 0)
        {
            if (itemsIdsToDelete.Contains(1))
            {
                Debug.WriteLine("Cannot delete Admin");
                return ReturnCode.ERR;
            }
            else
            {
                retCode = DeleteItems(itemsIdsToDelete);
                if (retCode != ReturnCode.OK)
                {
                    Debug.WriteLine($"Something went wrong wile deleting {nameof(IUserBase)}'s.");
                    return retCode;
                }
            }
        }

        var usersFromDb = GetItems();
        var dbCount = usersFromDb.Count;
        var difference = itemsToSave.Count - dbCount;

        var updatedUsers = itemsToSave.Where(ch => ch.InGroupId(usersFromDb)).ToList();
        if (updatedUsers.Any())
        {
            retCode = UpdateItems(updatedUsers);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Something went wrong wile updating {nameof(IUserBase)}'s.");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newUsers = itemsToSave.Where(ch => ch.Id == 0).ToList();
            retCode = AddItems(newUsers);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Something went wrong wile adding new {nameof(IUserBase)}'s.");
            }
        }
        return retCode;
    }
}

/// <summary>
/// API for calculating results for the year for HomeViewModel.
/// </summary>
public class YearResultsControl : ControlBase<YearResult, IYearResultBase>
{
    /// <summary>
    /// Get the remaindar for the year from the database, if not in database create one and set to 0.
    /// </summary>
    /// <param name="year">Year the remainder for which is asked.</param>
    /// <returns>Remainder sum.</returns>
    public static int GetYearRemainder(int year)
    {
        var yearRemainders = GetItemsByFilter(item => year == item.Year);
        if (yearRemainders.Count == 0)
        {
            AddItems(new[] { new YearResultUI { Year = year, YearRemainder = 0 } });
            return 0;
        }
        return yearRemainders.Single().YearRemainder;
    }

    /// <summary>
    /// Calculate the remainder of the year from all incomes and expenses and write the value to te database.
    /// </summary>
    /// <param name="year">The year for which all the data are collected for the calculation.</param>
    /// <returns>ReturnCode.OK if successfully calculated and written, ERR otherwise.</returns>
    public static ReturnCode CalculateYearRemainder(int year)
    {
        int lastYearRemainder = GetYearRemainder(year - 1);
        var yearSubsSum = HomeControl.ClaculateYearSubsSum(year);

        var yearExpenses = HomeControl.CalculateExpenses(year);

        int yearRemainder = lastYearRemainder + yearSubsSum - yearExpenses;

        var yearResultFromDb = GetItemsByFilter(item => item.Year == year).SingleOrDefault();
        if (yearResultFromDb is null)
        {
            AddItems([new YearResultUI { Year = year, YearRemainder = 0}]);
            yearResultFromDb = GetItemsByFilter(item => item.Year == year).Single();
        }
        yearResultFromDb.YearRemainder = yearRemainder;

        return UpdateItems([yearResultFromDb]);
    }
    
}