using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using AlmanUI.Views;
using AlmanUI.Models;
using AlmanUI.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using System.Diagnostics;
using Alman.SharedDefinitions;

namespace AlmanUI.ViewModels;

public partial class PrecontractsPageViewModel : ViewModelBase
{
    [ObservableProperty]
    public int _currentMonth = DateTime.Now.Month;

    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;

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


    [RelayCommand]
    public void TriggerSaveCommand(IReadOnlyList<PrecontractCompositeItem> items)
    {
        if (items.Count == 0) { return; }
        List<PrecontractUI> precontracts = new List<PrecontractUI>();
        foreach (var item in items)
        {
            if (item.PChild is null || item.Precontract is null)
            {
                Debug.WriteLine($"Null {nameof(item.PChild)} or {nameof(item.Precontract)}");
                continue;
            }
            precontracts.Add(new PrecontractUI
            {
                PchildId = item.PChild.ChildId,
                Psum = item.Precontract.Psum,
                Pcomment = item.Precontract.Pcomment,
                PMonth = item.Precontract.PMonth,
                PYear = item.Precontract.PYear
            });
        }

        ReturnCode retCode = PrecontractsControl.SavePrecontracts(precontracts);
        if (retCode != ReturnCode.OK)
        {
            Debug.WriteLine($"Something went wrong saving {nameof(PrecontractUI)}'s. Changes were not saved.");
        }
    }
     
}

