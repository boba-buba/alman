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

namespace AlmanUI.Views;

public partial class YearMonthOtherPageView : UserControl
{
    public YearMonthOtherPageView()
    {
        InitializeComponent();
        InitDataGrid();
        
        Mediator.Mediator.Instance.NotifyWithThreeParams += OnNotifyWithTreeParams;

    }

    private void OnNotifyWithTreeParams(string message, int  year, int month, IReadOnlyList<IYearMonthOtherBase> items)
    {
        if (message == "UpdateYearMonthOtherDataGrid") UpdateDataGrid(year, month, items);
    }

    private void UpdateDataGrid(int year, int month, IReadOnlyList<IYearMonthOtherBase> other)
    {
        YearMonthOtherMainDataGrid.Columns.Clear();
        InitDataGrid();
    }

    private void InitDataGrid()
    {
        YearMonthOtherMainDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        YearMonthOtherMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Activity Name", Binding = new Binding("OtherActivityName")});

        var weeks = new string[5] { "First", "Second", "Third", "Fourth", "Fifth" };
        for (int i = 0; i < 5; i++)
        {
            string moneyString = $"{weeks[i]}Week";
            string PayWay = $"PayingWay{weeks[i]}";
            AddMoneyTextBox<YearMonthOtherUI>(YearMonthOtherMainDataGrid, moneyString, PayWay, moneyString);
        }
        
        //YearMonthOtherMainDataGrid.ItemsSource = other;
        //SaveYearMonthOthersButton.CommandParameter = (_otherActivities, idsToDelete);

    }

    private void AddMoneyTextBox<TEntity>(DataGrid gridToAddTo, string bindingString, string bindingWayOfPay, string headerName)
    {
        var moneyTemplate = new FuncDataTemplate<YearMonthOtherUI>((x, _) =>
        {
            Grid cellGrid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Star)
                }
            };
            //TextBox
            TextBox moneyTextBox = new TextBox();
            Binding moneyBinding = new Binding(bindingString)
            {
                Mode = BindingMode.TwoWay,
                Converter = new IntToStringConverter(),
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            moneyTextBox.Bind(TextBox.TextProperty, moneyBinding);
            //moneyTextBox.KeyDown += UIUtilities.TextBox_NumericInput_KeyDown;
            //CombBox
            ComboBox wayOfPayCombobox = new ComboBox();
            
            //wayOfPayCombobox.Bind(ComboBox.ItemsSourceProperty, new Binding { Path = "DataContext.PaymentMethods", Source = gridToAddTo });
            //wayOfPayCombobox.Bind(ComboBox.SelectedItemProperty, new Binding(bindingWayOfPay));
            //Adding to cellGrid
            cellGrid.Children.Add(moneyTextBox);
            Grid.SetColumn(moneyTextBox, 0);
            cellGrid.Children.Add(wayOfPayCombobox);
            Grid.SetColumn(wayOfPayCombobox, 1);

            return cellGrid;
        });

        gridToAddTo.Columns.Add(new DataGridTemplateColumn
        {
            Header = headerName,
            CellTemplate = moneyTemplate,
        });
    }

}