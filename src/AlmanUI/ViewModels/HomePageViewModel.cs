using Alman.SharedDefinitions;
using AlmanUI.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Diagnostics;

namespace AlmanUI.ViewModels;

/// <summary>
/// ViewModel for fetching and managing data for Home.
/// </summary>
public partial class HomePageViewModel : ViewModelBase
{
    /// <summary>
    /// The year that is shown and data are fetched for.
    /// </summary>
    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;

    /// <summary>
    /// Remained money from the last year.
    /// </summary>
    [ObservableProperty]
    public int _lastYearRemainder = 0;

    /// <summary>
    /// Sum of the subscriptions for the year.
    /// </summary>
    [ObservableProperty]
    public int _yearSubsSum = 0;

    /// <summary>
    /// Unused (unspent) money from this year <seealso cref="CurrentYear"/>
    /// </summary>
    [ObservableProperty]
    public int _yearRemainder = 0; // monthBalance from this year minus all expenses
    
    /// <summary>
    /// Load all neccessary data and calculation from the database.
    /// </summary>
    private void LoadItems()
    {
        LastYearRemainder = YearResultsControl.GetYearRemainder(CurrentYear - 1);
        YearSubsSum = HomeControl.ClaculateYearSubsSum(CurrentYear);
        ReturnCode retCode = YearResultsControl.CalculateYearRemainder(CurrentYear);
        if (retCode != ReturnCode.OK)
        {
            Debug.WriteLine($"Smth went wrong during calculating {nameof(YearResultsControl.CalculateYearRemainder)}");
            return;
        }
        YearRemainder = YearResultsControl.GetYearRemainder(CurrentYear);
    }
    
    /// <summary>
    /// ctor that loads data for the view.
    /// </summary>
    public HomePageViewModel()
    {
        LoadItems();
    }

    /// <summary>
    /// Set year to the previous. Load data for new yaer.
    /// </summary>
    [RelayCommand]
    public void TriggerPrevYearCommand()
    {
        CurrentYear -= 1;
        LoadItems();
    }

    /// <summary>
    /// Set year to the next. Load data for new yaer.
    /// </summary>
    [RelayCommand]
    public void TriggerNextYearCommand()
    {
        CurrentYear += 1;
        LoadItems();
    }

    /// <summary>
    /// Calculate the remainder for <seealso cref="CurrentYear"/> year.
    /// </summary>
    [RelayCommand]
    public void TriggerCalculateRemainder()
    {
        ReturnCode retCode = YearResultsControl.CalculateYearRemainder(CurrentYear);
        if (retCode != ReturnCode.OK)
        {
            Debug.WriteLine($"Smth went wrong during calculating {nameof(YearResultsControl.CalculateYearRemainder)}");
            return;
        }
        LoadItems();
    }
}
