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
/// ViewModel for fetching and managing Final Payments.
/// </summary>
public partial class FinalPaymentsPageViewModel : ViewModelBase
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
    public FinalPaymentsPageViewModel() { }

    /// <summary>
    /// Set month to the previous. Send notification to the view to load data for new month.
    /// </summary>
    [RelayCommand]
    public void TriggerPrevMonth()
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

        Mediator.Mediator.Instance.SendWithParams("UpdateFinalPaymentsMainDataGrid", CurrentYear, CurrentMonth);
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
        Mediator.Mediator.Instance.SendWithParams("UpdateFinalPaymentsMainDataGrid", CurrentYear, CurrentMonth);

    }

    /// <summary>
    /// Save the modified UI vIew table and fetch the latest data from the database.
    /// </summary>
    /// <param name="items">Modified UI table.</param>
    [RelayCommand]
    public void TriggerSaveCommand(IReadOnlyList<FinalPayementCompositeItem> items)
    {
        if (items.Count == 0) { return; }
        List<IFinalPaymentBase> finalPayments = new List<IFinalPaymentBase>();
        foreach (var item in items)
        {
            if (item.StaffMember is null || item.FinalPayment is null)
            {
                Debug.WriteLine($"Null {nameof(item.StaffMember)} or {nameof(item.FinalPayment)}");
                continue;
            }
            finalPayments.Add(item.FinalPayment);
        }
        ReturnCode retCode = FinalPaymentsControl.SaveItems(finalPayments);
        if (retCode != ReturnCode.OK)
        {
            Debug.WriteLine($"Something went wrong saving {nameof(FinalPaymentUI)}'s. Changes were not saved.");
        }
        Mediator.Mediator.Instance.SendWithParams("UpdateFinalPaymentsMainDataGrid", CurrentYear, CurrentMonth);

    }

}
