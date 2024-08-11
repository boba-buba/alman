using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using AlmanUI.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.ViewModels;

public partial class PrepaymentsPageViewModel : ViewModelBase
{
    [ObservableProperty]
    public int _currentMonth = DateTime.Now.Month;

    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;

    public PrepaymentsPageViewModel() { }

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

        Mediator.Mediator.Instance.SendWithParams("UpdatePrepaymentsMainDataGrid", CurrentYear, CurrentMonth);
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
        Mediator.Mediator.Instance.SendWithParams("UpdatePrepaymentsMainDataGrid", CurrentYear, CurrentMonth);

    }


    [RelayCommand]
    public void TriggerSaveCommand(IReadOnlyList<PrepaymentCompositeItem> items)
    {
        if (items.Count == 0) { return; }
        List<IPrepaymentBase> prepayments = new List<IPrepaymentBase>();
        foreach (var item in items)
        {
            if (item.StaffMember is null || item.Prepayment is null)
            {
                Debug.WriteLine($"Null {nameof(item.StaffMember)} or {nameof(item.Prepayment)}");
                continue;
            }
            prepayments.Add(item.Prepayment);
        }
        ReturnCode retCode = PrepaymentsControl.SaveItems(prepayments);
        if (retCode != ReturnCode.OK)
        {
            Debug.WriteLine($"Something went wrong saving {nameof(PrepaymentUI)}'s. Changes were not saved.");
        }
    }
}
