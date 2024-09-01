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
        UIControlElements.AddNumericUpDownToGrid<IStaffMemberBase>(StaffMembersMainDataGrid, AlmanUI.Resources.CommonResources.StartMonth, "StartMonth", 1, 12);
        UIControlElements.AddNumericUpDownToGrid<IStaffMemberBase>(StaffMembersMainDataGrid, AlmanUI.Resources.CommonResources.StartYear, "StartYear", 2000, 2100);
        StaffMembersMainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.StaffResources.PositionName, Binding = new Binding("PositionName") });
        UIControlElements.AddNumericTextBoxToGrid<IStaffMemberBase>(StaffMembersMainDataGrid, AlmanUI.Resources.StaffResources.PositionSalary, "PositionSalary");
    }
}