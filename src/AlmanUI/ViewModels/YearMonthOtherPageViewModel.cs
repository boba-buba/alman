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

namespace AlmanUI.ViewModels;

/// <summary>
/// ViewModel for fetching and managing data for Monthly other activities.
/// </summary>
public partial class YearMonthOtherPageViewModel : ViewModelBase, IMonthButtons, ISaveButtonWithoutParam, IAddRemoveButtons, ICurrentMonth, ICurrentYear
{
    [ObservableProperty]
    public int _currentMonth = DateTime.Now.Month; //TODO converter

    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;

    /// <summary>
    /// Selected row in UI table view.
    /// </summary>
    [ObservableProperty]
    private IYearMonthOtherBase? _selectedOther = null;

    /// <summary>
    /// Read OtherActivities table from the database.
    /// </summary>
    public ObservableCollection<IYearMonthOtherBase> OtherActivities { get; set; }

    /// <summary>
    /// Ids of the items to delete from database.
    /// </summary>
    private List<int> _idsToDelete;

    /// <summary>
    /// Fetch all necessary data from that databse.
    /// </summary>
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

    /// <summary>
    /// ctor that initializes <seealso cref="_idsToDelete"/> and <seealso cref="OtherActivities"/>.
    /// </summary>
    public YearMonthOtherPageViewModel()
    {
        _idsToDelete = new List<int>();
        OtherActivities = new ObservableCollection<IYearMonthOtherBase>();
        LoadItems();
    }


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
        Mediator.Mediator.Instance.Send("UpdateYearMonthOtherDataGrid");
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
