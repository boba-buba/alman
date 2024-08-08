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

namespace AlmanUI.Views;

public partial class YearSubsPageView : UserControl
{
    private IReadOnlyList<IChildBase>? _childrenTable;

    private IReadOnlyList<IYearSubBase>? _yearSubsTable;

    private IReadOnlyList<YearSubCompositeItem>? _childYearSubs;

    private void LoadItems(int year)
    {
        _childrenTable = ChildrenControl.GetChildrenByFilter(ch => ch.ChildStartYear <=  year);

        if (_childrenTable is null)
        {
            return;
        }

        _yearSubsTable = YearSubsControl.GetYearSubsByFilter(ys => ys.Yyear == year);

        var childYearSubs = new List<YearSubCompositeItem>();
        foreach (var child in _childrenTable)
        {
            var newItem = new YearSubCompositeItem { YsChild = child};
            IYearSubBase? newItemYearSub = _yearSubsTable.SingleOrDefault(ys => ys.YchildId == child.ChildId);
            if (_yearSubsTable.Count == 0 || newItemYearSub is null)
            {
                newItemYearSub = new YearSubUI { YchildId = child.ChildId, Yyear = year };
            }
            newItem.YsYearSubscription = newItemYearSub;
            childYearSubs.Add(newItem);
        }
        _childYearSubs = childYearSubs;
    }

    public YearSubsPageView()
    {
        LoadItems(DateTime.Now.Year);
        if (_childYearSubs is null)
        {
            _childYearSubs = new List<YearSubCompositeItem>();
        }
        InitializeComponent();
        InitYearSubsMainDataGrid();
        Mediator.Mediator.Instance.NotifyWithOneParam += OnNotifyWithOneParam;
    }

    private void OnNotifyWithOneParam(string message, int year)
    {
        if (message == "UpdateYearSubsMainDataGrid") UpdateYearSubsMainDataGrid(year);
    }


    public void InitYearSubsMainDataGrid()
    {
        YearSubsMainDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);

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

        YearSubsMainDataGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "January",
            CellTemplate = new FuncDataTemplate<object>((item, namescope) =>
            {
                var textBox = new TextBox();
                textBox.Bind(TextBox.TextProperty, new Binding("YsYearSubscription.Yjanuary", BindingMode.TwoWay));
                textBox.KeyDown += UIUtilities.TextBox_NumericInput_KeyDown;  // Attach the filtering function
                return textBox;
            }),
        });

        YearSubsMainDataGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "February",
            CellTemplate = new FuncDataTemplate<object>((item, namescope) =>
            {
                var textBox = new TextBox();
                textBox.Bind(TextBox.TextProperty, new Binding("YsYearSubscription.Yfebruary", BindingMode.TwoWay));
                textBox.KeyDown += UIUtilities.TextBox_NumericInput_KeyDown;  // Attach the filtering function
                return textBox;
            }),
        });

        YearSubsMainDataGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "March",
            CellTemplate = new FuncDataTemplate<object>((item, namescope) =>
            {
                var textBox = new TextBox();
                textBox.Bind(TextBox.TextProperty, new Binding("YsYearSubscription.Ymarch", BindingMode.TwoWay));
                textBox.KeyDown += UIUtilities.TextBox_NumericInput_KeyDown;  // Attach the filtering function
                return textBox;
            }),
        });

        YearSubsMainDataGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "April",
            CellTemplate = new FuncDataTemplate<object>((item, namescope) =>
            {
                var textBox = new TextBox();
                textBox.Bind(TextBox.TextProperty, new Binding("YsYearSubscription.Yapril", BindingMode.TwoWay));
                textBox.KeyDown += UIUtilities.TextBox_NumericInput_KeyDown;  // Attach the filtering function
                return textBox;
            }),
        });

        YearSubsMainDataGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "May",
            CellTemplate = new FuncDataTemplate<object>((item, namescope) =>
            {
                var textBox = new TextBox();
                textBox.Bind(TextBox.TextProperty, new Binding("YsYearSubscription.Ymay", BindingMode.TwoWay));
                textBox.KeyDown += UIUtilities.TextBox_NumericInput_KeyDown;  // Attach the filtering function
                return textBox;
            }),
        });

        YearSubsMainDataGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "June",
            CellTemplate = new FuncDataTemplate<object>((item, namescope) =>
            {
                var textBox = new TextBox();
                textBox.Bind(TextBox.TextProperty, new Binding("YsYearSubscription.Yjune", BindingMode.TwoWay));
                textBox.KeyDown += UIUtilities.TextBox_NumericInput_KeyDown;  // Attach the filtering function
                return textBox;
            }),
        });

        YearSubsMainDataGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "July",
            CellTemplate = new FuncDataTemplate<object>((item, namescope) =>
            {
                var textBox = new TextBox();
                textBox.Bind(TextBox.TextProperty, new Binding("YsYearSubscription.Yjuly", BindingMode.TwoWay));
                textBox.KeyDown += UIUtilities.TextBox_NumericInput_KeyDown;  // Attach the filtering function
                return textBox;
            }),
        });

        YearSubsMainDataGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "August",
            CellTemplate = new FuncDataTemplate<object>((item, namescope) =>
            {
                var textBox = new TextBox();
                textBox.Bind(TextBox.TextProperty, new Binding("YsYearSubscription.Yaugust", BindingMode.TwoWay));
                textBox.KeyDown += UIUtilities.TextBox_NumericInput_KeyDown;  // Attach the filtering function
                return textBox;
            }),
        });

        YearSubsMainDataGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "September",
            CellTemplate = new FuncDataTemplate<object>((item, namescope) =>
            {
                var textBox = new TextBox();
                textBox.Bind(TextBox.TextProperty, new Binding("YsYearSubscription.Yseptember", BindingMode.TwoWay));
                textBox.KeyDown += UIUtilities.TextBox_NumericInput_KeyDown;  // Attach the filtering function
                return textBox;
            }),
        });

        YearSubsMainDataGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "October",
            CellTemplate = new FuncDataTemplate<object>((item, namescope) =>
            {
                var textBox = new TextBox();
                textBox.Bind(TextBox.TextProperty, new Binding("YsYearSubscription.Yoctober", BindingMode.TwoWay));
                textBox.KeyDown += UIUtilities.TextBox_NumericInput_KeyDown;  // Attach the filtering function
                return textBox;
            }),
        });

        YearSubsMainDataGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "November",
            CellTemplate = new FuncDataTemplate<object>((item, namescope) =>
            {
                var textBox = new TextBox();
                textBox.Bind(TextBox.TextProperty, new Binding("YsYearSubscription.Ynovember", BindingMode.TwoWay));
                textBox.KeyDown += UIUtilities.TextBox_NumericInput_KeyDown;  // Attach the filtering function
                return textBox;
            }),
        });

        YearSubsMainDataGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "December",
            CellTemplate = new FuncDataTemplate<object>((item, namescope) =>
            {
                var textBox = new TextBox();
                textBox.Bind(TextBox.TextProperty, new Binding("YsYearSubscription.Ydecember", BindingMode.TwoWay));
                textBox.KeyDown += UIUtilities.TextBox_NumericInput_KeyDown;  // Attach the filtering function
                return textBox;
            }),
        });

        YearSubsMainDataGrid.ItemsSource = _childYearSubs;
        SaveYearSubsButton.CommandParameter = _childYearSubs;
    }


    private void UpdateYearSubsMainDataGrid(int year)
    {
        LoadItems(year);
        YearSubsMainDataGrid.Columns.Clear();
        InitYearSubsMainDataGrid();
    }

}