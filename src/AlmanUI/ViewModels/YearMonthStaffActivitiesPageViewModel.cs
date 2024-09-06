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
/// ViewModel for fetching and managing the data for Monthly staff activities.
/// </summary>
public partial class YearMonthStaffActivitiesPageViewModel : 
    ViewModelBase, ICurrentYear, ICurrentMonth, IMonthButtons, ISaveButtonWithoutParam, ILoadItems
{
    [ObservableProperty]
    public int _currentMonth = DateTime.Now.Month; //TODO converter

    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;

    /// <summary>
    /// Monthly activities table read from the table.
    /// </summary>
    private IReadOnlyList<IYearMonthStaffActivityBase>? _yearMonthStaffActivitiesTable;

    /// <summary>
    /// Staff activities table read from the database.
    /// </summary>
    private IReadOnlyList<IStaffActivityBase> _staffActivitiesTable;

    /// <summary>
    /// Staff members table from the database.
    /// </summary>
    private IReadOnlyList<IStaffMemberBase>? _staffMemberTable;

    /// <summary>
    /// Result collection that will be shown in UI view.
    /// </summary>
    public ObservableCollection<YearMonthStaffActivityCompositeItem>? MemberActivities { get; set; }

    public void LoadItems()
    {
        _yearMonthStaffActivitiesTable =
            YearMonthStaffActivitiesControl.GetItemsByFilter(act => act.Year == CurrentYear && act.Month == CurrentMonth);
        _staffActivitiesTable = StaffActivitiesControl.GetItems();

        DateTime now = new DateTime(CurrentYear, CurrentMonth, 1);
        _staffMemberTable = StaffMembersControl.GetItemsByFilter(mem => new DateTime(mem.StartYear, mem.StartMonth, 1) <= now);

        MemberActivities.Clear();

        foreach (var member in _staffMemberTable)
        {
            var newItem = new YearMonthStaffActivityCompositeItem { StaffMember = member };
            newItem.Activities = new List<IYearMonthStaffActivityBase>(_yearMonthStaffActivitiesTable.Where(act => act.StaffMemberId == member.Id).ToList());

            foreach (var act in _staffActivitiesTable)
            {
                
                if (newItem.Activities.Where(activity => activity.StaffActivityId == act.Id).ToList().Count == 0)
                {
                    newItem.Activities!.Add(new YearMonthStaffActivityUI
                    {
                        Year = CurrentYear,
                        Month = CurrentMonth,
                        StaffActivityId = act.Id,
                        StaffMemberId = newItem.StaffMember!.Id,
                        SumPaid = 0,
                    });
                }
            }
            MemberActivities.Add(newItem);
        }
    }

    /// <summary>
    /// ctor.
    /// </summary>
    public YearMonthStaffActivitiesPageViewModel() { MemberActivities = new(); LoadItems(); }

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
        Mediator.Mediator.Instance.Send("UpdateYearMonthStaffActivities");

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
        Mediator.Mediator.Instance.Send("UpdateYearMonthStaffActivities");
    }

    [RelayCommand]
    public void TriggerSaveCommand()
    {

        List<IYearMonthStaffActivityBase> yearMonthActivities = new List<IYearMonthStaffActivityBase>();

        foreach (var item in MemberActivities)
        {
            if (item.Activities  is null || item.StaffMember is null)
            {
                Debug.WriteLine($"Null {nameof(item.Activities)} or {nameof(item.StaffMember)}");
                return;
            }
            foreach (var activity in item.Activities)
            {

                yearMonthActivities.Add(activity);
            }
        }

        ReturnCode retCode = YearMonthStaffActivitiesControl.SaveItems(yearMonthActivities, CurrentYear, CurrentMonth);
        if (retCode != ReturnCode.OK)
        {
            Debug.WriteLine($"Something went wrong updating {nameof(YearMonthStaffActivityUI)}'s. Changes were not saved.");
        }
        LoadItems();
        Mediator.Mediator.Instance.Send("UpdateYearMonthStaffActivities");
    }
}
