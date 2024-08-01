using Alman.SharedModels;
using AlmanUI.Controls;
using Alman.SharedDefinitions;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using AlmanUI.Views;
using System.Diagnostics;
using AlmanUI.Mediator;
using AlmanUI.Models;

namespace AlmanUI.ViewModels;

public partial class YearMonthActivitiesPageViewModel : ViewModelBase
{
    [ObservableProperty]
    public int _currentMonth = DateTime.Now.Month; //TODO converter

    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;

    
    public YearMonthActivitiesPageViewModel() {}


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

        Mediator.Mediator.Instance.SendWithParams("UpdateDataGrid", CurrentYear, CurrentMonth);
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
        Mediator.Mediator.Instance.SendWithParams("UpdateDataGrid", CurrentYear, CurrentMonth);
    }


    [RelayCommand]
    public void TriggerSaveCommand(IReadOnlyList<CompositeItem> items)
    {
       
        List<YearMonthActivityUI> yearMonthActivities = new List<YearMonthActivityUI>();

        foreach (var item in items)
        {
            if (item.YMActivities == null || item.YMChild == null)
            {
                Debug.WriteLine($"Null {nameof(item.YMActivities)} or {nameof(item.YMChild)}");
                return;
            }
            foreach (var activity in item.YMActivities) {

                yearMonthActivities.Add(new YearMonthActivityUI{
                    Year = CurrentYear,
                    Month = CurrentYear,
                    YmactivityId = activity.YmactivityId,
                    YmactivitySum = activity.YmactivitySum,
                    YmchildId = item.YMChild.ChildId,
                    YmwasPaid = activity.YmwasPaid,
                    YmwayOfPaying = activity.YmwayOfPaying,
                }); 
            }
        }

        ReturnCode retval = YearMonthActivitiesControl.UpdateYearMonthActivities(yearMonthActivities);
        if (retval != ReturnCode.OK)
        {
            Debug.WriteLine($"Something went wrong updating {nameof(YearMonthActivityUI)}'s. Changes were not saved.");
        }
    }
}


