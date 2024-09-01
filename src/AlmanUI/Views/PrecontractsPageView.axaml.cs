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
public partial class PrecontractsPageView : UserControl, IInitDataGrid, IUpdateDataGrid, ILoadItemsWithParams
{
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
    private IReadOnlyList<PrecontractCompositeItem>? _childPrecontracts { get; set; }

    public void LoadItems(int year, int month)
    {
        
        _childrenTable = ChildrenControl.GetItemsByFilter(ch =>
            ch.ChildStartYear == year &&
            ch.ChildStartMonth == month);

        if (_childrenTable is null)
        {
            return;
        }

        _precontractsTable = PrecontractsControl.GetItemsByFilter(pr =>
            pr.PYear == year &&
            pr.PMonth == month);


        var childPrecontracts = new List<PrecontractCompositeItem>();

        foreach (var child in _childrenTable)
        {
            var newItem = new PrecontractCompositeItem { PChild = child };
            IPrecontractBase? newItemPrecontract = _precontractsTable.SingleOrDefault(pr => pr.PchildId == child.Id);

            if (_precontractsTable.Count == 0 || newItemPrecontract == null)
            {
                newItemPrecontract = new PrecontractUI { PchildId = child.Id, PMonth = child.ChildStartMonth, PYear = child.ChildStartYear };

            }
            
            newItem.Precontract = newItemPrecontract;
            childPrecontracts.Add(newItem);
        }

        _childPrecontracts = childPrecontracts;

    }

    /// <summary>
    /// ctor.
    /// </summary>
    public PrecontractsPageView()
    {
        LoadItems(DateTime.Now.Year, DateTime.Now.Month);
        if (_childPrecontracts  is null)
        {
            _childPrecontracts = new List<PrecontractCompositeItem>();
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
        if (message == "UpdatePrecontractsMainDataGrid")
        {
            UpdateDataGrid(year, month);
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

        PrecontractsMainDataGrid.ItemsSource = _childPrecontracts;
        SavePrecontractsButton.CommandParameter = _childPrecontracts;
    }

    public void UpdateDataGrid(int year, int month)
    {
        LoadItems(year, month);
        PrecontractsMainDataGrid.Columns.Clear();
        InitDataGrid();
    }
}


