using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using Avalonia.Controls;
using Avalonia.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlmanUI.Views;

/// <summary>
/// View for the Contract fees.
/// </summary>
public partial class ContractFeesPageView : UserControl, ILoadItemsWithParams, IInitDataGrid, IUpdateDataGrid
{
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
    private IReadOnlyList<ContractFeeCompositeItem>? _childContractFees;

    public void LoadItems(int year, int month)
    {
        _childrenTable = ChildrenControl.GetItemsByFilter(ch =>
                new DateTime(ch.ChildStartYear, ch.ChildStartMonth, 1) <= new DateTime(year, month, 1));
        
        
        if (_childrenTable is null)
        {
            return;
        }

        _contractFeesTable = ContractFeesControl.GetItemsByFilter(cf =>
                cf.Cfyear == year &&
                cf.Cfmonth == month);
        

        var childContractFees = new List<ContractFeeCompositeItem>();

        foreach (var child in _childrenTable)
        {
            var newItem = new ContractFeeCompositeItem { CFchild = child };
            IContractFeeBase? newItemContractFee = _contractFeesTable.SingleOrDefault(cf => cf.CfchildId == child.Id);

            if (_contractFeesTable.Count == 0 || newItemContractFee == null)
            {
                newItemContractFee = new ContractFeeUI { CfchildId = child.Id, Cfmonth = month, CfsumPaid = 0, Cfyear = year };
            }
            newItem.CFcontractFee = newItemContractFee;
            childContractFees.Add(newItem);
        }
        _childContractFees = childContractFees;
    }

    /// <summary>
    /// ctor.
    /// </summary>
    public ContractFeesPageView()
    {
        LoadItems(DateTime.Now.Year, DateTime.Now.Month);
        if (_childContractFees is null)
        {
            _childContractFees = new List<ContractFeeCompositeItem>();
        }
        InitializeComponent();
        InitDataGrid();
        Mediator.Mediator.Instance.NotifyWithParams += OnNotifyWithParams;
    }

    /// <summary>
    /// Process notification that came from the Mediator.
    /// </summary>
    /// <param name="message">Message from view model.</param>
    /// <param name="year">1st param.</param>
    /// <param name="month">2nd param.</param>
    private void OnNotifyWithParams(string message, int year, int month)
    {
        if (message == "UpdateContractFeesMainDataGrid") UpdateDataGrid(year, month);
    }
    
    public void InitDataGrid()
    {
        ContractFeesMainDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);

        ContractFeesMainDataGrid.Columns.Add(
            new DataGridTextColumn { 
                Header = AlmanUI.Resources.ChildrenResources.ChildFirstName, 
                Binding = new Binding("CFchild.ChildName"), 
                IsReadOnly = true,
            });

        ContractFeesMainDataGrid.Columns.Add(
            new DataGridTextColumn { 
                Header = AlmanUI.Resources.ChildrenResources.ChildLastName, 
                Binding = new Binding("CFchild.ChildLastName"), 
                IsReadOnly = true,
            });

        UIControlElements.AddNumericTextBoxToGrid<ContractFeeCompositeItem>(ContractFeesMainDataGrid, AlmanUI.Resources.CommonResources.PaidSum, "CFcontractFee.CfsumPaid");

        ContractFeesMainDataGrid.Columns.Add(
            new DataGridTextColumn { 
                Header = AlmanUI.Resources.CommonResources.Month, 
                Binding = new Binding("CFcontractFee.Cfmonth"), 
                IsReadOnly = true,
            });

        ContractFeesMainDataGrid.Columns.Add(
            new DataGridTextColumn { 
                Header = AlmanUI.Resources.CommonResources.Year, 
                Binding = new Binding("CFcontractFee.Cfyear"), 
                IsReadOnly = true,
            });

        ContractFeesMainDataGrid.ItemsSource = _childContractFees;
        SaveContractFeesButton.CommandParameter = _childContractFees;

    }

    public void UpdateDataGrid(int year, int month)
    {
        LoadItems(year, month);
        ContractFeesMainDataGrid.Columns.Clear();
        InitDataGrid();
    }
}