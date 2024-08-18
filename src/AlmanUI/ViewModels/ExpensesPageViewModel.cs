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
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.ViewModels;

public partial class ExpensesPageViewModel : ViewModelBase
{
    [ObservableProperty]
    public int _currentMonth = DateTime.Now.Month;

    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;

    public ObservableCollection<IExpenseBase> Expenses { get; set; }

    [ObservableProperty]
    private IExpenseBase? _selectedExpense = null;

    private IList<int> _expensesIdsToDelete;
    public ExpensesPageViewModel() 
    {
        LoadItems();   
    }

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
    }

    [RelayCommand]
    public void TriggerAddNewCommand()
    {
        IExpenseBase expense = new ExpenseUI { Name = "", ExpenseSum = 0, Month = CurrentMonth, Year = CurrentYear, WayOfPaying = 1};
        Expenses.Add(expense);
    }


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
    }


    [RelayCommand]
    public void TriggerPrevMonthCommand()
    {
        if (CurrentMonth == 1)
        {
            CurrentMonth = 12;
            CurrentYear = CurrentYear - 1;
        }
        else
        {
            CurrentMonth = CurrentMonth - 1;
        }
        LoadItems();
        Mediator.Mediator.Instance.Send("UpdateExpensesDataGrid");
    }


    [RelayCommand]
    public void TriggerNextMonthCommand()
    {
        if (CurrentMonth == 12)
        {
            CurrentMonth = 1;
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
