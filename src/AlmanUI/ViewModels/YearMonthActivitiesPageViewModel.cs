using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace AlmanUI.ViewModels;



/// <summary>
/// ViewModel for fetching and managing the data for YearMonthActivities.
/// </summary>
public partial class YearMonthActivitiesPageViewModel : ViewModelBase, IMonthButtons, ISaveButtonWithParam<YearMonthActivityCompositeItem>, ICurrentMonth
{
    
    [ObservableProperty]
    public int _currentMonth = DateTime.Now.Month; //TODO converter

    /// <summary>
    /// The year that is shown and data are fetched for.
    /// </summary>
    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;

    

    /// <summary>
    /// ctor.
    /// </summary>
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
    public void TriggerSaveCommand(IReadOnlyList<YearMonthActivityCompositeItem> items)
    {
       
        List<IYearMonthActivityBase> yearMonthActivities = new List<IYearMonthActivityBase>();

        foreach (var item in items)
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
    }
}

