using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using AlmanUI.ViewModels;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using AlmanUI.Mediator;
using Alman.SharedDefinitions;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Diagnostics;


namespace AlmanUI.Views;

public partial class YearMonthActivitiesPageView : UserControl
{
    private IReadOnlyList<IYearMonthActivityBase> _yearMonthActivitiesTable;
    private IReadOnlyList<IActivityBase> _activitiesTable;
    private IReadOnlyList<IChildBase> _childrenTable;
    private IReadOnlyList<CompositeItem> _yearMonthActivities { get; set; }


    public YearMonthActivitiesPageView()
    {   
        var yearMonthActivities = YearMonthActivitiesControl.GetYearMonthActivities(DateTime.Now.Year, DateTime.Now.Month);
        _yearMonthActivitiesTable = yearMonthActivities;
        var activities = ActivitiesControl.GetActivities();
        _activitiesTable = activities;
        var children = ChildrenControl.GetChildrenByFilter(ch => true); //ch => ch.ChildState == 1
        _childrenTable = children;

        var compositeItems = new List<CompositeItem>();
        foreach (var child in _childrenTable)
        {
            var newItem = new CompositeItem { YMChild = child };
            var childActivities = _yearMonthActivitiesTable.Where(act => act.YmchildId == child.ChildId ).ToList();
            newItem.YMActivities = childActivities;
            
            compositeItems.Add(newItem);
        }
        
        _yearMonthActivities = compositeItems;
        
        
        InitializeComponent();
        InitDataGrid();
        Mediator.Mediator.Instance.NotifyWithParams += OnNotifyWithParams;
    }


    private void OnNotifyWithParams(string message, int year, int  month)
    {
        if (message == "UpdateDataGrid")
        {
            UpdateDataGrid(year, month);
        }
    }


    private void UpdateDataGrid(int year, int month)
    {
        var yearMonthActivities = YearMonthActivitiesControl.GetYearMonthActivities(year, month);
        _yearMonthActivitiesTable = yearMonthActivities;
        var activities = ActivitiesControl.GetActivities();
        _activitiesTable = activities;
        var children = ChildrenControl.GetChildrenByFilter(ch => true); //ch => ch.ChildState == 1
        _childrenTable = children;

        var compositeItems = new List<CompositeItem>();
        foreach (var child in _childrenTable)
        {
            var newItem = new CompositeItem { YMChild = child };
            var childActivities = _yearMonthActivitiesTable.Where(act => act.YmchildId == child.ChildId).ToList();
            newItem.YMActivities = childActivities;

            compositeItems.Add(newItem);
        }

        _yearMonthActivities = compositeItems;

        MainDataGrid.Columns.Clear();
        InitDataGrid();
    }


    private void InitDataGrid()
    {

        MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Child Name", Binding = new Binding("YMChild.ChildName") });
        MainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Child Lastname", Binding = new Binding("YMChild.ChildLastName") });

        foreach (var activity in _activitiesTable)
        {

            var template = new FuncDataTemplate<CompositeItem>((x, _) =>
            {
                var grid = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitions
                    {
                        new ColumnDefinition(GridLength.Star),
                        new ColumnDefinition(GridLength.Star),
                        new ColumnDefinition(GridLength.Star),
                        new ColumnDefinition(GridLength.Star)
                    }
                };

                var monthSumActivity = new TextBox {Name = "monthSumActivityBox" };
                

                if (x.YMActivities is null || x.YMActivities.Count == 0 || x.YMActivities.Where(act => act.YmactivityId == activity.ActivityId).ToList().Count == 0)
                {
                    x.YMActivities!.Add(new YearMonthActivityUI { 
                        YmactivityId = activity.ActivityId, 
                        YmchildId = x.YMChild!.ChildId, 
                        Month = DateTime.Now.Month,
                        Year = DateTime.Now.Year,
                        YmactivitySum = 0,
                        YmwasPaid = 0,
                        YmwayOfPaying = 0,
                    });

                    monthSumActivity.Text = 0.ToString();

                }

                IYearMonthActivityBase act = x.YMActivities.Single(act => act.YmactivityId == activity.ActivityId);
                int index = x.YMActivities.IndexOf(act);
                
                monthSumActivity.Bind(TextBox.TextProperty, new Binding($"YMActivities[{index}].YmactivitySum"));
                
                grid.Children.Add(monthSumActivity);
                Grid.SetColumn(monthSumActivity, 0);

                var fillDatesButton = new Button { Content = "v" };
                //fillDatesButton.Bind(Button.ContentProperty, new Binding($"YMActivities[{index}].YmactivitySum"));
                ToolTip.SetTip(fillDatesButton, "Show calendar to fill dates manully.");
                fillDatesButton.Click += OnFillDatesClick;
                
                grid.Children.Add(fillDatesButton);
                Grid.SetColumn(fillDatesButton, 1);

                var oneTimePrice = new TextBlock();
                oneTimePrice.Text = activity.ActivityPrice.ToString();
                grid.Children.Add(oneTimePrice);
                Grid.SetColumn(oneTimePrice, 2);

                var paymentMethod = new ComboBox();
                paymentMethod.Bind(ComboBox.ItemsSourceProperty, new Binding { Path = "DataContext.PaymentMethods", Source = MainDataGrid });
                paymentMethod.Bind(ComboBox.SelectedItemProperty, new Binding { Path = $"YMActivities[{index}].YmwayOfPaying" });
                //doesnt work
                paymentMethod.ItemTemplate = new FuncDataTemplate<WayOfPaying>((method, _) =>
                {
                    var textBlock = new TextBlock();
                    textBlock.Bind(TextBlock.TextProperty, new Binding($"YMActivities[{index}].YmwayOfPaying", BindingMode.TwoWay)
                    {
                        Converter = new AlmanUI.ViewModels.PaymentMethodConverter()
                    });
                    return textBlock;
                });
                
                grid.Children.Add(paymentMethod);
                Grid.SetColumn(paymentMethod, 3);


                return grid;
            });



            MainDataGrid.Columns.Add(new DataGridTemplateColumn
            {
                Header = activity.ActivityName,
                Width = DataGridLength.Auto,
                CellTemplate = template
            });
        }

        MainDataGrid.ItemsSource = _yearMonthActivities;
        SaveMonthActivitiesButton.CommandParameter = _yearMonthActivities;
    }

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

    public void UpdateSelectedDates(IReadOnlyList<DateTime> selectedDates, Button button)
    {
        var parentGrid = button.Parent as Grid;

        if (parentGrid is not null)
        {
            var sumTextBox = (TextBox)parentGrid.Children[0];
            var oneTimePriceTextBox = (TextBlock)parentGrid.Children[2];

            if (sumTextBox is not null && oneTimePriceTextBox is not null)
            {
                int monthSum = selectedDates.Count * Int32.Parse(oneTimePriceTextBox.Text);

                sumTextBox.Text = monthSum.ToString();
            }
        }
    }
}


public class CompositeItem
{
    public IChildBase? YMChild { get; set; }
    public IList<IYearMonthActivityBase>? YMActivities { get; set; }
}