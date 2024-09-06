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

/// <summary>
/// View for monthly staff activities.
/// </summary>
public partial class YearMonthStaffActivitiesPageView : UserControl, ILoadItemsWithParams, IUpdateDataGrid
{
    /// <summary>
    /// Monthly activities table read from the table.
    /// </summary>
    private IReadOnlyList<IYearMonthStaffActivityBase>? _yearMonthStaffActivitiesTable;

    /// <summary>
    /// Staff activities table read from the database.
    /// </summary>
    private IReadOnlyList<IStaffActivityBase> _staffActivitiesTable;

    /// <summary>
    /// Staff members table from the database.
    /// </summary>
    private IReadOnlyList<IStaffMemberBase>? _staffMemberTable;

    /// <summary>
    /// Result collection that will be shown in UI view.
    /// </summary>
    private IReadOnlyList<YearMonthStaffActivityCompositeItem>? _memberActivities;

    public void LoadItems(int year, int month)
    {
        _yearMonthStaffActivitiesTable = 
            YearMonthStaffActivitiesControl.GetItemsByFilter(act => act.Year == year && act.Month == month);
        _staffActivitiesTable = StaffActivitiesControl.GetItems();
        DateTime now = new DateTime(year, month, 1);
        _staffMemberTable = StaffMembersControl.GetItemsByFilter(mem => new DateTime(mem.StartYear, mem.StartMonth, 1) <= now);

        var compositeItems = new List<YearMonthStaffActivityCompositeItem>();
        foreach (var member in _staffMemberTable)
        {            
            List<IYearMonthStaffActivityBase> memberActivities = _yearMonthStaffActivitiesTable.Where(act => act.StaffMemberId == member.Id).ToList();
            var newItem = new YearMonthStaffActivityCompositeItem { StaffMember = member, Activities = memberActivities };
            compositeItems.Add(newItem);
        }
        _memberActivities = compositeItems;
    }

    /// <summary>
    /// ctor.
    /// </summary>
    public YearMonthStaffActivitiesPageView()
    {
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;
        LoadItems(year, month);
        InitializeComponent();
        InitDataGrid(year, month);
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
        if (message == "UpdateYearMonthStaffActivities")
            UpdateDataGrid(year, month);
    }


    public void UpdateDataGrid(int year, int month)
    {
        LoadItems(year, month);
        YearMonthStaffActivitiesMainDataGrid.Columns.Clear();
        InitDataGrid(year, month);
    }

    /// <summary>
    /// Initialize data grid of the table for the UI view.
    /// </summary>
    /// <param name="year">Year for which the data are fetched.</param>
    /// <param name="month">Month for which the data are fetched.</param>
    public void InitDataGrid(int year, int month)
    {
        YearMonthStaffActivitiesMainDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        YearMonthStaffActivitiesMainDataGrid.ItemsSource = _memberActivities;

        YearMonthStaffActivitiesMainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.StaffResources.StaffFirstName, Binding = new Binding("StaffMember.FirstName"), IsReadOnly = true });
        YearMonthStaffActivitiesMainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.StaffResources.StaffLastName, Binding = new Binding("StaffMember.LastName"), IsReadOnly = true });

        var staffActivitiesLocal = new List<IStaffActivityBase>(_staffActivitiesTable);
        
        foreach (IStaffActivityBase activity in staffActivitiesLocal)
        {
            var activityTemplate = new FuncDataTemplate<YearMonthStaffActivityCompositeItem>((x, _) => 
            {
                Grid grid = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitions
                    {
                        new ColumnDefinition(GridLength.Star),
                    }
                };

                if (x.Activities is null)
                {
                    x.Activities = new List<IYearMonthStaffActivityBase>();
                }

                if (x.Activities.Count == 0 || x.Activities.Where(act => act.StaffActivityId == activity.Id).ToList().Count == 0) 
                {
                    x.Activities.Add(new YearMonthStaffActivityUI
                    {
                        Year = year, Month = month,
                        StaffActivityId = activity.Id,
                        StaffMemberId = x.StaffMember!.Id,
                        SumPaid = 0
                    });


                }
                var activityInDb = x.Activities.Single(act => act.StaffActivityId == activity.Id);

                int index = x.Activities.IndexOf(activityInDb);
                
                var textBox = UIControlElements.CreateNumericTextBox($"Activities[{index}].SumPaid");

                grid.Children.Add( textBox );
                Grid.SetColumn(textBox, 0);

                return grid;
            });

            YearMonthStaffActivitiesMainDataGrid.Columns.Add(new DataGridTemplateColumn
            {
                Header = activity.ActivityName,
                CellTemplate = activityTemplate,
  
            });
        }
        
        SaveMonthStaffActivitiesButton.CommandParameter = _memberActivities;
    }
}