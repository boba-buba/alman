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
/// View for precontracts.
/// </summary>
public partial class PrecontractsPageView : UserControl, IInitDataGrid, IUpdateDataGridWithoutParams
{
    
    /// <summary>
    /// ctor.
    /// </summary>
    public PrecontractsPageView()
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
        if (message == "UpdatePrecontractsMainDataGrid")
        {
            UpdateDataGrid();
        }
    }

    public void InitDataGrid()
    {
        PrecontractsMainDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);

        PrecontractsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.ChildrenResources.ChildFirstName, Binding = new Binding("PChild.ChildName"), IsReadOnly = true });
        PrecontractsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.ChildrenResources.ChildLastName, Binding = new Binding("PChild.ChildLastName"), IsReadOnly = true });

        UIControlElements.AddNumericTextBoxToGrid<PrecontractCompositeItem>(PrecontractsMainDataGrid, AlmanUI.Resources.CommonResources.PaidSum, "Precontract.Psum");

        PrecontractsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.CommonResources.Comment, Binding = new Binding("Precontract.Pcomment") });

        PrecontractsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.CommonResources.Month, Binding = new Binding("Precontract.PMonth"), IsReadOnly = true });
        PrecontractsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.CommonResources.Year, Binding = new Binding("Precontract.PYear"), IsReadOnly = true});

    }

    public void UpdateDataGrid()
    {
        PrecontractsMainDataGrid.Columns.Clear();
        InitDataGrid();
    }
}


