using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Layout;
using DbAccess.Models;
using Material.Icons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;


namespace AlmanUI.Views;

/// <summary>
/// View for monthly children activities.
/// </summary>
public partial class YearMonthActivitiesPageView : UserControl, IInitDataGrid, IUpdateDataGrid, ILoadItemsWithParams
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

    public void LoadItems(int year, int month)
    {
        _yearMonthActivitiesTable = YearMonthActivitiesControl.GetItemsByFilter(act => act.Year == year && act.Month == month);
        _activitiesTable = ActivitiesControl.GetItems();

        _childrenTable = ChildrenControl.GetItemsByFilter(ch => true); ///TODO: children that were accepted earlier or in that month.

        var compositeItems = new List<YearMonthActivityCompositeItem>();
        foreach (var child in _childrenTable)
        {
            var newItem = new YearMonthActivityCompositeItem { YMChild = child };
            newItem.YMActivities = new List<IYearMonthActivityBase>(_yearMonthActivitiesTable.Where(act => act.YmchildId == child.Id).ToList());
            
            foreach (var act in _activitiesTable)
            {
                if (newItem.YMActivities is null || newItem.YMActivities.Count == 0 || newItem.YMActivities.Where(activity => activity.YmactivityId == act.Id).ToList().Count == 0)
                {
                    newItem.YMActivities!.Add(new YearMonthActivityUI
                    {
                        YmactivityId = act.Id,
                        YmchildId = newItem.YMChild!.Id,
                        Month = month,
                        Year = year,
                        YmactivitySum = 0,
                        YmwasPaid = 0,
                        YmwayOfPaying = 0,
                    });
                }
            }

            compositeItems.Add(newItem);
        }
        _yearMonthActivities = compositeItems;
    }

    /// <summary>
    /// ctor
    /// </summary>
    public YearMonthActivitiesPageView()
    {
        LoadItems(DateTime.Now.Year, DateTime.Now.Month);
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
        LoadItems(year, month);

        MainDataGrid.Columns.Clear();
        InitDataGrid();
    }

    public void InitDataGrid()
    {
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
                        new ColumnDefinition(GridLength.Star)
                    }
                };

                IYearMonthActivityBase act = x.YMActivities!.Single(act => act.YmactivityId == activity.Id);
                int index = x.YMActivities!.IndexOf(act);

                var monthSumActivity = UIControlElements.CreateNumericTextBox($"YMActivities[{index}].YmactivitySum");
                
                grid.Children.Add(monthSumActivity);
                Grid.SetColumn(monthSumActivity, 0);

                var fillDatesButton = new Button { Content = UIControlElements.CreateIcon(MaterialIconKind.CalendarMultiselectOutline, 24, 24) };
                ToolTip.SetTip(fillDatesButton, AlmanUI.Resources.ChildrenResources.FillDatesToolTip);
                fillDatesButton.Click += OnFillDatesClick;
                
                grid.Children.Add(fillDatesButton);
                Grid.SetColumn(fillDatesButton, 1);

                var oneTimePrice = new TextBlock();
                oneTimePrice.Text = activity.ActivityPrice.ToString();
                oneTimePrice.Padding = new Thickness(3);
                grid.Children.Add(oneTimePrice);
                Grid.SetColumn(oneTimePrice, 2);

                var paymentMethod = UIControlElements.CreateComboBox(new WayOfPayingConverter(), $"YMActivities[{index}].YmwayOfPaying");
                paymentMethod.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
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
            CanUserResize = false,
            MinWidth = 40,
            Width = new DataGridLength(100, DataGridLengthUnitType.Star)

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

        int year = 0;
        int month = 0;
        if (item.YMActivities is not null && item.YMActivities.Count > 0)
        {
            year = item.YMActivities![0].Year;
            month = item.YMActivities![0].Month;
        }
        ChildBill bill = YearMonthActivitiesControl.CalculateChildBill(item.YMChild!.Id, year, month);

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
    /// <param name="selectedDates">List of selected dates for month.</param>
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


