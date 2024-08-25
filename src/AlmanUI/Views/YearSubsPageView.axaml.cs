using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System;
using System.Linq;
using Avalonia.Data;
using Avalonia.Controls.Templates;
using System.Diagnostics;

namespace AlmanUI.Views;

public partial class YearSubsPageView : UserControl
{
    public YearSubsPageView()
    {
        InitializeComponent();
        InitYearSubsMainDataGrid();
        Mediator.Mediator.Instance.NotifyWithOneParam += OnNotifyWithOneParam;
    }

    private void OnNotifyWithOneParam(string message, int year)
    {
        if (message == "UpdateYearSubsMainDataGrid") UpdateYearSubsMainDataGrid();
    }

    public void InitYearSubsMainDataGrid()
    {
        YearSubsMainDataGrid.MinColumnWidth = 150;

        YearSubsMainDataGrid.Columns.Add(
            new DataGridTextColumn
            {
                Header = "Child name",
                Binding = new Binding("YsChild.ChildName"),
                IsReadOnly = true,
            });

        YearSubsMainDataGrid.Columns.Add(
            new DataGridTextColumn
            {
                Header = "Child Lastname",
                Binding = new Binding("YsChild.ChildLastName"),
                IsReadOnly = true,
            });
        
        var months = new List<string> {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};

        foreach (var month in months)
        {
            var moneyTemplate = new FuncDataTemplate<YearSubCompositeItem>((x, _) =>
            {
                Grid cellGrid = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitions
                    {
                        new ColumnDefinition(GridLength.Auto),
                        new ColumnDefinition(GridLength.Auto)
                    }
                };
                int indexMonth = months.IndexOf(month);
                IYearSubBase subscription = x.YsYearSubscriptions.Single(sub => sub.Month == indexMonth + 1);
                int index = x.YsYearSubscriptions.IndexOf(subscription);

                //TextBox
                TextBox moneyTextBox = UIControlElements.CreateNumericTextBox($"YsYearSubscriptions[{index}].Payment");
                //CombBox
                ComboBox wayOfPayCombobox = UIControlElements.CreateComboBox(new WayOfPayingConverter(), $"YsYearSubscriptions[{index}].WayOfaying");
                //Adding to cellGrid
                cellGrid.Children.Add(moneyTextBox);
                Grid.SetColumn(moneyTextBox, 0);
                cellGrid.Children.Add(wayOfPayCombobox);
                if (x.YsChild.Id == 0) { wayOfPayCombobox.IsVisible = false; }
                Grid.SetColumn(wayOfPayCombobox, 1);
                
                return cellGrid;
            });

            YearSubsMainDataGrid.Columns.Add(new DataGridTemplateColumn
            {
                Header = AlmanUI.Resources.CommonResources.ResourceManager.GetString(month, AlmanUI.Resources.CommonResources.Culture)!,
                CellTemplate = moneyTemplate,
            });
        }
    }

    

    private void UpdateYearSubsMainDataGrid()
    {
        YearSubsMainDataGrid.Columns.Clear();
        InitYearSubsMainDataGrid();
    }
}