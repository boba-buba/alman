using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DbAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.ViewModels;

public partial class FinalPaymentsPageViewModel : ViewModelBase
{
    [ObservableProperty]
    public int _currentMonth = DateTime.Now.Month;

    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;

    public FinalPaymentsPageViewModel() { }

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
    }

}
