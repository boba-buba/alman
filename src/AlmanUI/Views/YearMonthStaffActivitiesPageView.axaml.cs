using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using DbAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AlmanUI.Views;

public partial class YearMonthStaffActivitiesPageView : UserControl
{
    private IReadOnlyList<IYearMonthStaffActivityBase> _yearMonthStaffActivitiesTable;
    private IReadOnlyList<IStaffActivityBase> _staffActivitiesTable;
    private IReadOnlyList<IStaffMemberBase> _staffMemberTable;

    private IReadOnlyList<YearMonthStaffActivityCompositeItem> _memberActivities;

    private void LoadItems(int year, int month)
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

    public YearMonthStaffActivitiesPageView()
    {
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;
        LoadItems(year, month);
        InitializeComponent();
        InitDataGrid(year, month);
        Mediator.Mediator.Instance.NotifyWithParams += OnNotifyWithParams;
    }

    private void OnNotifyWithParams(string message, int year, int month)
    {
        if (message == "UpdateYearMonthStaffActivities")
            UpdateDataGrid(year, month);
    }

    private void UpdateDataGrid(int year, int month)
    {
        LoadItems(year, month);
        YearMonthStaffActivitiesMainDataGrid.Columns.Clear();
        InitDataGrid(year, month);
    }

    private void InitDataGrid(int year, int month)
    {
        YearMonthStaffActivitiesMainDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        YearMonthStaffActivitiesMainDataGrid.ItemsSource = _memberActivities;

        YearMonthStaffActivitiesMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "First Name", Binding = new Binding("StaffMember.FirstName"), IsReadOnly = true });
        YearMonthStaffActivitiesMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Last Name", Binding = new Binding("StaffMember.LastName"), IsReadOnly = true });

        var staffActivitiesLocal = new List<IStaffActivityBase>(_staffActivitiesTable);
        
        foreach (IStaffActivityBase activity in staffActivitiesLocal)
        {
            Debug.WriteLine($"{activity.ActivityName} AAA");
            var activityTemplate = new FuncDataTemplate<YearMonthStaffActivityCompositeItem>((x, _) => 
            {
                Debug.WriteLine($"{activity.ActivityName} again");
                Grid grid = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitions
                    {
                        new ColumnDefinition(GridLength.Star),
                        new ColumnDefinition(GridLength.Star)
                    }
                };
                var textBox = new TextBox() { };

                if (x.Activities is null)
                {
                    x.Activities = new List<IYearMonthStaffActivityBase>();
                }

                int i = x.Activities.Where(act => act.StaffActivityId == activity.Id).ToList().Count;
                if (x.Activities.Count == 0 || x.Activities.Where(act => act.StaffActivityId == activity.Id).ToList().Count == 0) 
                {
                    x.Activities.Add(new YearMonthStaffActivityUI
                    {
                        Year = year, Month = month,
                        StaffActivityId = activity.Id,
                        StaffMemberId = x.StaffMember!.Id,
                        SumPaid = 0
                    });

                    textBox.Text = 0.ToString();

                }
                var activityInDb = x.Activities.Single(act => act.StaffActivityId == activity.Id);

                int index = x.Activities.IndexOf(activityInDb);


                textBox.Bind(TextBox.TextProperty, new Binding($"Activities[{index}].SumPaid"));
                textBox.KeyDown += UIUtilities.TextBox_NumericInput_KeyDown; //doesnt work???

                grid.Children.Add( textBox );
                Grid.SetColumn(textBox, 0);

                CheckBox WasPaidCheckBox = new CheckBox();
                
                var binding = new Binding($"Activities[{index}].WasPaid")
                {
                    Mode = BindingMode.TwoWay,
                    Converter = new IntToBoolConverter(),  // Apply the converter here
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // Optional: Immediately update the source
                };

                WasPaidCheckBox.Bind(CheckBox.IsCheckedProperty, binding);

                grid.Children.Add(WasPaidCheckBox);
                Grid.SetColumn(WasPaidCheckBox, 1);

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