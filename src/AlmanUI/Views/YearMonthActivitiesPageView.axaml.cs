using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
namespace AlmanUI.Views;

public partial class YearMonthActivitiesPageView : UserControl
{
    private IReadOnlyList<IYearMonthActivityBase> _yearMonthActivitiesTable;
    private IReadOnlyList<IActivityBase> _activitiesTable;
    private IReadOnlyList<IChildBase> _childrenTable;

    private IReadOnlyList<CompositeItem> _yearMonthActivities { get; set; }
    public YearMonthActivitiesPageView()
    {
        var year = DateTime.Now.Year;
        var month = DateTime.Now.Month;

        var yearMonthActivities = YearMonthActivitiesControl.GetYearMonthActivities(year, month);
        _yearMonthActivitiesTable = yearMonthActivities;
        var activities = ActvitiesControl.GetActivities();
        _activitiesTable = activities;
        var activeChildren = ChildrenControl.GetChildrenOnCondition(ch => ch.ChildState == 1);
        _childrenTable = activeChildren;

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
                        new ColumnDefinition(GridLength.Star)
                    }
                };

                var monthSumActivity = new TextBox();

                if (x.YMActivities.Count == 0 || x.YMActivities is null || x.YMActivities.Where(act => act.YmactivityId == activity.ActivityId).ToList().Count == 0)
                {
                    x.YMActivities!.Add(new YearMonthActivityUI { 
                        YmactivityId = activity.ActivityId, 
                        YmchildId = x.YMChild.ChildId, 
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

                var fillDatesButton = new Button { Content = "Fill Dates" };
                grid.Children.Add(fillDatesButton);
                Grid.SetColumn(fillDatesButton, 1);

                var oneTimePrice = new TextBlock();
                oneTimePrice.Text = activity.ActivityPrice.ToString();
                grid.Children.Add(oneTimePrice);
                Grid.SetColumn(oneTimePrice, 2);

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
}



public class CompositeItem
{

    public IChildBase YMChild { get; set; }
    public IList<IYearMonthActivityBase> YMActivities { get; set; }
}