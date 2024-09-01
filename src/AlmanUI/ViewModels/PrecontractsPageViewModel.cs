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
/// ViewModel for fetching and managing the data for Precontracts.
/// </summary>
public partial class PrecontractsPageViewModel : ViewModelBase, IMonthButtons
{
    /// <summary>
    /// The month that is shown and data are fetched for.
    /// </summary>
    [ObservableProperty]
    public int _currentMonth = DateTime.Now.Month;

    /// <summary>
    /// The year that is shown and data are fetched for.
    /// </summary>
    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;

    /// <summary>
    /// ctor.
    /// </summary>
    public PrecontractsPageViewModel() { }


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

        Mediator.Mediator.Instance.SendWithParams("UpdatePrecontractsMainDataGrid", CurrentYear, CurrentMonth);

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
        Mediator.Mediator.Instance.SendWithParams("UpdatePrecontractsMainDataGrid", CurrentYear, CurrentMonth);
    }

    /// <summary>
    /// Save the modified UI vIew table and fetch the latest data from the database.
    /// </summary>
    /// <param name="items">Modified UI table.</param>
    [RelayCommand]
    public void TriggerSaveCommand(IReadOnlyList<PrecontractCompositeItem> items)
    {
        if (items.Count == 0) { return; }
        List<IPrecontractBase> precontracts = new List<IPrecontractBase>();
        foreach (var item in items)
        {
            if (item.PChild is null || item.Precontract is null)
            {
                Debug.WriteLine($"Null {nameof(item.PChild)} or {nameof(item.Precontract)}");
                continue;
            }
            precontracts.Add(item.Precontract);
        }

        ReturnCode retCode = PrecontractsControl.SaveItems(precontracts);
        if (retCode != ReturnCode.OK)
        {
            Debug.WriteLine($"Something went wrong saving {nameof(PrecontractUI)}'s. Changes were not saved.");
        }
        Mediator.Mediator.Instance.SendWithParams("UpdatePrecontractsMainDataGrid", CurrentYear, CurrentMonth);
    }
}

