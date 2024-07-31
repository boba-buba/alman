﻿using Alman.SharedModels;
using AlmanUI.Controls;
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

namespace AlmanUI.ViewModels;

public partial class YearMonthActivitiesPageViewModel : ViewModelBase
{
    [ObservableProperty]
    public int _currentMonth = DateTime.Now.Month; //TODO converter

    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;

    private IReadOnlyList<IYearMonthActivityBase> _yearMonthActivities;
    private IReadOnlyList<IActivityBase> _activities;
    private IReadOnlyList<IChildBase> _children;
    public YearMonthActivitiesPageViewModel()
    {
        var yearMonthActivities = YearMonthActivitiesControl.GetYearMonthActivities(CurrentYear, CurrentMonth);
        _yearMonthActivities = yearMonthActivities;
        var activities = ActvitiesControl.GetActivities();
        _activities = activities;
        var activeChildren = ChildrenControl.GetChildrenOnCondition(ch => ch.ChildState == 1);
        _children = activeChildren;
    }

    [RelayCommand]
    public void TriggerPrevMonthCommand()
    {

    }


    [RelayCommand]
    public void TriggerNextMonthCommand()
    {

    }

    [RelayCommand]
    public void TriggerSaveCommand(IReadOnlyList<CompositeItem> items)
    {
       
        foreach (var item in items)
        {

            foreach (var act in item.YMActivities)
            { 
                Debug.WriteLine($"{item.YMChild.ChildName} {act.YmactivityId} = {act.YmactivitySum}");
            }    

        }
    }
}

