using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.ViewModels;

/// <summary>
/// ViewModel for fetching and managing the data for Monthly staff activities.
/// </summary>
public partial class YearMonthStaffActivitiesPageViewModel : 
    ViewModelBase, ICurrentYear, ICurrentMonth, IMonthButtons, ISaveButtonWithParam<YearMonthStaffActivityCompositeItem>
{
    [ObservableProperty]
    public int _currentMonth = DateTime.Now.Month; //TODO converter

    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;

    /// <summary>
    /// ctor.
    /// </summary>
    public YearMonthStaffActivitiesPageViewModel() { }

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

        Mediator.Mediator.Instance.SendWithParams("UpdateYearMonthStaffActivities", CurrentYear, CurrentMonth);

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
        Mediator.Mediator.Instance.SendWithParams("UpdateYearMonthStaffActivities", CurrentYear, CurrentMonth);
    }

    [RelayCommand]
    public void TriggerSaveCommand(IReadOnlyList<YearMonthStaffActivityCompositeItem> items)
    {

        List<IYearMonthStaffActivityBase> yearMonthActivities = new List<IYearMonthStaffActivityBase>();

        foreach (var item in items)
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
        Mediator.Mediator.Instance.SendWithParams("UpdateYearMonthStaffActivities", CurrentYear, CurrentMonth);
    }
}
