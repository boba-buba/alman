using Alman.SharedDefinitions;
using Alman.SharedModels;
using DbAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlmanUI.Models;
namespace AlmanUI.Controls;

public static class HomeControl
{
    public static int CalculateExpenses(int year)
        => ExpensesControl.GetItemsByFilter(item => item.Year == year).Sum(item => item.ExpenseSum);

    public static int ClaculateYearSubsSum(int year)
        => YearSubsControl.GetItemsByFilter(item => item.Yyear == year).Sum(item => item.Payment);

}

public class UserUIControl : ControlBase<User, IUserBase>
{

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


public class YearResultsControl : ControlBase<YearResult, IYearResultBase>
{
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