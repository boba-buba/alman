using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using AlmanUI.Views;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;


namespace AlmanUI.ViewModels;

public partial class YearMonthOtherPageViewModel : ViewModelBase
{
    [ObservableProperty]
    public int _currentMonth = DateTime.Now.Month; //TODO converter

    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;


    [ObservableProperty]
    private IYearMonthOtherBase? _selectedOther = null;

    private ObservableCollection<IYearMonthOtherBase> OtherActivities;
    private List<int> idsToDelete;


    public List<int> PaymentMethods { get; } = new List<int>() { (int)WayOfPaying.Cash, (int)WayOfPaying.Transfer };

    public YearMonthOtherPageViewModel()
    {
        OtherActivities = new ObservableCollection<IYearMonthOtherBase>(
            YearMonthOtherControl.GetItemsByFilter(other => other.Year == CurrentYear && other.Month == CurrentMonth));
        
        idsToDelete = new List<int>();
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
        OtherActivities = new ObservableCollection<IYearMonthOtherBase>(
            YearMonthOtherControl.GetItemsByFilter(other => other.Year == CurrentYear && other.Month == CurrentMonth));

        Mediator.Mediator.Instance.SendWithParams("UpdateYearMonthOtherDataGrid", CurrentYear, CurrentMonth);
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
        OtherActivities = new ObservableCollection<IYearMonthOtherBase>(
            YearMonthOtherControl.GetItemsByFilter(other => other.Year == CurrentYear && other.Month == CurrentMonth));

        Mediator.Mediator.Instance.SendWithParams("UpdateYearMonthOtherDataGrid", CurrentYear, CurrentMonth);
    }


    [RelayCommand]
    public void TriggerSaveCommand()
    {
        var retCode = YearMonthOtherControl.SaveItems(OtherActivities, idsToDelete);
        if (retCode != ReturnCode.OK)
        {
            Debug.WriteLine($"Smth went wrong saving {nameof(IYearMonthOtherBase)}'s");
            return;
        }
        idsToDelete.Clear();
        OtherActivities.Clear();
        foreach (var other in YearMonthOtherControl.GetItemsByFilter(other => other.Year == CurrentYear && other.Month == CurrentMonth))
        {
            OtherActivities.Add(other);
        }
    }

    [RelayCommand]
    public void TriggerAddNewOtherommand()
    {
        YearMonthOtherUI newOther = new YearMonthOtherUI { Month = CurrentMonth, Year = CurrentMonth, OtherActivityName = "" };
        if (OtherActivities is null)
        {
            OtherActivities = new ObservableCollection<IYearMonthOtherBase> { newOther };
        }
        else
        {
            OtherActivities.Add(newOther);
        }
        
    }

    [RelayCommand]
    public void TriggerRemoveOtherCommand()
    {
        if (SelectedOther == null)
        {
            return;
        }

        if (SelectedOther.Id != 0)
        {
            idsToDelete.Add(SelectedOther.Id);
        }
        OtherActivities.Remove(SelectedOther);
        SelectedOther = null;
        Mediator.Mediator.Instance.SendWithThreeParams("UpdateDataGrid", CurrentYear, CurrentMonth, OtherActivities);
    }

}
