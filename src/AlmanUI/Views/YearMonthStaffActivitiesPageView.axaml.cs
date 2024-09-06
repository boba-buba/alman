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
public partial class YearMonthStaffActivitiesPageView : UserControl, IUpdateDataGridWithoutParams
{
    

    /// <summary>
    /// ctor.
    /// </summary>
    public YearMonthStaffActivitiesPageView()
    {
        InitializeComponent();
        InitDataGrid();
        Mediator.Mediator.Instance.Notify += OnNotify;
    }

    /// <summary>
    /// Process notification that came from the Mediator.
    /// </summary>
    /// <param name="message">Message from view model.</param>
    /// <param name="year">1st param.</param>
    /// <param name="month">2nd param.</param>
    private void OnNotify(string message)
    {
        if (message == "UpdateYearMonthStaffActivities")
            UpdateDataGrid();
    }


    public void UpdateDataGrid()
    {
        YearMonthStaffActivitiesMainDataGrid.Columns.Clear();
        InitDataGrid();
    }

    /// <summary>
    /// Initialize data grid of the table for the UI view.
    /// </summary>
    public void InitDataGrid()
    {
        YearMonthStaffActivitiesMainDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);

        YearMonthStaffActivitiesMainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.StaffResources.StaffFirstName, Binding = new Binding("StaffMember.FirstName"), IsReadOnly = true });
        YearMonthStaffActivitiesMainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.StaffResources.StaffLastName, Binding = new Binding("StaffMember.LastName"), IsReadOnly = true });

        IReadOnlyList<IStaffActivityBase> staffActivitiesLocal = StaffActivitiesControl.GetItems();
        
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

                

                var activityInDb = x.Activities!.Single(act => act.StaffActivityId == activity.Id);
                int index = x.Activities!.IndexOf(activityInDb);
                
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
        
    }
}