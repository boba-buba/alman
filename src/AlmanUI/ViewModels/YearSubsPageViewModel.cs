using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Models;
using AlmanUI.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.ViewModels;

public partial class YearSubsPageViewModel : ViewModelBase
{
    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;

    public YearSubsPageViewModel() { }

    [RelayCommand]
    public void TriggerPrevYearCommand()
    {
        CurrentYear -= 1;
        Mediator.Mediator.Instance.SendWithOneParam("UpdateYearSubsMainDataGrid", CurrentYear);
    }

    [RelayCommand]
    public void TriggerNextYearCommand()
    {
        CurrentYear += 1;
        Mediator.Mediator.Instance.SendWithOneParam("UpdateYearSubsMainDataGrid", CurrentYear);
    }


    [RelayCommand]
    public void TriggerSaveCommand(IReadOnlyList<YearSubCompositeItem> items)
    {
        if (items.Count == 0) return;
        List<IYearSubBase> yearSubs = new List<IYearSubBase>();

        foreach (var item in items)
        {
            if (item.YsChild is null || item.YsYearSubscription is null)
            {
                Debug.WriteLine($"Null {nameof(item.YsChild)} or {nameof(item.YsYearSubscription)}");
                continue;
            }

            yearSubs.Add(item.YsYearSubscription);
        }

        ReturnCode retCode = YearSubsControl.SaveYearSubs(yearSubs);
        if (retCode is not ReturnCode.OK)
        {
            Debug.WriteLine($"Something went wrong saving {nameof(YearSubUI)}'s. Changes were not saved.");
        }
    }
}
