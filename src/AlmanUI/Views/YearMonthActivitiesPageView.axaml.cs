using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Interactivity;
using Material.Icons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace AlmanUI.Views;

/// <summary>
/// View for monthly children activities.
/// </summary>
public partial class YearMonthActivitiesPageView : UserControl, IInitDataGrid, IUpdateDataGrid
{
    /// <summary>
    /// Monthly activities table read from the database.
    /// </summary>
    private IReadOnlyList<IYearMonthActivityBase> _yearMonthActivitiesTable;

    /// <summary>
    /// Activities table read from the database.
    /// </summary>
    private IReadOnlyList<IActivityBase> _activitiesTable;

    /// <summary>
    /// Children table raed from the database.
    /// </summary>
    private IReadOnlyList<IChildBase> _childrenTable;

    /// <summary>
    /// Result collection that will be shown in UI view.
    /// </summary>
    private IReadOnlyList<YearMonthActivityCompositeItem> _yearMonthActivities { get; set; }

    /// <summary>
    /// ctor
    /// </summary>
    public YearMonthActivitiesPageView()
    {
        _yearMonthActivitiesTable = YearMonthActivitiesControl.GetItemsByFilter(act => act.Year == DateTime.Now.Year && act.Month == DateTime.Now.Month);
        _activitiesTable = ActivitiesControl.GetItems();
        _childrenTable = ChildrenControl.GetItemsByFilter(ch => true);

        var compositeItems = new List<YearMonthActivityCompositeItem>();
        foreach (var child in _childrenTable)
        {
            var newItem = new YearMonthActivityCompositeItem { YMChild = child };
            var childActivities = _yearMonthActivitiesTable.Where(act => act.YmchildId == child.Id ).ToList();
            newItem.YMActivities = childActivities;
            
            compositeItems.Add(newItem);
        }
        
        _yearMonthActivities = compositeItems;
        
        
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
    private void OnNotifyWithParams(string message, int year, int  month)
    {
        if (message == "UpdateDataGrid")
        {
            UpdateDataGrid(year, month);
        }
    }

    public void UpdateDataGrid(int year, int month)
    {
        var yearMonthActivities = YearMonthActivitiesControl.GetItemsByFilter(act => act.Year == year && act.Month == month);
        _yearMonthActivitiesTable = yearMonthActivities;
        var activities = ActivitiesControl.GetItems();
        _activitiesTable = activities;
        var children = ChildrenControl.GetItemsByFilter(ch => true); //ch => ch.ChildState == 1
        _childrenTable = children;

        var compositeItems = new List<YearMonthActivityCompositeItem>();
        foreach (var child in _childrenTable)
        {
            var newItem = new YearMonthActivityCompositeItem { YMChild = child };
            var childActivities = _yearMonthActivitiesTable.Where(act => act.YmchildId == child.Id).ToList();
            newItem.YMActivities = childActivities;

            compositeItems.Add(newItem);
        }

        _yearMonthActivities = compositeItems;

        MainDataGrid.Columns.Clear();
        InitDataGrid();
    }

    public void InitDataGrid()
    {
        //MainDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);

        MainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.ChildrenResources.ChildFirstName, Binding = new Binding("YMChild.ChildName"), IsReadOnly = true, MinWidth = 300 });
        MainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.ChildrenResources.ChildLastName, Binding = new Binding("YMChild.ChildLastName"), IsReadOnly = true, MinWidth = 300 });

        foreach (var activity in _activitiesTable)
        {
            var template = new FuncDataTemplate<YearMonthActivityCompositeItem>((x, _) =>
            {
                var grid = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitions
                    {
                        new ColumnDefinition(GridLength.Auto),
                        new ColumnDefinition(GridLength.Auto),
                        new ColumnDefinition(GridLength.Auto),
                        new ColumnDefinition(GridLength.Auto)
                    }
                };

                
                

                if (x.YMActivities is null || x.YMActivities.Count == 0 || x.YMActivities.Where(act => act.YmactivityId == activity.Id).ToList().Count == 0)
                {
                    x.YMActivities!.Add(new YearMonthActivityUI { 
                        YmactivityId = activity.Id, 
                        YmchildId = x.YMChild!.Id, 
                        Month = DateTime.Now.Month,
                        Year = DateTime.Now.Year,
                        YmactivitySum = 0,
                        YmwasPaid = 0,
                        YmwayOfPaying = 0,
                    });
                }

                IYearMonthActivityBase act = x.YMActivities.Single(act => act.YmactivityId == activity.Id);
                int index = x.YMActivities.IndexOf(act);

                var monthSumActivity = UIControlElements.CreateNumericTextBox($"YMActivities[{index}].YmactivitySum");
                //monthSumActivity.Bind(TextBox.TextProperty, new Binding($"YMActivities[{index}].YmactivitySum"));
                
                grid.Children.Add(monthSumActivity);
                Grid.SetColumn(monthSumActivity, 0);

                var fillDatesButton = new Button { Content = UIControlElements.CreateIcon(MaterialIconKind.CalendarMultiselectOutline, 24, 24) };
                ToolTip.SetTip(fillDatesButton, AlmanUI.Resources.ChildrenResources.FillDatesToolTip);
                fillDatesButton.Click += OnFillDatesClick;
                
                grid.Children.Add(fillDatesButton);
                Grid.SetColumn(fillDatesButton, 1);

                var oneTimePrice = new TextBlock();
                oneTimePrice.Text = activity.ActivityPrice.ToString();
                grid.Children.Add(oneTimePrice);
                Grid.SetColumn(oneTimePrice, 2);

                var paymentMethod = UIControlElements.CreateComboBox(new WayOfPayingConverter(), $"YMActivities[{index}].YmwayOfPaying");
                grid.Children.Add(paymentMethod);
                Grid.SetColumn(paymentMethod, 3);

                return grid;
            });

            MainDataGrid.Columns.Add(new DataGridTemplateColumn
            {
                Header = activity.ActivityName,
                CellTemplate = template,
                MinWidth = 300
            });
        }

        var childBillTemplate = new FuncDataTemplate<YearMonthActivityCompositeItem>((x, _) => 
        {
            
            Button calculateButton = new Button()
            {
                Content = UIControlElements.CreateIcon(MaterialIconKind.Cash, 24, 24),
                CommandParameter = x,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                
            };

            calculateButton.Click += OnCalculateBillClick;
            
            return calculateButton;
        });
        
        MainDataGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "",
            CellTemplate = childBillTemplate,
            Width = new DataGridLength(40)
        });

        MainDataGrid.ItemsSource = _yearMonthActivities;
        SaveMonthActivitiesButton.CommandParameter = _yearMonthActivities;
    }

    /// <summary>
    /// Calculate Bill button is clicked, show the window with the bill.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnCalculateBillClick(object? sender, RoutedEventArgs e)
    {
        
        if (sender is null) { return; }
        var compositeItem = ((Button)sender).CommandParameter;
        if (compositeItem is null) { return; }

        
        YearMonthActivityCompositeItem item = (YearMonthActivityCompositeItem)compositeItem;
            
        ChildBill bill = YearMonthActivitiesControl.CalculateChildBill(item.YMChild!.Id, item.YMActivities![0].Year, item.YMActivities![0].Month);

        var billWindow = new ChildBillWindow();
        billWindow.SetChildBill(bill);
        if (VisualRoot is null) { return; }
        await billWindow.ShowDialog((Window)VisualRoot);
    }

    /// <summary>
    /// Fill dates button is clicked. Show calendar and save chosen dates.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnFillDatesClick(object? sender, RoutedEventArgs e)
    {
        var calendarWindow = new YMActivitiesCalendarWindow();
        if (VisualRoot is null) { return; }
        await calendarWindow.ShowDialog((Window)VisualRoot);
        if (calendarWindow.SelectedDates != null)
        {

            if (sender is null)
            {
                Debug.WriteLine("Null sender when must be button.");
                return;
            }

            Button fillDatesButtonSender = (Button)sender;

            UpdateSelectedDates(calendarWindow.SelectedDates, fillDatesButtonSender);
        }
    }

    /// <summary>
    /// Update chosen dates in calendar window.
    /// </summary>
    /// <param name="selectedDates"></param>
    /// <param name="button"></param>
    public void UpdateSelectedDates(IReadOnlyList<DateTime> selectedDates, Button button)
    {
        var parentGrid = button.Parent as Grid;

        if (parentGrid is not null)
        {
            var sumTextBox = (TextBox)parentGrid.Children[0];
            var oneTimePriceTextBox = (TextBlock)parentGrid.Children[2];

            if (sumTextBox is not null && oneTimePriceTextBox is not null)
            {
                int monthSum = selectedDates.Count * Int32.Parse(oneTimePriceTextBox.Text!);

                sumTextBox.Text = monthSum.ToString();
            }
        }
    }
}


