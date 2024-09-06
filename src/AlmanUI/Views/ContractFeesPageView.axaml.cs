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
public partial class ContractFeesPageView : UserControl, IInitDataGrid, IUpdateDataGridWithoutParams
{
    /// <summary>
    /// ctor.
    /// </summary>
    public ContractFeesPageView()
    {
        InitializeComponent();
        InitDataGrid();
        Mediator.Mediator.Instance.Notify += OnNotify;
    }

    /// <summary>
    /// Process notification that came from the Mediator.
    /// </summary>
    /// <param name="message">Message from view model.</param>
    private void OnNotify(string message)
    {
        if (message == "UpdateContractFeesMainDataGrid") UpdateDataGrid();
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
    }

    public void UpdateDataGrid()
    {
        ContractFeesMainDataGrid.Columns.Clear();
        InitDataGrid();
    }
}