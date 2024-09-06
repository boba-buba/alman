using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Collections.ObjectModel;
using AlmanUI.Views;

namespace AlmanUI.ViewModels;

/// <summary>
/// ViewModel for getting and managing data for Children Contract fees.
/// </summary>
public partial class ContractFeesPageViewModel : ViewModelBase, ILoadItems
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
    public ContractFeesPageViewModel() 
    {
        LoadItems();
    }

    /// <summary>
    /// Read children table from the database.
    /// </summary>
    private IReadOnlyList<IChildBase>? _childrenTable;

    /// <summary>
    /// Read contract fees table from the database.
    /// </summary>
    private IReadOnlyList<IContractFeeBase>? _contractFeesTable;

    /// <summary>
    /// Read child contracts table from the database. 
    /// </summary>
    public ObservableCollection<ContractFeeCompositeItem>? ChildContractFees { get; set; }

    public void LoadItems()
    {
        _childrenTable = ChildrenControl.GetItemsByFilter(ch =>
                new DateTime(ch.ChildStartYear, ch.ChildStartMonth, 1) <= new DateTime(CurrentYear, CurrentMonth, 1));


        if (_childrenTable is null)
        {
            return;
        }

        _contractFeesTable = ContractFeesControl.GetItemsByFilter(cf =>
                cf.Cfyear == CurrentYear &&
                cf.Cfmonth == CurrentMonth);
        if (ChildContractFees is not null && ChildContractFees.Count > 0)
        {
            ChildContractFees.Clear();

        }
        else if (ChildContractFees is null)
        {
            ChildContractFees = new ObservableCollection<ContractFeeCompositeItem>();
        }


        foreach (var child in _childrenTable)
        {
            var newItem = new ContractFeeCompositeItem { CFchild = child };
            IContractFeeBase? newItemContractFee = _contractFeesTable.SingleOrDefault(cf => cf.CfchildId == child.Id);

            if (_contractFeesTable.Count == 0 || newItemContractFee == null)
            {
                newItemContractFee = new ContractFeeUI { CfchildId = child.Id, Cfmonth = CurrentMonth, CfsumPaid = 0, Cfyear = CurrentYear };
            }
            newItem.CFcontractFee = newItemContractFee;
            ChildContractFees.Add(newItem);
        }
    }

    /// <summary>
    /// Set month to the previous. Send notification to the view to load data for new month.
    /// </summary>
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
        Mediator.Mediator.Instance.Send("UpdateContractFeesMainDataGrid");
    }

    /// <summary>
    /// Set month to the next. Send notification to the view to load data for new month.
    /// </summary>
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
        Mediator.Mediator.Instance.Send("UpdateContractFeesMainDataGrid");
    }

    /// <summary>
    /// Save the changes made in UI View and fetch the data from the database.
    /// </summary>
    [RelayCommand]
    public void TriggerSaveCommand()
    {
        if (ChildContractFees is null || ChildContractFees.Count == 0) { return; }
        List<IContractFeeBase> contractFees = new List<IContractFeeBase>();
        foreach (var item in ChildContractFees)
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
        LoadItems();
        Mediator.Mediator.Instance.Send("UpdateContractFeesMainDataGrid");

    }
}
