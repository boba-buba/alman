using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace AlmanUI.ViewModels;

/// <summary>
/// ViewModel for getting and managing data for Expenses.
/// </summary>
public partial class ExpensesPageViewModel : ViewModelBase
{
    /// <summary>
    /// The month that is shown and data are fetched for.
    /// </summary>
    [ObservableProperty]
    public int _currentMonth = DateTime.Now.Month;

    /// <summary>
    /// The year that is shownd and the data are fetched for.
    /// </summary>
    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;

    /// <summary>
    /// Read Expenses table from the database.
    /// </summary>
    public ObservableCollection<IExpenseBase> Expenses { get; set; }

    /// <summary>
    /// Overall sum of all expenses for the month.
    /// </summary>
    [ObservableProperty]
    public int _monthSum;

    /// <summary>
    /// Selected item in the UI View.
    /// </summary>
    [ObservableProperty]
    private IExpenseBase? _selectedExpense = null;

    /// <summary>
    /// Ids of the rows to be deleted from the database.
    /// </summary>
    private IList<int> _expensesIdsToDelete;

    /// <summary>
    /// ctor taht loads the items for the view.
    /// </summary>
    public ExpensesPageViewModel() 
    {
        LoadItems();   
    }

    /// <summary>
    /// Load all UI View data from the database.
    /// </summary>
    private void LoadItems()
    {
        if (_expensesIdsToDelete is null)
        {
            _expensesIdsToDelete = new List<int>();
        }
        else
        {
            _expensesIdsToDelete.Clear();
        }
        if (Expenses is null)
        {
            Expenses = new ObservableCollection<IExpenseBase>();
        }
        else
        {
            Expenses.Clear();
        }

        foreach (var exp in ExpensesControl.GetItemsByFilter(exp => exp.Month == CurrentMonth && exp.Year == CurrentYear))
        {
            Expenses.Add(exp);
        }
           
        var items = ExpensesControl.GetItemsByFilter(exp => exp.Year == CurrentYear && exp.Month == CurrentMonth && exp.WayOfPaying == (int)WayOfPaying.Cash);
        MonthSum = (from item in items select item.ExpenseSum).Sum();
    }

    /// <summary>
    /// Add new row to the UI table.
    /// </summary>
    [RelayCommand]
    public void TriggerAddNewCommand()
    {
        IExpenseBase expense = new ExpenseUI { Name = "", ExpenseSum = 0, Month = CurrentMonth, Year = CurrentYear, WayOfPaying = 1};
        Expenses.Add(expense);
    }

    /// <summary>
    /// Remove the row from the UI View table.
    /// </summary>
    [RelayCommand]
    public void TriggerRemoveCommand()
    {
        if (SelectedExpense is null)
        {
            return;
        }
        if (SelectedExpense.Id != 0)
        {
            _expensesIdsToDelete.Add(SelectedExpense.Id);
        }
        Expenses.Remove(SelectedExpense);
        SelectedExpense = null;
    }

    /// <summary>
    /// Save the modified UI View table <seealso cref="Expenses"/> and fetch the latest data from the database.
    /// </summary>
    [RelayCommand]
    public void TriggerSaveCommand()
    {
        ReturnCode retCode = ExpensesControl.SaveItems(Expenses, CurrentYear, CurrentMonth, _expensesIdsToDelete);
        if (retCode != ReturnCode.OK)
        {
            Debug.WriteLine($"Smth went wrong saving {nameof(IExpenseBase)}'s");
            return;
        }
        LoadItems();
        Mediator.Mediator.Instance.Send("UpdateExpensesDataGrid");
    }

    /// <summary>
    /// Set month to the previous. Send notification to the view to load data for new month.
    /// </summary>
    [RelayCommand]
    public void TriggerPrevMonthCommand()
    {
        if (CurrentMonth == (int)Months.January)
        {
            CurrentMonth = (int)Months.December;
            CurrentYear = CurrentYear - 1;
        }
        else
        {
            CurrentMonth = CurrentMonth - 1;
        }
        LoadItems();
        Mediator.Mediator.Instance.Send("UpdateExpensesDataGrid");
    }

    /// <summary>
    /// Set month to the next. Send notification to the view to load data for new month.
    /// </summary>
    [RelayCommand]
    public void TriggerNextMonthCommand()
    {
        if (CurrentMonth == (int)Months.December)
        {
            CurrentMonth = (int)Months.January;
            CurrentYear = CurrentYear + 1;
        }
        else
        {
            CurrentMonth = CurrentMonth + 1;
        }
        LoadItems();
        Mediator.Mediator.Instance.Send("UpdateExpensesDataGrid");
    }

}
