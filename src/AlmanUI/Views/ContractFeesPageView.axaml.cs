using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlmanUI.Views;

public partial class ContractFeesPageView : UserControl
{
    private IReadOnlyList<IChildBase>? _childrenTable;

    private IReadOnlyList<IContractFeeBase>? _contractFeesTable;

    private IReadOnlyList<ContractFeesCompositeItem>? _childContractFees;

    private void LoadItems(int year, int month)
    {
        _childrenTable = ChildrenControl.GetChildrenByFilter(ch =>
                new DateTime(ch.ChildStartYear, ch.ChildStartMonth, 1) <= new DateTime(year, month, 1));
        
        
        if (_childrenTable is null)
        {
            return;
        }

        _contractFeesTable = ContractFeesControl.GetContractFeesByFilter(cf =>
                cf.Cfyear == year &&
                cf.Cfmonth == month);
        

        var childContractFees = new List<ContractFeesCompositeItem>();

        foreach (var child in _childrenTable)
        {
            var newItem = new ContractFeesCompositeItem { CFchild = child };
            IContractFeeBase? newItemContractFee = _contractFeesTable.SingleOrDefault(cf => cf.CfchildId == child.ChildId);

            if (_contractFeesTable.Count == 0 || newItemContractFee == null)
            {
                newItemContractFee = new ContractFeeUI { CfchildId = child.ChildId, Cfmonth = month, CfsumPaid = 0, Cfyear = year };
            }
            newItem.CFcontractFee = newItemContractFee;
            childContractFees.Add(newItem);
        }

        _childContractFees = childContractFees;
    }


    public ContractFeesPageView()
    {
        LoadItems(DateTime.Now.Year, DateTime.Now.Month);
        if (_childContractFees is null)
        {
            _childContractFees = new List<ContractFeesCompositeItem>();
        }
        InitializeComponent();
        InitContractFeesMainGrid();
        Mediator.Mediator.Instance.NotifyWithParams += OnNotifyWithParams;


    }

    private void OnNotifyWithParams(string message, int year, int month)
    {
        if (message == "UpdateContractFeesMainDataGrid") UpdateContractFeesMainDataGrid(year, month);
    }
    
    private void InitContractFeesMainGrid()
    {
        ContractFeesMainDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);

        ContractFeesMainDataGrid.Columns.Add(
            new DataGridTextColumn { 
                Header = "Child name", 
                Binding = new Binding("CFchild.ChildName"), 
                IsReadOnly = true,
                //Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            });

        ContractFeesMainDataGrid.Columns.Add(
            new DataGridTextColumn { 
                Header = "Child Lastname", 
                Binding = new Binding("CFchild.ChildLastName"), 
                IsReadOnly = true,
                //Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            });

        var paidSumColumn = new DataGridTemplateColumn
        {
            Header = "Paid Sum",
            CellTemplate = new FuncDataTemplate<object>((item, namescope) =>
            {
                var textBox = new TextBox();
                textBox.Bind(TextBox.TextProperty, new Binding("CFcontractFee.CfsumPaid", BindingMode.TwoWay));
                textBox.KeyDown += TextBox_KeyDown;  // Attach the filtering function
                return textBox;
            }),
            //Width = new DataGridLength(1, DataGridLengthUnitType.Star)

        };

        ContractFeesMainDataGrid.Columns.Add(paidSumColumn);


        ContractFeesMainDataGrid.Columns.Add(
            new DataGridTextColumn { 
                Header = "Month", 
                Binding = new Binding("CFcontractFee.Cfmonth"), 
                IsReadOnly = true,
                //Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            });

        ContractFeesMainDataGrid.Columns.Add(
            new DataGridTextColumn { 
                Header = "Year", 
                Binding = new Binding("CFcontractFee.Cfyear"), 
                IsReadOnly = true,
                //Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            });

        ContractFeesMainDataGrid.ItemsSource = _childContractFees;
        SaveContractFeesButton.CommandParameter = _childContractFees;

    }

    private void UpdateContractFeesMainDataGrid(int year, int month)
    {
        LoadItems(year, month);
        ContractFeesMainDataGrid.Columns.Clear();
        InitContractFeesMainGrid();
    }

    private void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            // Allow control keys (Backspace, Delete, Arrow keys, etc.)
            if (e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.Left || e.Key == Key.Right ||
                e.Key == Key.Tab || e.Key == Key.Enter)
            {
                return;
            }

            // Check if the pressed key is a digit
            if (!(e.Key >= Key.D0 && e.Key <= Key.D9) &&
                !(e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                // If not a digit, mark the event as handled, so the key is not processed
                e.Handled = true;
            }
        }
    }
}