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
/// ViewModel for getting and managing data for Children Contract fees.
/// </summary>
public partial class ContractFeesPageViewModel : ViewModelBase
{
    /// <summary>
    /// The month that is shown in UI View and the data are fetched for the month.
    /// </summary>
    [ObservableProperty]
    public int _currentMonth = DateTime.Now.Month;

    /// <summary>
    /// The year that is shown in UI View and the data are fetched for the year.
    /// </summary>
    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;

    /// <summary>
    /// ctor.
    /// </summary>
    public ContractFeesPageViewModel() { }

    /// <summary>
    /// Set month to the previous. Send notification to the view to load data for new month.
    /// </summary>
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

        Mediator.Mediator.Instance.SendWithParams("UpdateContractFeesMainDataGrid", CurrentYear, CurrentMonth);
    }

    /// <summary>
    /// Set month to the next. Send notification to the view to load data for new month.
    /// </summary>
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
        Mediator.Mediator.Instance.SendWithParams("UpdateContractFeesMainDataGrid", CurrentYear, CurrentMonth);
    }

    /// <summary>
    /// Save the changes made in UI View and fetch the data from the database.
    /// </summary>
    /// <param name="items">MOdified UI table to save.</param>
    [RelayCommand]
    public void TriggerSaveCommand(IReadOnlyList<ContractFeeCompositeItem> items)
    {
        if (items.Count == 0) { return; }
        List<IContractFeeBase> contractFees = new List<IContractFeeBase>();
        foreach (var item in items)
        {
            if (item.CFchild is null || item.CFcontractFee is null)
            {
                Debug.WriteLine($"Null {nameof(item.CFchild)} or {nameof(item.CFcontractFee)}");
                continue;
            }

            contractFees.Add(item.CFcontractFee);   
        }

        ReturnCode retCode = ContractFeesControl.SaveItems(contractFees);
        if (retCode != ReturnCode.OK)
        {
            Debug.WriteLine($"Something went wrong saving {nameof(ContractFeeUI)}'s. Changes were not saved.");
        }

        Mediator.Mediator.Instance.SendWithParams("UpdateContractFeesMainDataGrid", CurrentYear, CurrentMonth);

    }
}
