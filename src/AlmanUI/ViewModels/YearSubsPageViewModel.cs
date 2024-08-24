using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

namespace AlmanUI.ViewModels;

public partial class YearSubsPageViewModel : ViewModelBase
{

    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;
   

    public ObservableCollection<int> MonthlySum { get; private set; }

    private IReadOnlyList<IChildBase>? _childrenTable;

    private IReadOnlyList<IYearSubBase>? _yearSubsTable;

    public ObservableCollection<YearSubCompositeItem> ChildYearSubs { get; set; }
    
    private void LoadItems()
    {
        ChildYearSubs.Clear();
        _childrenTable = ChildrenControl.GetItemsByFilter(ch => ch.ChildStartYear <= CurrentYear);

        if (_childrenTable is null)
        {
            return;
        }

        _yearSubsTable = YearSubsControl.GetItemsByFilter(ys => ys.Yyear == CurrentYear);
        
        foreach (var child in _childrenTable)
        {
            var newItem = new YearSubCompositeItem { YsChild = child };
            
            for (int i = 0; i < 12; i++)
            {
                IYearSubBase? newItemYearSub = _yearSubsTable.SingleOrDefault(ys => ys.YchildId == child.Id && ys.Month == i + 1);
                if (_yearSubsTable.Count == 0 || newItemYearSub is null)
                {
                    newItemYearSub = new YearSubUI { YchildId = child.Id, Yyear = CurrentYear, Month = i + 1, Payment = 0 };
                }

                newItem.YsYearSubscriptions.Add(newItemYearSub);
            }
            ChildYearSubs.Add(newItem);
        }
        CalculateSum();
        
    }
    
    public YearSubsPageViewModel() 
    {
        MonthlySum = new ObservableCollection<int>(new int[12]);
        ChildYearSubs = new ObservableCollection<YearSubCompositeItem>();
        LoadItems();
        ChildYearSubs.CollectionChanged += (s, e) => CalculateSum();
    }

    private void CalculateSum()
    {
        if (_yearSubsTable is null)
        {
            return;
        }
        for (int i = 0; i < 12; i++)
        {
            var items = _yearSubsTable.Where(sub => sub.Month == i + 1);
            int monthSum = items.Sum(i => i.Payment);
            MonthlySum[i] = monthSum;

        }
    }

    [RelayCommand]
    public void TriggerPrevYearCommand()
    {
        CurrentYear -= 1;
        LoadItems();
        Mediator.Mediator.Instance.SendWithOneParam("UpdateYearSubsMainDataGrid", CurrentYear);
    }

    [RelayCommand]
    public void TriggerNextYearCommand()
    {
        CurrentYear += 1;
        LoadItems();
        Mediator.Mediator.Instance.SendWithOneParam("UpdateYearSubsMainDataGrid", CurrentYear);
    }


    [RelayCommand]
    public void TriggerSaveCommand()
    {
        if (ChildYearSubs.Count == 0) return;
        List<IYearSubBase> yearSubs = new List<IYearSubBase>();

        foreach (var item in ChildYearSubs)
        {
            if (item.YsChild is null || item.YsYearSubscriptions is null)
            {
                Debug.WriteLine($"Null {nameof(item.YsChild)} or {nameof(item.YsYearSubscriptions)}");
                continue;
            }
            yearSubs.AddRange(item.YsYearSubscriptions);
        }

        ReturnCode retCode = YearSubsControl.SaveItems(yearSubs);
        if (retCode is not ReturnCode.OK)
        {
            Debug.WriteLine($"Something went wrong saving {nameof(YearSubUI)}'s. Changes were not saved.");
        }
        CalculateSum();
    }
}
