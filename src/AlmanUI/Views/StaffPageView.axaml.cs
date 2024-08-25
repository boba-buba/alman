using Alman.SharedModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml;

namespace AlmanUI.Views;

public partial class StaffPageView : UserControl
{
    public StaffPageView()
    {
        InitializeComponent();
        InitStaffMainDataGrid();
    }

    private void InitStaffMainDataGrid()
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