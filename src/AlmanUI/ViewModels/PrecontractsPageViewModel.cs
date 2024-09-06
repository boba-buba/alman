using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using AlmanUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace AlmanUI.ViewModels;

/// <summary>
/// ViewModel for fetching and managing the data for Precontracts.
/// </summary>
public partial class PrecontractsPageViewModel : ViewModelBase, IMonthButtons, ILoadItems
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
    /// Children table read from the database.
    /// </summary>
    private IReadOnlyList<IChildBase>? _childrenTable;

    /// <summary>
    /// Precontracts table read from the database.
    /// </summary>
    private IReadOnlyList<IPrecontractBase>? _precontractsTable;

    /// <summary>
    /// Collection of the items to be shown in UI view.
    /// </summary>
    public ObservableCollection<PrecontractCompositeItem>? ChildPrecontracts { get; set; }

    public void LoadItems()
    {

        _childrenTable = ChildrenControl.GetItemsByFilter(ch =>
            ch.ChildStartYear == CurrentYear &&
            ch.ChildStartMonth == CurrentMonth);

        if (_childrenTable is null)
        {
            return;
        }

        _precontractsTable = PrecontractsControl.GetItemsByFilter(pr =>
            pr.PYear == CurrentYear &&
            pr.PMonth == CurrentMonth);

        if (ChildPrecontracts is null)
        {
            ChildPrecontracts = new();
        }
        else if (ChildPrecontracts.Count > 0)
        {
            ChildPrecontracts.Clear();
        }

        foreach (var child in _childrenTable)
        {
            var newItem = new PrecontractCompositeItem { PChild = child };
            IPrecontractBase? newItemPrecontract = _precontractsTable.SingleOrDefault(pr => pr.PchildId == child.Id);

            if (_precontractsTable.Count == 0 || newItemPrecontract == null)
            {
                newItemPrecontract = new PrecontractUI { PchildId = child.Id, PMonth = child.ChildStartMonth, PYear = child.ChildStartYear };

            }

            newItem.Precontract = newItemPrecontract;
            ChildPrecontracts.Add(newItem);
        }


    }


    /// <summary>
    /// ctor.
    /// </summary>
    public PrecontractsPageViewModel() { LoadItems(); }


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
        Mediator.Mediator.Instance.Send("UpdatePrecontractsMainDataGrid");

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
        Mediator.Mediator.Instance.Send("UpdatePrecontractsMainDataGrid");
    }

    /// <summary>
    /// Save the modified UI vIew table and fetch the latest data from the database.
    /// </summary>
    [RelayCommand]
    public void TriggerSaveCommand()
    {
        if (ChildPrecontracts is null || ChildPrecontracts.Count == 0) { return; }
        List<IPrecontractBase> precontracts = new List<IPrecontractBase>();
        foreach (var item in ChildPrecontracts)
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
        LoadItems();
        Mediator.Mediator.Instance.Send("UpdatePrecontractsMainDataGrid");
    }
}

