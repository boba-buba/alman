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
/// ViewModel for fetching and managing data for yearly subscriptions.
/// </summary>
public partial class YearSubsPageViewModel : ViewModelBase, ICurrentYear, ISaveButtonWithoutParam, IYearButtons
{

    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;
   
    /// <summary>
    /// Monthly sums for all 12 months during the year.
    /// </summary>
    public ObservableCollection<int> MonthlySum { get; private set; }

    /// <summary>
    /// Read children table from the database.
    /// </summary>
    private IReadOnlyList<IChildBase>? _childrenTable;

    /// <summary>
    /// Read from the database yearly subscriptions.
    /// </summary>
    private IReadOnlyList<IYearSubBase>? _yearSubsTable;

    /// <summary>
    /// Collection of elements to be shown in UI View.
    /// </summary>
    public ObservableCollection<YearSubCompositeItem> ChildYearSubs { get; set; }
    
    /// <summary>
    /// Fetch all necessary data from the database.
    /// </summary>
    private void LoadItems()
    {
        ChildYearSubs.Clear();
        _childrenTable = ChildrenControl.GetItemsByFilter(ch => ch.ChildStartYear <= CurrentYear);

        if (_childrenTable is null)
        {
            return;
        }

        _yearSubsTable = YearSubsControl.GetItemsByFilter(ys => ys.Yyear == CurrentYear);
        
        foreach (var child in _childrenTable)
        {
            var newItem = new YearSubCompositeItem { YsChild = child };
            
            for (int i = 0; i < (int)Months.December; i++)
            {
                IYearSubBase? newItemYearSub = _yearSubsTable.SingleOrDefault(ys => ys.YchildId == child.Id && ys.Month == i + 1);
                if (_yearSubsTable.Count == 0 || newItemYearSub is null)
                {
                    newItemYearSub = new YearSubUI { YchildId = child.Id, Yyear = CurrentYear, Month = i + 1, Payment = 0 };
                }

                newItem.YsYearSubscriptions.Add(newItemYearSub);
            }
            ChildYearSubs.Add(newItem);
        }
        ChildYearSubs.Add(CalculateSum());
        
    }
    
    /// <summary>
    /// ctor that initializes <seealso cref="MonthlySum"/>, <seealso cref="ChildYearSubs"/> and loads data from database.
    /// </summary>
    public YearSubsPageViewModel() 
    {
        MonthlySum = new ObservableCollection<int>(new int[12]);
        ChildYearSubs = new ObservableCollection<YearSubCompositeItem>();
        LoadItems();
    }

    /// <summary>
    /// Calculate sums for all 12 months for the moment.
    /// </summary>
    /// <returns></returns>
    private YearSubCompositeItem CalculateSum()
    {

        var monthSum = new YearSubCompositeItem { YsChild = new ChildUI { Id = 0, ChildLastName = "", ChildName = "" } };

        if (_yearSubsTable is null)
        {
            for (int i = 0; i < (int)Months.December; i++)
            {
                IYearSubBase subsSum = new YearSubUI { Payment = 0, Month = i + 1, Yyear = CurrentYear };
                monthSum.YsYearSubscriptions.Add(subsSum);
            }
        }
        else
        {
            for (int i = 0; i < (int)Months.December; i++)
            {
                var items = _yearSubsTable.Where(sub => sub.Month == i + 1);
                IYearSubBase subsSum = new YearSubUI { Payment = items.Sum(i => i.Payment), Month = i + 1, Yyear = CurrentYear };
                monthSum.YsYearSubscriptions.Add(subsSum);
            }
        }
        return monthSum;
    }


    [RelayCommand]
    public void TriggerPrevYearCommand()
    {
        CurrentYear -= 1;
        LoadItems();
        Mediator.Mediator.Instance.SendWithOneParam("UpdateYearSubsMainDataGrid", CurrentYear);
    }

    [RelayCommand]
    public void TriggerNextYearCommand()
    {
        CurrentYear += 1;
        LoadItems();
        Mediator.Mediator.Instance.SendWithOneParam("UpdateYearSubsMainDataGrid", CurrentYear);
    }


    [RelayCommand]
    public void TriggerSaveCommand()
    {
        if (ChildYearSubs.Count == 0) return;
        List<IYearSubBase> yearSubs = new List<IYearSubBase>();

        for (int i = 0; i < ChildYearSubs.Count - 1; i++)
        {
            if (ChildYearSubs[i].YsChild is null || ChildYearSubs[i].YsYearSubscriptions is null)
            {
                Debug.WriteLine($"Null {nameof(IChildBase)} or {nameof(List<IYearSubBase>)}");
                continue;
            }
            yearSubs.AddRange(ChildYearSubs[i].YsYearSubscriptions);
        }

        ReturnCode retCode = YearSubsControl.SaveItems(yearSubs);
        if (retCode is not ReturnCode.OK)
        {
            Debug.WriteLine($"Something went wrong saving {nameof(YearSubUI)}'s. Changes were not saved.");
        }
        LoadItems();
    }
}
