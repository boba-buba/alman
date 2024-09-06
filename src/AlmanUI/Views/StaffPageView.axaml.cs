using Alman.SharedDefinitions;
using Alman.SharedModels;
using Avalonia.Controls;
using Avalonia.Data;

namespace AlmanUI.Views;

/// <summary>
/// View for staff page.
/// </summary>
public partial class StaffPageView : UserControl, IInitDataGrid
{
    /// <summary>
    /// ctor.
    /// </summary>
    public StaffPageView()
    {
        InitializeComponent();
        InitDataGrid();
    }

    public void InitDataGrid()
    {
        StaffMembersMainDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);

        StaffMembersMainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.CommonResources.FirstName, Binding = new Binding("FirstName") });
        StaffMembersMainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.CommonResources.LastName, Binding = new Binding("LastName") });
        UIControlElements.AddCheckBoxToGrid<IStaffMemberBase>(StaffMembersMainDataGrid, AlmanUI.Resources.CommonResources.IsActive, "State");
        UIControlElements.AddNumericUpDownToGrid<IStaffMemberBase>(StaffMembersMainDataGrid, AlmanUI.Resources.CommonResources.StartMonth, "StartMonth", (int)Months.January, (int)Months.December);
        UIControlElements.AddNumericUpDownToGrid<IStaffMemberBase>(StaffMembersMainDataGrid, AlmanUI.Resources.CommonResources.StartYear, "StartYear", (int)YearsBounds.StartYear, (int)YearsBounds.EndYear);
        StaffMembersMainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.StaffResources.PositionName, Binding = new Binding("PositionName") });
        UIControlElements.AddNumericTextBoxToGrid<IStaffMemberBase>(StaffMembersMainDataGrid, AlmanUI.Resources.StaffResources.PositionSalary, "PositionSalary");
    }
}