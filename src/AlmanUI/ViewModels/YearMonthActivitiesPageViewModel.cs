using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using AlmanUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
namespace AlmanUI.ViewModels;



/// <summary>
/// ViewModel for fetching and managing the data for YearMonthActivities.
/// </summary>
public partial class YearMonthActivitiesPageViewModel : ViewModelBase, IMonthButtons, ISaveButtonWithoutParam, ICurrentMonth, ILoadItems
{
    
    [ObservableProperty]
    public int _currentMonth = DateTime.Now.Month; //TODO converter

    /// <summary>
    /// The year that is shown and data are fetched for.
    /// </summary>
    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;

    /// <summary>
    /// Monthly activities table read from the database.
    /// </summary>
    private IReadOnlyList<IYearMonthActivityBase> _yearMonthActivitiesTable;

    /// <summary>
    /// Activities table read from the database.
    /// </summary>
    private IReadOnlyList<IActivityBase> _activitiesTable;

    /// <summary>
    /// Children table raed from the database.
    /// </summary>
    private IReadOnlyList<IChildBase> _childrenTable;

    /// <summary>
    /// Result collection that will be shown in UI view.
    /// </summary>
    public ObservableCollection<YearMonthActivityCompositeItem> YearMonthActivities { get; set; }

    public void LoadItems()
    {
        _yearMonthActivitiesTable = 
            YearMonthActivitiesControl.GetItemsByFilter(act => act.Year == CurrentYear && act.Month == CurrentMonth);
        _activitiesTable = ActivitiesControl.GetItems();

        _childrenTable = ChildrenControl.GetItemsByFilter(ch =>
                new DateTime(ch.ChildStartYear, ch.ChildStartMonth, 1) <= new DateTime(CurrentYear, CurrentMonth, 1));

        YearMonthActivities.Clear();

        foreach (var child in _childrenTable)
        {
            var newItem = new YearMonthActivityCompositeItem { YMChild = child };
            newItem.YMActivities = new List<IYearMonthActivityBase>(_yearMonthActivitiesTable.Where(act => act.YmchildId == child.Id).ToList());

            foreach (var act in _activitiesTable)
            {
                if (newItem.YMActivities is null || newItem.YMActivities.Count == 0 || newItem.YMActivities.Where(activity => activity.YmactivityId == act.Id).ToList().Count == 0)
                {
                    newItem.YMActivities!.Add(new YearMonthActivityUI
                    {
                        YmactivityId = act.Id,
                        YmchildId = newItem.YMChild!.Id,
                        Month = CurrentMonth,
                        Year = CurrentYear,
                        YmactivitySum = 0,
                        YmwasPaid = 0,
                        YmwayOfPaying = 0,
                    });
                }
            }

            YearMonthActivities.Add(newItem);
        }
    }



    /// <summary>
    /// ctor.
    /// </summary>
    public YearMonthActivitiesPageViewModel() { YearMonthActivities = new(); LoadItems(); }



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
        Mediator.Mediator.Instance.Send("UpdateDataGrid");
    }


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
        Mediator.Mediator.Instance.Send("UpdateDataGrid");
    }


    [RelayCommand]
    public void TriggerSaveCommand()
    {
       
        List<IYearMonthActivityBase> yearMonthActivities = new List<IYearMonthActivityBase>();

        foreach (var item in YearMonthActivities)
        {
            if (item.YMActivities == null || item.YMChild == null)
            {
                Debug.WriteLine($"Null {nameof(item.YMActivities)} or {nameof(item.YMChild)}");
                return;
            }
            foreach (var activity in item.YMActivities) {

                yearMonthActivities.Add(activity); 
            }
        }

        ReturnCode retCode = YearMonthActivitiesControl.SaveItems(yearMonthActivities, CurrentYear, CurrentMonth);
        if (retCode != ReturnCode.OK)
        {
            Debug.WriteLine($"Something went wrong updating {nameof(YearMonthActivityUI)}'s. Changes were not saved.");
        }
        LoadItems();
        Mediator.Mediator.Instance.Send("UpdateDataGrid");

    }
}

