using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Resources;

namespace AlmanUI.Views;

public partial class YearMonthOtherPageView : UserControl
{
    public YearMonthOtherPageView()
    {
        InitializeComponent();
        InitDataGrid();
        
        Mediator.Mediator.Instance.Notify += OnNotify;

    }

    private void OnNotify(string message)
    {
        if (message == "UpdateYearMonthOtherDataGrid") UpdateDataGrid();
    }

    private void UpdateDataGrid()
    {
        YearMonthOtherMainDataGrid.Columns.Clear();
        InitDataGrid();
    }

    private void InitDataGrid()
    {
        YearMonthOtherMainDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        
        YearMonthOtherMainDataGrid.MinColumnWidth = 200;

        YearMonthOtherMainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.OtherResources.OtherActivityName, Binding = new Binding("OtherActivityName")});

        var weeks = new string[5] { "First", "Second", "Third", "Fourth", "Fifth" };
        for (int i = 0; i < 5; i++)
        {
            string moneyString = AlmanUI.Resources.OtherResources.ResourceManager.GetString($"{weeks[i]}Week", AlmanUI.Resources.OtherResources.Culture)!;
            string PayWay = $"PayingWay{weeks[i]}";
            UIControlElements.AddMoneyTextBox<IYearMonthOtherBase>(YearMonthOtherMainDataGrid, $"{weeks[i]}Week", PayWay, moneyString);
        }
    }

}