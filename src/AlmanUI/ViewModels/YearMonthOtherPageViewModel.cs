using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using AlmanUI.Views;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DbAccess.Models;
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

    public ObservableCollection<IYearMonthOtherBase> OtherActivities { get; set; }
    private List<int> _idsToDelete;

    private void LoadItems()
    {
        if (_idsToDelete is null)
        {
            _idsToDelete = new List<int>();
        }
        else
        {
            _idsToDelete.Clear();
        }
        if (OtherActivities is null)
        {
            OtherActivities = new ObservableCollection<IYearMonthOtherBase>();
        }
        else
        {
            OtherActivities.Clear();
        }

        foreach (var other in YearMonthOtherControl.GetItemsByFilter(other => other.Month == CurrentMonth && other.Year == CurrentYear))
        {
            OtherActivities.Add(other);
        }
    }

    public YearMonthOtherPageViewModel()
    {
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
        Mediator.Mediator.Instance.Send("UpdateYearMonthOtherDataGrid");
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
        Mediator.Mediator.Instance.Send("UpdateYearMonthOtherDataGrid");
    }


    [RelayCommand]
    public void TriggerSaveCommand()
    {
        var retCode = YearMonthOtherControl.SaveItems(OtherActivities, _idsToDelete);
        if (retCode != ReturnCode.OK)
        {
            Debug.WriteLine($"Smth went wrong saving {nameof(IYearMonthOtherBase)}'s");
            return;
        }
        LoadItems();
    }

    [RelayCommand]
    public void TriggerAddNewCommand()
    {
        YearMonthOtherUI newOther = new YearMonthOtherUI { Month = CurrentMonth, Year = CurrentYear, OtherActivityName = "" };
        OtherActivities.Add(newOther);
    }

    [RelayCommand]
    public void TriggerRemoveCommand()
    {
        if (SelectedOther == null)
        {
            return;
        }

        if (SelectedOther.Id != 0)
        {
            _idsToDelete.Add(SelectedOther.Id);
        }
        OtherActivities.Remove(SelectedOther);
        SelectedOther = null;
    }

}
